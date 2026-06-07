using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class SettingPlayerManager : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    private GameObject[] players;
    private List<CharacterInfoSetting> students  = new List<CharacterInfoSetting>();
    private List<CharacterInfoSetting> professors = new List<CharacterInfoSetting>();
    private List<int> roomNumbers = new List<int>();
    private List<CharacterInfoSetting> ps  = new List<CharacterInfoSetting>();
    private int playersNum;
    private GameObject avatarPlayer; 
    public Sprite[] avatarImages;
    public RuntimeAnimatorController[] controllers;
    List<string> animals = new List<string>();
    public int animalIndex;
    public Sprite[] animalImages;
    public Image[] profileImages;
    public Text[] profileNicknames;
    private int ran;
    private string animal;
    public int myProfileIndex;

    private void Awake() 
    {
        MyAwake();    
    }

    public void MyAwake() 
    {
        PV = gameObject.GetComponent<PhotonView>();    
    }

    public void SetRoom()
    {
        players = GameManagerScript.instance.getPlayers();

        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].GetComponent<CharacterInfoSetting>().role == "student" || players[i].GetComponent<CharacterInfoSetting>().role == "graduate")
            {
                students.Add(players[i].GetComponent<CharacterInfoSetting>());
            }
            else
            {
                professors.Add(players[i].GetComponent<CharacterInfoSetting>());
            }
        }
        roomNumbers = RoomSetting(students.Count);

        for(int i = 0; i < students.Count; i++)
        {
            students[i].GetComponent<PhotonView>().RPC("SetRoom", RpcTarget.AllBuffered, roomNumbers[i]);
        }

        for(int i = 0; i < professors.Count; i++)
        {
            //professors[i].GetComponent<PhotonView>().RPC("SetProfessorReady", RpcTarget.AllBuffered);
        }
        print("student : " + students.Count + "professor : " + professors.Count);
    }

    private List<int> RoomSetting(int num)
    {
        List<int> numbers = new List<int>();
        List<int> roomNums = new List<int>();
        for(int i = 0; i < num; i++)
        {
            numbers.Add(i+1);
        }
        for(int stu = 0; stu < num ; stu ++)
        {
            int ran = UnityEngine.Random.Range(0, numbers.Count);
            roomNums.Add(numbers[ran]);
            numbers.RemoveAt(ran);
        }
        return roomNums;
    }

    public void SetRole()
    {
        players = GameManagerScript.instance.getPlayers();

        for(int i = 0; i < players.Length; i++)
        {
            ps.Add(players[i].GetComponent<CharacterInfoSetting>());
        }
        playersNum = ps.Count;
        
        if(playersNum > 0)
        {
            List<string> roles = new List<string>();
            if(playersNum == 5)
            {
                roles = ShuffleRoles(3,1,1);
            }
            else if(playersNum == 6)
            {
                roles = ShuffleRoles(4,1,1);
                
            }
            else if(playersNum == 7)
            {
                roles = ShuffleRoles(4,1,2);
            }
            else if(playersNum == 8)
            {
                roles = ShuffleRoles(5,1,2);
            }
            //for test 3 players or 2 or 1.
            else if(playersNum == 2 || playersNum == 3)
            {
                roles = ShuffleRoles(1,1,1);
            }
            else
            {
                roles = ShuffleRoles(2,2,2);
            }
            
            for(int i = 0; i < playersNum; i++)
            {
                GameManagerScript.instance.setRoles(ps[i].nickName, roles[i]);
            }
        }
    }

    private List<string> ShuffleRoles(int stu, int grad, int prof)
    {
        List <string> roleArray = new List <string>();
        List <string> playersRoleArray = new List<string>();
        int total = stu + grad + prof;
        
        for(int i = 0; i < stu; i++)
        {
            roleArray.Add("student");
        }

        for(int i = 0; i < grad; i++)
        {
            roleArray.Add("graduate");
        }

        for(int i = 0; i < prof; i++)
        {
            roleArray.Add("professor");
        }

        
        for(int player = 0; player < total ; player ++)
        {
            int ran = UnityEngine.Random.Range(0, roleArray.Count);
            playersRoleArray.Add(roleArray[ran]);
            roleArray.RemoveAt(ran);
        }
        
        return playersRoleArray;
    }

    
    //Animal Setting(Avatar)
    public void AnimalCharacterSetting(string nickName)
    {
        string[] animalArray = {"bear", "bunny", "cat", "fox", "goat", "mouse", "puppy", "raccoon"};

        animals.Add("bear");
        animals.Add("bunny");
        animals.Add("cat");
        animals.Add("fox");
        animals.Add("goat");
        animals.Add("mouse");
        animals.Add("puppy");
        animals.Add("raccoon");
        
        StartCoroutine(FindAndSetCharacter());

        animal = animals[ran];
        animalIndex = Array.IndexOf(animalArray, animal);

        print(nickName + " " + animal);

        PV.RPC("SetAnimalAvatar", RpcTarget.AllBuffered, animal, animalIndex, nickName);
    }

    IEnumerator FindAndSetCharacter()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        while(!(PhotonNetwork.PlayerList.Length == players.Length)) yield return null;

        print(PhotonNetwork.PlayerList.Length);
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            print("! " + players[i].GetComponent<CharacterInfoSetting>().myAnimalCharacter + " !");
            if(animals.Contains(players[i].GetComponent<CharacterInfoSetting>().myAnimalCharacter))
            {
                int index = animals.FindIndex(a => a.Contains(players[i].GetComponent<CharacterInfoSetting>().myAnimalCharacter));
                animals.RemoveAt(index);
                print(players[i].GetComponent<CharacterInfoSetting>().nickName + " " + players[i].GetComponent<CharacterInfoSetting>().myAnimalCharacter);
            }
        }
        ran = UnityEngine.Random.Range(0, animals.Count);
    }

    [PunRPC]
    void SetAnimalAvatar(string animalName, int imageIndex, string playerName)
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
        avatarPlayer = null;

        for(int i = 0; i < characters.Length; i++)
        {
            if(playerName == characters[i].GetComponent<CharacterInfoSetting>().nickName)
            {
                avatarPlayer = characters[i];
                avatarPlayer.GetComponent<CharacterInfoSetting>().myAnimalCharacter = animalName;
                avatarPlayer.GetComponent<CharacterInfoSetting>().animalInd = imageIndex;
            }
        }
        if(avatarPlayer != null)
        {
            avatarPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = avatarImages[imageIndex];
            avatarPlayer.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = controllers[imageIndex];
        }
    }


    //Animal Profile Setting
    public void SetCharacter()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        SetAnimalCharacter(players.Length);
    }

    void SetAnimalCharacter(int playerNum)
    {
        Transform playerProfile = GameManagerScript.instance.readyObj.transform.Find("WindowUI/GameInfoWindow/Panel/PlayersInfo");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < 8; i++)
        {
            profileImages[i] = playerProfile.GetChild(i).GetComponent<Image>();
            profileNicknames[i] = playerProfile.GetChild(i).GetChild(0).GetComponent<Text>();
        }
        SetPlayersProfile(playerNum);

        for(int i = 0; i < players.Length; i++)
        {
            profileNicknames[i].text = players[i].GetComponent<CharacterInfoSetting>().nickName;
            profileImages[i].sprite = AnimalImageMatch(players[i].GetComponent<CharacterInfoSetting>().myAnimalCharacter);
            if(players[i].GetComponent<CharacterInfoSetting>().nickName == GameManagerScript.instance.getMyPlayerInfo().nickName) myProfileIndex = i;
        }
        
    }

    private Sprite AnimalImageMatch(string animal)
    {
        if (animal == "bear")
        {
            return animalImages[0];
        }
        else if (animal == "bunny")
        {
            return animalImages[1];
        }
        else if (animal == "cat")
        {
            return animalImages[2];
        }
        else if (animal == "fox")
        {
            return animalImages[3];
        }
        else if (animal == "goat")
        {
            return animalImages[4];
        }
        else if (animal == "mouse")
        {
            return animalImages[5];
        }
        else if (animal == "puppy")
        {
            return animalImages[6];
        }
        else if (animal == "raccoon")
        {
            return animalImages[7];
        }
        else return animalImages[0];
    }

    public void SetPlayersProfile(int playerNum)
    {
        print(playerNum);
        for(int i = 0; i < playerNum; i ++)
        {
            profileImages[i].gameObject.SetActive(true);
            profileNicknames[i].gameObject.SetActive(true);
        }

        if(playerNum == 8) return;

        for(int i = 7; i > playerNum - 1; i--)
        {
            profileImages[i].gameObject.SetActive(false);
            profileNicknames[i].gameObject.SetActive(false);
        }
    }
}
