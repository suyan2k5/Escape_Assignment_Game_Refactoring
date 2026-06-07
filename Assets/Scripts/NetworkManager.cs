using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Create Room Window
    public InputField UserT, RoomT;
    public Slider missionNumS;
    int missionNum;

    // Room Info Panel Object in Room List
    public GameObject roomPrefab;
    private List<GameObject> roomPrefabs = new List<GameObject>();
    public Transform roomListContent;

    // Windows to Create/Join Room
    public GameObject gameSettingWindow;
    public GameObject createRoomWindow;
    // Validity Check Display Text
    public Text warningText;

    // ??
    int roomPlayerNum;

    // Random Nickname Lists
    string[] preNickList = { "모범생", "취준생", "과탑", "새내기", "복학생", "과대", "외톨이"};
    string[] postNickList = { "곰돌이", "토끼", "강아지", "너구리", "고양이", "여우", "염소", "쥐" };

    // room name validity check
    private List<string> roomNameList = new List<string>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Game Start Button in Main Menu Click Event
    // Server Connect & Show Room List
    public void GameStart()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectUsingSettings();
        gameSettingWindow.SetActive(true);
    }

    // Callback function of connection to master server
    public override void OnConnectedToMaster()
    {
        print("Server Entered");
        PhotonNetwork.JoinLobby();
    }

    // Set player's nickname randomly
    public void setNickname()
    {
        string nickname = preNickList[(int) UnityEngine.Random.Range(0, 7)] + postNickList[(int) UnityEngine.Random.Range(0, 8)];

        PhotonNetwork.LocalPlayer.NickName = nickname;
    }
    
    // Callback function of joining lobby
    public override void OnJoinedLobby()
    {
        setNickname();
    }

    // Turn off the room list window and disconnect to master server
    public void backToMain()
    {
        gameSettingWindow.SetActive(false);
        PhotonNetwork.Disconnect();
    }

    // Show create room window
    public void createNewRoom()
    {
        createRoomWindow.GetComponent<WindowAnimation>().OnAnimUI();
        //createRoomWindow.SetActive(true);
    }

    // Function which is on "CreateButton" Object
    // Check whether Mission Num Text is valid.
    // If valid, execute createAndEnterRoom and check a validity of room name text.
    public void checkBeforeEnterRoom()
    {
        // int num;
        // if(string.IsNullOrEmpty(missionNumS.value))
        // {
        //     warningText.text = "미션 개수는 4~6 사이의 정수를 입력해주세요.";
        //     warningText.color = Color.red;
        // }
        // else if(int.TryParse(missionNumS.value, out num))
        // {
        //     if(num > 3 && num < 7)
        //     {
        //         missionNum = num;
        //         createAndEnterRoom();
        //     }
        //     else
        //     {
        //         warningText.text = "미션 개수는 4~6 사이의 정수를 입력해주세요.";
        //         warningText.color = Color.red;
        //     }
        // }
        // else
        // {
        //     warningText.text = "미션 개수는 4~6 사이의 정수를 입력해주세요.";
        //     warningText.color = Color.red;
        // }
        missionNum = (int)missionNumS.value;
        createAndEnterRoom();
    }

    // Check whether Room Name Text is valid.
    // If valid, Create Room
    public void createAndEnterRoom()
    {
        string roomname = RoomT.text;
        if(!(1 <= roomname.Length && roomname.Length <= 13))
        {
            warningText.text = "1~13자 이내의 방 이름을 입력해주세요";
            warningText.color = Color.red;
            return;
        }
        for(int i = 0; i < roomNameList.Count; i++)
        {
            if(roomname == roomNameList[i])
            {
                warningText.text = "같은 방 이름이 이미 존재합니다";
                warningText.color = Color.red;
                return;
            }
        }

        GameManagerScript.instance.missionNumber = (int)missionNumS.value;
        
        PhotonNetwork.JoinOrCreateRoom(roomname, new RoomOptions { MaxPlayers = 8 }, null);
    }

    // Exit create room window with initializeing components
    public void exitCreateRoomSetting()
    {
        createRoomWindow.GetComponent<WindowAnimation>().OffAnimUI();
        //createRoomWindow.SetActive(false);
        warningText.text = "미션 개수를 입력해주세요." + "\n" + "미션 개수 설정은 미션 세팅에서 다시 바꿀 수 있습니다.";
        warningText.color = Color.black;
    }

    // Callback function of updating room list
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Room Name List Setting for Create Room Rejection
        roomNameList.Clear();
        for(int i = 0; i < roomList.Count; i++) {
            roomNameList.Add(roomList[i].Name);
        }

        // Destroy the created room panels
        if(roomPrefabs.Count > 0)
        {
            for(int i = 0; i < roomPrefabs.Count; i ++)
            {
                Destroy(roomPrefabs[i]);
            }
            roomPrefabs.Clear();
        }

        // Instantiate the rooms panels
        for(int i = 0; i < roomList.Count; i++)
        {
            // print(roomList.Count);
            GameObject room = Instantiate(roomPrefab, roomListContent);
            Transform playerNumT = room.transform.GetChild(1);
            Transform enterBtn = room.transform.GetChild(2);

            Color color;
            room.transform.GetChild(0).GetComponent<Text>().text = roomList[i].Name;
            playerNumT.GetComponent<Text>().text = roomList[i].PlayerCount + "/" + roomList[i].MaxPlayers;
            enterBtn.GetComponent<Button>().onClick.AddListener(() => { PhotonNetwork.JoinRoom(room.transform.GetChild(0).GetComponent<Text>().text); });

            if(i % 6 == 0)
            {
                ColorUtility.TryParseHtmlString("#F3BECB", out color);
                room.GetComponent<Image>().color = color;
            }
            else if(i % 6 == 1)
            {
                ColorUtility.TryParseHtmlString("#F3E8BE", out color);
                room.GetComponent<Image>().color = color;
            }
            else if(i % 6 == 2)
            {
                ColorUtility.TryParseHtmlString("#DFF3BE", out color);
                room.GetComponent<Image>().color = color;
            }
            else if(i % 6 == 3)
            {
                ColorUtility.TryParseHtmlString("#CCEFF4", out color);
                room.GetComponent<Image>().color = color;
            }
            else if(i % 6 == 4)
            {
                ColorUtility.TryParseHtmlString("#BBCCEE", out color);
                room.GetComponent<Image>().color = color;
            }
            else if(i % 6 == 5)
            {
                ColorUtility.TryParseHtmlString("#D1BEF3", out color);
                room.GetComponent<Image>().color = color;
            }

            if(!(roomList[i].IsOpen))
            {
                room.GetComponent<Image>().color = Color.gray;
                playerNumT.GetComponent<Text>().text += " 게임중";
                enterBtn.GetComponent<Image>().color = Color.gray;
            }
            room.SetActive(true);
            roomPlayerNum = roomList[i].PlayerCount;
            roomPrefabs.Add(room);
        }
    }

    // Callback function of joining room
    public override void OnJoinedRoom()
    {
        // Nickname Duplication Check
        // If there is a duplication, change a nickname randomly and check again
        while(true)
        {
            bool noDup = true;
            foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                print(player.NickName + " isLocal " + player.IsLocal);
                if(player.NickName == PhotonNetwork.LocalPlayer.NickName)
                {
                    if(player.IsLocal) continue;
                    setNickname();
                    noDup = false;
                    break;
                }
            }
            if(noDup) break;
        }
        GameManagerScript.instance.manageStart();

        print("Joined Room");
        StartCoroutine("LoadScene");
    }

    // Load Ready Scene
    public IEnumerator LoadScene()
    {
        string roomName = "Ready_SY";
        PhotonNetwork.LoadLevel(roomName);

        // Check whether the loading is finished
        // If there's an error which level loading progress is always less than 1, exit loop in a arbitory time.
        float escTime = 3f;
        while(PhotonNetwork.LevelLoadingProgress < 1)
        {
            escTime -= Time.deltaTime;
            if(escTime < 0) break;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        print("Ready Scene Loading is ended ( or exited manually )");

        // Spawn or Find Ready object
        if(PhotonNetwork.IsMasterClient) GameManagerScript.instance.readyObj = PhotonNetwork.Instantiate(GameManagerScript.instance.readyObjPref.name, Vector3.zero, Quaternion.identity);
        else {
            GameObject obj = GameObject.Find(GameManagerScript.instance.readyObjPref.name + "(Clone)");
            // while(!obj) {
            while(!(GameManagerScript.instance.readyObj)) {
                obj = GameObject.Find(GameManagerScript.instance.readyObjPref.name + "(Clone)");
                yield return new WaitForSeconds(0.1f);
            }
            GameManagerScript.instance.readyObj = obj;
        }
        
        // Instantiate Player Object
        GameObject player = PhotonNetwork.Instantiate("TestCharacter", Vector2.zero, Quaternion.identity);
        player.GetComponent<CharacterInfoSetting>().currentSceneName = roomName;
        player.GetComponent<CharacterInfoSetting>().setNickName(PhotonNetwork.LocalPlayer.NickName);


        // Set GMS
        GameManagerScript.instance.StartAll();              // Execute Awake or Start function in ready objects
        GameManagerScript.instance.updatePause = false;
        GameManagerScript.instance.setGameStart();

        // Let every client find player
        GameManagerScript.instance.FindPlayersRPC('a');

        // Only master client( who created a room ) get the information of missionnum;
        if(PhotonNetwork.IsMasterClient)
        {
            Transform readyManager = GameManagerScript.instance.readyObj.transform.Find("OnlyReadyScene/ReadyManager");
            GameManagerScript.instance.missionNumber = missionNum;
        }

        // Find Camera and Execute Camera Setting
        while(true)
        {
            GameObject cam = GameObject.Find("Main Move Camera");
            if(cam != null)
            {
                print("CameraSet in " + roomName);
                cam.GetComponent<CameraPosition>().cameraSetting(roomName);
                break;
            }
            else
            {
                print("No Camera");
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void BeforeLeftRoom()
    {
        // Find players without this client
        GameManagerScript.instance.FindPlayersRPC('o');
        print("OnLeftRoom excuted.");
    }
}
