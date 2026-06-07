using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class SettingPlayers : MonoBehaviourPunCallbacks
{
    private GameObject[] players;
    private List<CharacterInfoSetting> students  = new List<CharacterInfoSetting>();
    private List<CharacterInfoSetting> professors = new List<CharacterInfoSetting>();
    private List<int> roomNumbers = new List<int>();

    public void setPlayerRoom()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

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
            professors[i].GetComponent<PhotonView>().RPC("SetProfessorReady", RpcTarget.AllBuffered);
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
            int ran = Random.Range(0, numbers.Count);
            roomNums.Add(numbers[ran]);
            numbers.RemoveAt(ran);
        }
        return roomNums;
    }

    public void setAnimalCharacter()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PhotonView>().RPC("SetAnimalCharacter", RpcTarget.All, i, players.Length);
        }

    }
}
