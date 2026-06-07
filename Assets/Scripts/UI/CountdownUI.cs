using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CountdownUI : MonoBehaviourPunCallbacks
{
    //씬 이동 시 카운트 다운, 씬 이동 안내하도록 만드는 스크립트, 기본 5초로 설정
    [SerializeField] private float setCount = 5.0f;
    [SerializeField] private Text countdownText;
    [SerializeField] private Text announceScene;
    [SerializeField] private GameObject MasterGameStart;
    [SerializeField] private GameObject MasterMissionSet;
    [SerializeField] private GameObject AssignmentButton;
    public GameObject nicknameChangeWindow;
    public CharacterInfoSetting characterInfo;
    public bool countDown = false;
    public PhotonView PV;
    public bool countdownSet;
    public bool letGameStart;
    public int missionNumber = 0;
    private MissionManager missionManager;
    private bool isDone = false;
    public bool isCounting = false;
    private bool retire = false;

    public SettingWindow settingWindow;

    public void MyAwake()
    {
        countdownText.text = setCount.ToString();
        // settingRole = GetComponent<SettingRole>();
        // settingPlayers = GetComponent<SettingPlayers>();
        // missionProgressBar = GameManagerScript.instance.readyObj.transform.Find("MainMobileUI/MissionProgressBar").gameObject;
        // missionManager = GameManagerScript.instance.readyObj.transform.Find("OnlyReadyScene/CSVManager").GetComponent<MissionManager>();

        letGameStart = false;

        countdownSet = false;

        MasterGameStart.SetActive(PhotonNetwork.IsMasterClient ? true : false);
        MasterMissionSet.SetActive(PhotonNetwork.IsMasterClient ? true : false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManagerScript.instance.updatePause) return;
        if (retire) return;
        //현재 플레이어가 어디에 있는지 위치 정보를 출력하고 난 후에 카운트 다운
        if (countdownSet && characterInfo.roomInfoPrintDone && letGameStart)
        {
            if (setCount > 0)
            {
                isCounting = true;
                setCount -= Time.deltaTime;
            }
            else if (setCount <= 0)
            {
                countDown = true;
            }
            countdownText.gameObject.SetActive(true);
            countdownText.text = Mathf.Ceil(setCount).ToString();
            if (countDown && !isDone)
            {
                WaitSecond();
            }
        }
    }

    //카운트 다운 도와주는 코루틴
    void WaitSecond()
    {
        isDone = true;
        countdownText.gameObject.SetActive(false);
        announceScene.gameObject.SetActive(true);
        
        print("Move to Spawn");

        announceScene.gameObject.SetActive(false);
        isCounting = false;
        retire = true;
        GameManagerScript.instance.GameStart();
    }

    public void countDownDone()
    {
        print("TTTTTestsTTTTTT");
        // Transform mpb = missionProgressBar.transform;

        // Image img = mpb.GetChild(0).GetComponent<Image>();
        // Color col = img.color;
        // img.color = new Color(col.r, col.g, col.b, 1);

        // img = mpb.GetChild(1).GetChild(0).GetComponent<Image>();
        // col = img.color;
        // img.color = new Color(col.r, col.g, col.b, 1);

        // img = mpb.GetChild(2).GetChild(0).GetComponent<Image>();
        // col = img.color;
        // img.color = new Color(col.r, col.g, col.b, 1);

        // missionProgressChecker.gameStart();

        // Destroy(gameObject);
        
        // transform.parent.gameObject.tag = "ReadyObject";
        // transform.parent.gameObject.SetActive(false);
    }

    public void GameStartClick()
    {
        // Game Start
        PV.RPC("LetGameStartAll", RpcTarget.AllBuffered);
        //GameManagerScript.instance.OnlyForMaster();
        // settingRole.setRoles();
        // settingPlayers.setPlayerRoom();

        // if (missionNumber == 0) missionNumber = 6;
        // missionManager.masterMissionSetting(missionNumber);
        // settingPlayers.setAnimalCharacter();
        // settingWindow.leaveRoomBtnOff();
        // PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    [PunRPC]
    public void LetGameStartAll()
    {
        letGameStart = true;

        // All GMS got their player list and client player after clicking Game Start btn
        // GameManagerScript.instance.FindPlayers();
    }




    // Nickname Change
    // public void OnNicknameChangeWindowBtn()
    // {
    //     if(!isCounting)
    //         nicknameChangeWindow.GetComponent<WindowAnimation>().OnAnimUI();
    // }

    // public void OffNicknameChangeWindowBtn()
    // {
    //     nicknameChangeWindow.GetComponent<WindowAnimation>().OffAnimUI();
    // }

    public InputField newNickname;
    // public void NicknameChangeBtn()
    // {
    //     // Length Rejection
    //     int newNickLength = newNickname.text.Length;
    //     if (!(1 <= newNickLength && newNickLength <= 13))
    //     {
    //         newNickname.placeholder.GetComponent<Text>().text = "1~13자 이내의 닉네임을 입력해주세요";
    //         newNickname.text = "";
    //         return;
    //     }
    //     // Duplicate Rejection
    //     foreach (Player p in PhotonNetwork.CurrentRoom.Players.Values)
    //     {
    //         if (p.NickName == newNickname.text)
    //         {
    //             newNickname.placeholder.GetComponent<Text>().text = "같은 닉네임이 이미 존재합니다";
    //             newNickname.text = "";
    //             return;
    //         }
    //     }

    //     // Go
    //     // Photon Nickname Change
    //     GameManagerScript.instance.readyObj.transform.Find("OnlyReadyScene/ReadyChatManager").GetComponent<ReadyChat>().nicknameChanged(PhotonNetwork.LocalPlayer.NickName, newNickname.text);
    //     PhotonNetwork.LocalPlayer.NickName = newNickname.text;

    //     // Player Nickname Text Change
    //     GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //     GameObject player = players[0];
    //     foreach(GameObject p in players) if(p.GetComponent<PhotonView>().IsMine) player = p;
    //     // player.GetComponent<CharacterInfoSetting>().nickName = newNickname.text;
    //     player.GetComponent<CharacterInfoSetting>().setNickName(newNickname.text); // CharacterInfoSetting
    //     newNickname.text = "";

    //     OffNicknameChangeWindowBtn();
    // }
}
