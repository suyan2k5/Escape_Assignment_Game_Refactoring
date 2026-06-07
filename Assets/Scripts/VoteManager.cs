using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class VoteManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject lightOff;
    [SerializeField] private Image electedPlayerImage;
    [SerializeField] private RuntimeAnimatorController[] controllers;
    [SerializeField] private Sprite[] playerImg;
    [SerializeField] private Sprite[] playerStandImg;
    [SerializeField] private Sprite[] playerWalkImg;
    [SerializeField] private GameObject[] voteButtons;
    public GameObject voteWindow;
    public bool isVoting = false;
    private int playerNum;
    private int shouldVote;
    private SettingPlayerManager settingPlayerManager;
    
    private Dictionary<string, int> voteToPlayer = new Dictionary<string, int>();
    private int votedNum;
    private GameObject[] players;
    public float voteTime = 10.0f;
    public Text timeText;
    public GameObject resultWindow;
    private GameObject myPlayer;
    private PhotonView PV;
    public Text resultText;
    private CharacterInfoSetting electedPlayer;
    private GameObject player;
    private bool isShowingResult = false;
    private int animalIndex;

    private void Awake() 
    {
        PV = gameObject.GetComponent<PhotonView>();  
    }

    void Update()
    {
        if(isVoting)
        {
            if(!myPlayer.GetComponent<CharacterInfoSetting>().isAlive)
            {
                for(int j = 0; j < voteButtons.Length; j++)
                {
                    voteButtons[j].GetComponent<Button>().interactable = false;
                    voteButtons[j].GetComponent<Image>().color = Color.gray;
                }
            }
            if(voteTime > 0)
            {
                voteTime -= Time.deltaTime;
            }
            else if(voteTime <= 0)
            {
                timeText.gameObject.SetActive(false);
                if(!isShowingResult)
                {
                    voteWindow.SetActive(false);
                    voteResult();
                    isShowingResult = true;
                }
            }
            timeText.gameObject.SetActive(true);
            timeText.text = Mathf.Ceil(voteTime).ToString();
        }

        if(votedNum == shouldVote && votedNum != 0)
        {
            if(!isShowingResult)
            {
                voteWindow.SetActive(false);
                voteResult();
                isShowingResult = true;
            }
        }
    }

    public void voteSetting()
    {
        removeAllVoteData();
        isVoting = true;
        voteWindow.SetActive(true);
        players = GameObject.FindGameObjectsWithTag("Player");
        playerNum = players.Length;
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].GetComponent<PhotonView>().IsMine) myPlayer = players[i];
            if(players[i].GetComponent<CharacterInfoSetting>().isAlive)
            {
                shouldVote ++;
                voteToPlayer.Add(players[i].GetComponent<CharacterInfoSetting>().nickName, 0);
            }
        }
        voteImageSetting();
    }

    private void voteImageSetting()
    {
        for(int i = playerNum; i < 8; i++)
        {
            voteButtons[i].SetActive(false);
        }

        for(int i = 0; i < playerNum; i++)
        {
            voteButtons[i].GetComponent<Image>().sprite = playerImg[imageMatching(players[i])];
            voteButtons[i].GetComponentInChildren<Text>().text = players[i].GetComponent<CharacterInfoSetting>().nickName;
            if(players[i].GetComponent<CharacterInfoSetting>().isAlive == false)
            {
                voteButtons[i].GetComponent<Button>().interactable = false;
                voteButtons[i].GetComponent<Image>().color = Color.gray;
            }
        }
    }

    public void checkVote(int buttonIndex)
    {
        if(myPlayer.GetComponent<CharacterInfoSetting>().isAlive)
        {
            PV.RPC("checkVoteRPC", RpcTarget.AllBuffered, buttonIndex);
            if(myPlayer.GetComponent<PhotonView>().IsMine)
            {
                voteWindow.SetActive(false);
            }
        }
        else
        {
            for(int i = 0; i < voteButtons.Length; i++)
            {
                voteButtons[i].GetComponent<Button>().interactable = false;
                voteButtons[i].GetComponent<Image>().color = Color.gray;
            }
        }
    }

    [PunRPC]
    void checkVoteRPC(int _buttonNumber)
    {
        string playerName = players[_buttonNumber].GetComponent<CharacterInfoSetting>().nickName;
        voteToPlayer[playerName] = voteToPlayer[playerName] + 1;
        votedNum ++;
    }

    private int imageMatching(GameObject player)
    {
        string characterName = player.GetComponent<CharacterInfoSetting>().myAnimalCharacter;
        if(characterName == "bear")
        {
            animalIndex = 0;
            return animalIndex;
        }
        else if(characterName == "bunny")
        {
            animalIndex = 1;
            return animalIndex;
        }
        else if(characterName == "cat")
        {
            animalIndex = 2;
            return animalIndex;
        }
        else if(characterName == "fox")
        {
            animalIndex = 3;
            return animalIndex;
        }
        else if(characterName == "goat")
        {
            animalIndex = 4;
            return animalIndex;
        }
        else if(characterName == "mouse")
        {
            animalIndex = 5;
            return animalIndex;
        }
        else if(characterName == "puppy")
        {
            animalIndex = 6;
            return animalIndex;
        }
        else if(characterName == "raccoon")
        {
            animalIndex = 7;
            return animalIndex;
        }
        else return animalIndex;
    }

    //result
    public void voteResult()
    {
        //동표 : 단상에 아무도 올라가지 않고 3초 후 화면 어두워짐
        //투표 인원이 한 명도 없을 경우 : 단상에 아무도 올라가지 않음
        string result = analyzeResult();
        print(result);
        resultWindow.SetActive(true);
        if(votedNum == 0 || result == "Same")
        {
            StartCoroutine(showResultPanel(0, player));
        }

        //학생일 때 : 올라가고 2초 후에 글씨 뜸 (ㅇㅇㅇ는 학생이었습니다), 불투명도 조절, 2초 후에 내려옴
        //교수나 대학원생일 때 : 올라가고 2초 후에 글씨 뜸(ㅇㅇㅇ는 교수/대학원생이었습니다), 불투명도 조절 2초 후에 처형
        else
        {
            for(int i = 0; i < playerNum; i++)
            {
                if(players[i].GetComponent<CharacterInfoSetting>().nickName == result)
                {
                    electedPlayer = players[i].GetComponent<CharacterInfoSetting>();
                    player = players[i];
                }
            }
            if(electedPlayer != null)
            {
                if(electedPlayer.role == "student")
                {
                    StartCoroutine(showResultPanel(1, player));
                }
                else if(electedPlayer.role == "professor")
                {
                    StartCoroutine(showResultPanel(2, player));;
                }
                else
                {
                    StartCoroutine(showResultPanel(3, player));
                }
            }
        }
    }

    IEnumerator showResultPanel(int num, GameObject player)
    {
        electedPlayerImage.gameObject.SetActive(true);
        settingPlayerManager = GameManagerScript.instance.settingPlayerManager;
        Color c = electedPlayerImage.color;
        c.a = 0.5f;
        Vector2 pos = electedPlayerImage.GetComponent<RectTransform>().anchoredPosition;

        if(num == 0)
        {
            electedPlayerImage.gameObject.SetActive(false);
            resultText.text = "아무도 지목되지 않았습니다.";
            yield return new WaitForSeconds(1.0f);
            lightOff.SetActive(true);
        }
        else if(num == 1)
        {
            //Animator animator = electedPlayerImage.GetComponent<Animator>();
            int pNum = imageMatching(electedPlayer.gameObject);
            electedPlayerImage.sprite = playerStandImg[pNum];
            electedPlayerImage.transform.GetChild(0).gameObject.SetActive(true);
            //electedPlayerImage.GetComponent<Animator>().runtimeAnimatorController = controllers[animalIndex];
            yield return new WaitForSeconds(0.5f);
            electedPlayerImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-157, -109);
            float x = electedPlayerImage.GetComponent<RectTransform>().anchoredPosition.x;
            lightOff.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            for(int i = 0; i < 5; i ++)
            {
                yield return new WaitForSeconds(0.2f);
                x -= 50;
                electedPlayerImage.sprite = playerStandImg[pNum];
                yield return new WaitForSeconds(0.2f);
                electedPlayerImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -109);
                electedPlayerImage.sprite = playerWalkImg[pNum];
            }
            yield return new WaitForSeconds(0.2f);
            electedPlayerImage.sprite = playerStandImg[pNum];
            yield return new WaitForSeconds(1.0f);
            electedPlayerImage.color = c;
            resultText.text = electedPlayer.nickName + "는 학생이었습니다.";
        }
        else if(num == 2)
        {
            electedPlayerImage.sprite = playerStandImg[imageMatching(electedPlayer.gameObject)];
            yield return new WaitForSeconds(1.0f);
            electedPlayerImage.color = c;
            resultText.text = electedPlayer.nickName + "는 교수였습니다.";
            resultText.color = Color.red;
            if(myPlayer == player)
            {
                GameManagerScript.instance.playerDieManager.GetComponent<PhotonView>().RPC("spreadDeadInfoWithoutInstantiate", RpcTarget.All, player.GetComponent<CharacterInfoSetting>().nickName, settingPlayerManager.myProfileIndex);
            }
        }
        else
        {
            electedPlayerImage.sprite = playerStandImg[imageMatching(electedPlayer.gameObject)];
            yield return new WaitForSeconds(1.0f);
            electedPlayerImage.color = c;
            resultText.text = electedPlayer.nickName + "는 대학원생이었습니다.";
            resultText.color = Color.red;
            if(myPlayer == player)
            {
                GameManagerScript.instance.playerDieManager.GetComponent<PhotonView>().RPC("spreadDeadInfoWithoutInstantiate", RpcTarget.All, player.GetComponent<CharacterInfoSetting>().nickName, settingPlayerManager.myProfileIndex);
            }
        }
        yield return new WaitForSeconds(2.0f);
        lightOff.SetActive(false);
        electedPlayerImage.GetComponent<RectTransform>().anchoredPosition = pos;
        resultWindow.SetActive(false);
        electedPlayerImage.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<ChatManager>().offMeetingWindow();
    }

    private string analyzeResult()
    {
        string maxValName = "";
        int maxVal = 0;
        //최다 득표
        foreach(KeyValuePair<string, int> items in voteToPlayer)
        {
            string name = items.Key;
            int value = items.Value;
            if(value > maxVal)
            {
                maxValName = name;
                maxVal = value;
            } 
        }
        //동표 있는지
        foreach(KeyValuePair<string, int> items in voteToPlayer)
        {
            if(items.Value == maxVal)
            {
                //동표 있음
                if(items.Key != maxValName)
                {
                    return "Same";
                }
            }
        }
        return maxValName;
    }

    private void removeAllVoteData()
    {
        voteTime = 10.0f;
        voteToPlayer.Clear();
        isShowingResult = false;
        voteButtons[7].SetActive(true);
        voteButtons[6].SetActive(true);
        voteButtons[5].SetActive(true);
        voteButtons[4].SetActive(true);
        voteButtons[3].SetActive(true);
        voteButtons[2].SetActive(true);
        voteButtons[1].SetActive(true);
        playerNum = 0;
        votedNum = 0;
        shouldVote = 0;
        electedPlayerImage.sprite = null;
        resultText.text = "";
        for(int i = 0; i < 8; i ++)
        {
            voteButtons[i].SetActive(true);
            voteButtons[i].GetComponent<Button>().interactable = true;
            voteButtons[i].GetComponent<Image>().color = Color.white;
        }
    }
}
