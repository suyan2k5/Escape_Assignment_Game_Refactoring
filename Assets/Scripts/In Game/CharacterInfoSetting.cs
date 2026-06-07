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

public class CharacterInfoSetting : MonoBehaviourPunCallbacks, IPunObservable
{
    //占시뤄옙占싱어에占쏙옙 占십울옙占쏙옙 占쏙옙占쏙옙 占쏙옙 占쏙옙占?占쏙옙占쏙옙
    MissionManager missionManager;
    PlayerUISettingScript playerUI;
    // CountdownUI countdownUI;
    JoyStick joyStick;
    public string nickName;
    public int myRoomNum;
    public bool roomInfoPrintDone;
    public List<Mission> assignedMission;
    public int missionRemained; public bool isMissionAssigned;
    public string currentSceneName;
    public string role;
    public int missionSolvingNum = -1;
    public bool isAlive = true;
    private bool isSpawn = true;
    public string myAnimalCharacter;
    public Image[] profileImages;
    public Sprite[] avatarImages;
    public Text[] profileNicknames;
    public Sprite[] animalImages;
    public RuntimeAnimatorController[] controllers;
    Vector2 playerPos;
    Vector2 computerPos;
    Vector2 customPos;
    private float distance;
    [SerializeField] private float range = 1.8f;
    public int myAnimalProfileIndex;
    public int animalInd;
    private GameObject avatarPlayer; 
    private BoxCollider2D col2D;
    public int assignedMissionNum;
    int animalIndex;
    [SerializeField] Sprite[] changeWindows;
    private SettingPlayerManager settingPlayerManager;
    public int assignmentStack = 0;

    void Start()
    {
        if (!isAlive && gameObject.CompareTag("Player")) isAlive = true;

        if(!(gameObject.GetPhotonView().IsMine)) return;
        // Awake Code
        if (gameObject.CompareTag("Player"))
        {
            roomInfoPrintDone = false;
            Transform readyObj = GameManagerScript.instance.readyObj.transform;
            playerUI = readyObj.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
            missionManager = readyObj.Find("OnlyReadyScene/CSVManager").GetComponent<MissionManager>();
            // countdownUI = readyObj.Find("OnlyReadyScene/ReadyManager").GetComponent<CountdownUI>();
            joyStick = readyObj.Find("MainMobileUI/JoyStickObject/JoyStick").GetComponent<JoyStick>();
            settingPlayerManager = readyObj.Find("OnlyReadyScene/ReadyManager").GetComponent<SettingPlayerManager>();

            missionRemained = 0; isMissionAssigned = false;
        }

        // player vs dead
        // player
        if (gameObject.CompareTag("Player"))
        {
            col2D = GetComponent<BoxCollider2D>();
            // 
            if (!playerUI.isClassroomDetermined && GameManagerScript.instance.isGameStarted)
            {
                // countdownUI.countdownSet = true;
                if (gameObject.GetComponent<PhotonView>().IsMine)
                {
                    settingPlayerManager.AnimalCharacterSetting(nickName);
                    // animalIndex = settingPlayerManager.animalIndex;

                    missionManager.character = this;
                    // countdownUI.characterInfo = this;
                    joyStick.character = gameObject;
                    joyStick.playerMulti = gameObject.GetComponent<PlayerMulti>();
                    setNickName(GetComponent<PlayerMulti>().NickNameText.text);
                }
            }
        }

        // dead
        // do nothing
    }

    public void restart()
    {
        currentSceneName = "Ready";
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        Start();
        print("Restart Done");
    }

    private void Update()
    {
        if(GameManagerScript.instance.updatePause) return;
        if(!(gameObject.GetPhotonView().IsMine)) return;
        // player vs dead
        // player
        if (gameObject.CompareTag("Player"))
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.down, 0.02f, LayerMask.GetMask("Ground"));

