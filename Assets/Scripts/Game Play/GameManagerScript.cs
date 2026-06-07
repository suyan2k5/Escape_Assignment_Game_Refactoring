using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class GameManagerScript : MonoBehaviourPunCallbacks
{
    //게임을 전체적으로 관리하는 스크립트
    //게임 시작, 종료, 강의실 번호 지정, 위치 업데이트 담당 -> 수정 예정
    [SerializeField] private SceneChanger SceneChanger;
    [SerializeField] GameObject[] mainButtons;
    public static GameManagerScript instance;     //싱글톤 사용
    public bool isGameStarted;
    private int playerRoom;
    private PhotonView PV;
    private GameObject[] players;
    private CharacterInfoSetting[] playersInfo;
    private GameObject myPlayer;
    private CharacterInfoSetting myPlayerInfo;
    private float popUpTime = 0.5f;
    [SerializeField] Sprite[] studentWinSprite;
    [SerializeField] Sprite professorWinSprite;
    [SerializeField] Sprite graduateWinSprite;
    [SerializeField] private AudioClip readyMusic;
    private AudioSource music;
    public SceneChanger gameEndSceneChanger;

    public bool updatePause = true;
    public GameObject readyObjPref;
    public GameObject readyObj;
    public int missionNumber;
    [Header("Manager Scripts")]
    public GameObject readyManager;
    public SettingPlayerManager settingPlayerManager;
    public UIManager uiManager;
    public PlayerDie playerDieManager;
    public PlayerUISettingScript playerUISettingScript;
    public Dictionary<string, int> PlayerList = new Dictionary<string, int>();
    [Header("Others")]
    public bool getProfAuthority = false;
    public bool isReadyObjDestroyed = false;

    void Awake()
    {
        instance = this;
        isGameStarted = false;
        DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
        players = new GameObject[1];
        PlayerList.Add("student", 0);
        PlayerList.Add("graduate", 0);
        PlayerList.Add("professor", 0);
    }

    //게임 시작
    public void GameStart()
    {
        //if(PhotonNetwork.IsMasterClient)    OnlyForMaster();
        PhotonNetwork.LoadLevel("Spawn_SY");
        readyManager.GetComponent<SceneChanger>().StartCoroutine("moveToSpawn");
        string sceneName = "Spawn";
        myPlayerInfo.currentSceneName = sceneName;
    
        PhotonNetwork.CurrentRoom.IsOpen = false;
        
        setMusicStop();
        uiManager.SettingAfterStart();
        FindPlayers();
        settingPlayerManager.SetCharacter();
        getMyPlayerInfo().SettingWithMyRole();
        readyObj.tag = "ReadyObject";
        readyObj.transform.GetChild(0).gameObject.SetActive(false);

        Camera.main.GetComponent<CameraPosition>().cameraSetting(sceneName);
        GameObject miscanv = GameObject.Find(sceneName + "MissionCanvas");
        if(miscanv != null) miscanv.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Move Camera").GetComponent<Camera>(); else print("CurCanvas Missing");
    }

    public void OnlyForMaster()
    {
        settingPlayerManager.SetRole();
        settingPlayerManager.SetRoom();

        MissionManager.mission.masterMissionSetting(missionNumber);
        //settingPlayerManager.SetCharacter();
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    //게임 진행
    public void SetAssignment()
    {

    }

    //게임 종료 준비 및 재시작
    public void SetEnding(int endingNum)
    {
        switch(endingNum)
        {
            //학생 승리
            case 1:
                break;
            //대학원생 승리
            case 2:
                break;
            //교수 승리
            case 3:
                break;
        }
    }

    //게임 종료
    public void ExitGame()
    {
        StartCoroutine("playerLeaveRoom");
        GameObject[] objects = FindObjectsOfType<GameObject>();
        for(int i = 0; i < objects.Length; i++)
        {
            if(objects[i].name != "PhotonMono")  Destroy(objects[i]);
        }
    }

    IEnumerator playerLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        while(PhotonNetwork.InRoom)     yield return null;

        print("test leave room");
        PhotonNetwork.LoadLevel("Mainmenu");

        GameObject[] objects = FindObjectsOfType<GameObject>();
        for(int i = 0; i < objects.Length; i++)
        {
            if(objects[i].tag != "MainObject" && objects[i].name != "Main Camera" && objects[i].name != "PhotonMono")
            {
                Destroy(objects[i]);
            }
        }
    }

    // when a client entered the room, make that player's GMS PhotonView enable
    public void manageStart()
    {
        PV.enabled = true;
    }

    //현재 상태를 게임 시작 상태로 지정
    public void setGameStart()
    {
        this.isGameStarted = true;
        setReadyMusic();
    }

    public void setReadyMusic()
    {
        music = readyObj.transform.Find("Audio Source").GetComponent<AudioSource>();
        music.clip = readyMusic;
        music.Play();
    }

    public void setMusicStop()
    {
        music.Stop();
    }

    // Find Players
    [PunRPC]
    public void FindPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if(players == null) return;
        playersInfo = new CharacterInfoSetting[players.Length];
        for(int i = 0; i < players.Length; i++)
        {
            playersInfo[i] = players[i].GetComponent<CharacterInfoSetting>();
            if(players[i].GetComponent<PhotonView>().IsMine)
            {
                myPlayer = players[i];
                myPlayerInfo = playersInfo[i];
            }
        }
        print("Find Player Done. Player Number = " + players.Length + " / Player Info Number = " + playersInfo.Length);
        // readyObj.transform.Find("MainMobileUI/TestLog").GetComponent<Text>().text += "Find Player Done. Player Number = " + players.Length + " / Player Info Number = " + playersInfo.Length + '\n';
    }

    // It runs the RPC method "FindPlayers"
    // mode 'a' means the rpc target is All, and 'o' means Others
    public void FindPlayersRPC(char mode)
    {
        if(mode.CompareTo('a') == 0)
            PV.RPC("FindPlayers", RpcTarget.All);
        else if(mode.CompareTo('o') == 0)
            PV.RPC("FindPlayers", RpcTarget.Others);
    }

    // returns the all players list
    public GameObject[] getPlayers()
    {
        return players;
    }

    // returns my client's player
    public GameObject getMyPlayer()
    {
        return myPlayer;
    }

    // returns the all players' CIS script list
    public CharacterInfoSetting[] getPlayersInfo()
    {
        return playersInfo;
    }

    // returns CIS script of my client's player
    public CharacterInfoSetting getMyPlayerInfo()
    {
        return myPlayerInfo;
    }

    // runs playerNicknameChangedRPC method
    public void playerNicknameChanged(string fromNick, string toNick)
    {
        PV.RPC("playerNicknameChangedRPC", RpcTarget.AllBuffered, fromNick, toNick);
    }

    [PunRPC]
    public void playerNicknameChangedRPC(string fromNick, string toNick)
    {
        FindPlayers();
        GameObject p = null;
        CharacterInfoSetting pinfo = null;
        for(int i = 0; i < players.Length; i++)
        {
            if(playersInfo[i] == null) Debug.LogError(i + "-th playersInfo Null");
            pinfo = playersInfo[i];
            if(pinfo.nickName == fromNick)
            {
                p = players[i];
                break;
            }
        }
        pinfo.nickName = toNick;
        p.transform.Find("Canvas").GetChild(0).GetComponent<Text>().text = toNick;
    }

    // Set Role
    public void setRoles(string nick, string role)
    {
        PV.RPC("setRolesRPC", RpcTarget.All, nick, role);
    }

    [PunRPC]
    public void setRolesRPC(string nick, string role)
    {
        for(int i = 0; i < playersInfo.Length; i++)
        {
            if(playersInfo[i].nickName == nick)
            {
                playersInfo[i].SetRole(role);
            }
        }
        SetPlayerList(role, 1);  //Player List Dictionary update
    }

    public void SetPlayerList(string role, int num)
    {
        PlayerList[role] += num;

        if((PlayerList["professor"] == 0) && (PlayerList["graduate"] > 0))
        {
            //교수가 죽으면 대학원생이 과제를 받고, 학생에게 부여하는 과정 자체가 안 됨. 이 때 교수 대행 권한을 줌.
            PV.RPC("GetProfessorAuthority", RpcTarget.All);
        }
    }

    [PunRPC]
    void GetProfessorAuthority()
    {
        GameObject[] players = getPlayers();
        foreach(GameObject p in players)
        {
            if(p.GetComponent<PhotonView>().IsMine)
            {
                if((p.GetComponent<CharacterInfoSetting>().role == "graduate") && p.GetComponent<CharacterInfoSetting>().isAlive)
                {
                    getProfAuthority = true;
                }
            }
        }
    }

    //player die
    public void StuNGraPlayerDie()
    {
        playerDieManager.StuNGraPlayerDie();
        MissionManager.mission.SetSolvedRate();

        // if (myPlayer.GetComponent<PhotonView>().IsMine)
        // {
        //     MissionProgressChecker missionProgressChecker = GameManagerScript.instance.readyObj.transform.Find("MainMobileUI/MissionProgressBar").GetComponent<MissionProgressChecker>();
        //     CharacterInfoSetting characterInfoSetting = myPlayer.GetComponent<CharacterInfoSetting>();
        //     missionProgressChecker.BarInfoChangeRunner(1, characterInfoSetting.assignedMission.Count - characterInfoSetting.missionRemained);
        //     missionProgressChecker.BarInfoChangeRunner(0, characterInfoSetting.assignedMission.Count);
        // }
    }

    public Sprite getStuWinSprite() {
        int n = Random.Range(0, studentWinSprite.Length);
        return studentWinSprite[n];
    }

    public Sprite getProfWinSprite() {
        return professorWinSprite;
    }

    public Sprite getGradWinSprite() {
        return graduateWinSprite;
    }

    // GMS variables reset
    public void ResetValues()
    {
        isGameStarted = false;
        updatePause = true;
    }

    // Let all the ready objects start manually
    public void StartAll()
    {
        uiManager = readyObj.transform.GetChild(9).gameObject.GetComponent<UIManager>();
        // OnlyReadyScene Script
        Transform onlyReadyScene = readyObj.transform.Find("OnlyReadyScene");
        uiManager = readyObj.transform.GetChild(9).gameObject.GetComponent<UIManager>();

        //missionManager = readyObj.transform.Find("OnlyReadyScene/CSVManager").GetComponent<MissionManager>();
        readyManager = readyObj.transform.Find("OnlyReadyScene/ReadyManager").gameObject;
        settingPlayerManager = readyManager.GetComponent<SettingPlayerManager>();
        settingPlayerManager.MyAwake();
        playerDieManager = readyObj.transform.Find("Die Manager").gameObject.GetComponent<PlayerDie>();
        playerDieManager.MyAwake();
        playerUISettingScript = GameManagerScript.instance.readyObj.transform.Find("PlayerUI").GetComponent<PlayerUISettingScript>();

        // onlyReadyScene.Find("ReadyManager").GetComponent<CountdownUI>().MyAwake();
        uiManager.MyAwake();

        // WindowUI Script
        uiManager.GetComponent<SettingWindow>().MyAwake();
    }
}