            if (gameObject.GetComponent<PhotonView>().IsMine)
            {
                //gameObject.GetComponent<PhotonView>().RPC("SpreadSceneName", RpcTarget.All, currentSceneName);
                // if (role == "professor" || role == "graduate")
                // {
                //     if (currentSceneName == "Spawn")
                //     {
                //         if(role != "graduate")
                //         {
                //             playerUI.onAssignmentButton();
                        
                //         }
                //         gameObject.GetComponent<AssignmentManager>().curtime = 5.0f;
                //         playerUI.ButtonsForInGame();
                //         Sabotage sabotage = GameManagerScript.instance.uiManager.GetComponent<Sabotage>();
                //         sabotage.profLightCooltime = 15.0f;
                //     }
                // }
                // if (role == "professor" && currentSceneName == "Spawn")
                // {
                //     playerUI.ButtonsForInGame();
                //     playerUI.onSabotageButton(0);
                // }
                //else if (role == "student" && currentSceneName == "Spawn") playerUI.ButtonsForInGame();

                // if (role != "professor" && currentSceneName == "Spawn" && isSpawn)
                // {
                //     playerUI.setStudentRoomUI();
                //     isSpawn = false;
                // }
                if (currentSceneName == "Lecture1" || currentSceneName == "Lecture2" || currentSceneName == "Lecture3" ||
                    currentSceneName == "Lecture4" || currentSceneName == "Lecture5" || currentSceneName == "Lecture6")
                {
                    playerPos = gameObject.transform.position;
                    computerPos = (Vector2)(GameObject.Find(currentSceneName + "Background/Computer").transform.position);
                    distance = new Vector2(computerPos.x - playerPos.x, computerPos.y - playerPos.y).magnitude;
                    if (distance < range && isAlive)
                    {
                        playerUI.onMeetingButton();
                    }
                    else playerUI.offMeetingButton();
                }
                else if(currentSceneName == "Ready_SY")
                {
                    playerPos = gameObject.transform.position;
                    if(GameObject.Find("ReadyBackground")) customPos = (Vector2)(GameObject.Find("ReadyBackground/CustomizePanel").transform.position);
                    distance = new Vector2(customPos.x - playerPos.x, customPos.y - playerPos.y).magnitude;
                    if (distance < range)
                    {
                        playerUI.onCustomizeButton();
                    }
                    else playerUI.offCustomizeButton();
                }
            }
        }
        // dead
        // do not find raycast
    }


    //////////////////////////////////////////////

    //역할에 맞게 UI 세팅
    public void SettingWithMyRole()
    {
        playerUI.ButtonsForInGame();

        if(role == "student")
        {
            playerUI.setStudentRoomUI();
            playerUI.SetStuNGraSabotage();
        }
        else if(role == "graduate")
        {
            gameObject.AddComponent<GraduateManager>();
            gameObject.GetComponent<GraduateManager>().SetImages(changeWindows);
            playerUI.setStudentRoomUI();
            playerUI.OnAssignmentButton();
            playerUI.SetStuNGraSabotage();
        }
        else if(role == "professor")
        {
            playerUI.OnAssignmentButton();
            Sabotage sabotage = GameManagerScript.instance.uiManager.GetComponent<Sabotage>();
            sabotage.profLightCooltime = 15.0f;
            playerUI.OnSabotageButton();
            playerUI.SetProfessorSabotage();
            playerUI.roomSettingUI(-1);
        }
        else Debug.Log("Error : There is no setting with my role.");
    }
    //////////////////////////////////////////////

    public void setNickName(string _NickName)
    {
        GameManagerScript.instance.playerNicknameChanged(nickName, _NickName);
    }

    //占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙占싹곤옙, 占쏙옙占시듸옙 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙트 占쏙옙占쏙옙占쌍댐옙 占쌉쇽옙
 
    // MissionManager, MasterMissionSetting
    [PunRPC]
    public void MissionSetting(int missionNumber)
    {
        if(gameObject.GetComponent<PhotonView>().IsMine)
        {
            assignedMission = missionManager.setMyMission(missionNumber);
            playerUI.missionSettingUI(assignedMission);
            missionRemained = assignedMission.Count;
            isMissionAssigned = true;
            assignedMissionNum = assignedMission.Count;
            // StartCoroutine(missionProgressBarSetting());
        }
    }

    // IEnumerator missionProgressBarSetting()
    // {        
    //     // Mission Progress Bar Update
    //     Transform missionProgressBar = GameManagerScript.instance.readyObj.transform.Find("MainMobileUI");
    //     missionProgressBar = missionProgressBar.Find("MissionProgressBar");

    // [PunRPC] // 원래는 syncMissionbyGMS // Used in CharacterInfoSetting.cs/MissionSetting
    IEnumerator missionProgressBarSetting()
    {        
        // Mission Progress Bar Update
        Transform missionProgressBar = GameManagerScript.instance.readyObj.transform.Find("MainMobileUI");
        missionProgressBar = missionProgressBar.Find("MissionProgressBar");

        Image img = missionProgressBar.GetChild(0).GetComponent<Image>();
        Color col = img.color;
        img.color = new Color(col.r, col.g, col.b, 0);

    //     img = missionProgressBar.GetChild(1).GetChild(0).GetComponent<Image>();
    //     col = img.color;
    //     img.color = new Color(col.r, col.g, col.b, 0);

    //     img = missionProgressBar.GetChild(2).GetChild(0).GetComponent<Image>();
    //     col = img.color;
    //     img.color = new Color(col.r, col.g, col.b, 0);

        missionProgressBar.gameObject.SetActive(true);
        yield return null;
        // print("켜졌다! " + missionProgressBar.gameObject.activeSelf);
        // if(role == "student")
        // {
        //     missionProgressBar.GetComponent<MissionProgressChecker>().BarInfoChangeRunner(0, assignedMission.Count);
        // }
    }

    public void gradAdditionalMission()
    {
        assignedMission = missionManager.gradMission(assignedMission);
        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            playerUI.gradMissionSettingUI(assignedMission);
        }
    }

    public List<Mission> getMission()
    {
        return assignedMission;
    }

    public void SetRole(string _role)
    {
        role = _role;
    }

    [PunRPC]
    void SetRoom(int _room)
    {
        myRoomNum = _room;
        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            playerUI.roomSettingUI(myRoomNum);
        }
        // roomInfoPrintDone = true;
    }

    // [PunRPC]
    // void SetProfessorReady()
    // {
    //     roomInfoPrintDone = true;
    //     playerUI.roomSettingUI(-1);
    // }

    // public SpriteRenderer[] customizes;

    // // player die
    // public Sprite[] deadImages;
    // public GameObject dead;
    // [PunRPC]
    // void spreadDeadInfo(String deadNickName)
    // {
    //     if(gameObject.CompareTag("Dead")) return;
        
    //     Color halfOpacity = new Color(255, 255, 255, 0.5f);
    //     gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = halfOpacity;
    //     for (int i = 0; i < customizes.Length; i++)
    //     {
    //         customizes[i].color = halfOpacity;
    //     }

    //     // 죽은 유저 정보 수정 및 사용
    //     if(deadNickName == nickName)
    //     {
    //         isAlive = false;
    //         // scene name
    //         dead.GetComponent<CharacterInfoSetting>().currentSceneName = this.currentSceneName;
    //         // dead img
    //         dead.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = deadImages[animalInd];
    //         // img flip
    //         if (gameObject.transform.Find("Image").GetComponent<SpriteRenderer>().flipX) dead.transform.Find("Image").GetComponent<SpriteRenderer>().flipX = true;
    //         // nickname
    //         Text deadtext = dead.transform.Find("Canvas/NicknameT").GetComponent<Text>();
    //         deadtext.text = deadNickName;
    //         deadtext.color = Color.red;
    //         // object instantiate
    //         dead = PhotonNetwork.Instantiate(dead.name, gameObject.transform.position, Quaternion.identity);
    //     }
    //     profileImages[animalInd].color = Color.gray;
    //     profileNicknames[animalInd].color = Color.red;
    // }
    // [PunRPC]
    // void spreadDeadInfoWithoutInstantiate(String deadNickName)
    // {
    //     if(gameObject.CompareTag("Dead")) return;
        
    //     gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
        
    //     // 죽은 유저 정보 수정 및 사용
    //     if(deadNickName == nickName)
    //     {
    //         isAlive = false;
    //     }
    //     profileImages[myAnimalProfileIndex].color = Color.gray;
    //     profileNicknames[myAnimalProfileIndex].color = Color.red;
    // }
    // [ContextMenu("dodie")]
    // public void dodie()
    // {
    //     gameObject.GetComponent<PhotonView>().RPC("spreadDeadInfo", RpcTarget.AllBuffered, nickName);
    // }

    public void SetMyScene(string scene)
    {
        currentSceneName = scene;
        gameObject.GetComponent<PhotonView>().RPC("SpreadSceneName", RpcTarget.All, currentSceneName);
    }

    //Assigment
    [PunRPC]
    void setAssignmentStack()
    {        
        if(gameObject.GetComponent<PhotonView>().IsMine)
        {
            assignmentStack += 1;
            if(role == "student")
        {
            if(assignmentStack == 3) GameManagerScript.instance.StuNGraPlayerDie();
        }
        else if(role == "graduate")
        {
            if(assignmentStack == 2) GameManagerScript.instance.StuNGraPlayerDie();
        }

        }
        
        print("get assignment " + assignmentStack);
    }

    [PunRPC]
    void GraduateStackSend()
    {
        if(gameObject.GetComponent<PhotonView>().IsMine)
        {
            --assignmentStack;
            playerUI.decAssignmentStack(assignmentStack);
        }
        print("get assignment " + assignmentStack + "from graduate");
    }


    [PunRPC]
    void SpreadSceneName(string sceneName)
    {
        currentSceneName = sceneName;
        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            StartCoroutine(playerUI.setInfoRoom(sceneName));
            
            if(currentSceneName == ("Lecture" + myRoomNum.ToString()))  playerUI.addAssignmentStack(assignmentStack);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(missionRemained);
            stream.SendNext(nickName);
            stream.SendNext(assignedMissionNum);
            stream.SendNext(currentSceneName);
        }
        else
        {
            missionRemained = (int)stream.ReceiveNext();
            nickName = (string)stream.ReceiveNext();
            assignedMissionNum = (int)stream.ReceiveNext();
            currentSceneName = (string)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void setMissionNumber(int num)
    {
        missionSolvingNum = num;
    }

    [PunRPC]
    public void RoleChange(string _role)
    {
        role = _role;
    }
}
