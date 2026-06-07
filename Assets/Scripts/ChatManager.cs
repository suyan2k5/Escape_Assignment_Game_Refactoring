using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ChatManager : MonoBehaviour
{
    GameObject[] players; CharacterInfoSetting[] playersInfo;
    GameObject myPlayer; CharacterInfoSetting myPlayerInfo;
    PhotonView PV, playerPV;

    [Header("Chat Setting")]
    public float chatTime = 3f;

    // chat flow control variables
    private bool chatStart = false;
    private bool obotimeStart = false; // one by one speaking time
    private bool freetalkStart = false; // after obo, 60seconds free talking time is given.

    [Header("Chat Objects")]
    [SerializeField] private GameObject meetingWindow, chatPanel;
    [SerializeField] private GameObject msgOther, msgOwn;
    [SerializeField] private GameObject extendCutDownText;
    [SerializeField] private InputField inputField;
    [SerializeField] private Sprite[] chatImages;
    [SerializeField] private Text timeT;
    [SerializeField] private GameObject testPoint;
    Transform msgListPanel;
    
    float height;
    GameObject prevMsg;
    Dictionary<string, int> meetingStack = new Dictionary<string, int>();
    Dictionary<string, int> playerStack = new Dictionary<string, int>();
    VoteManager voteManager;
    bool startVoting = false;
    List<GameObject> msgList = new List<GameObject>();
    List<GameObject> timeRecordList = new List<GameObject>();

    private void Awake()
    {
        PV = gameObject.GetComponent<PhotonView>();
        msgListPanel = chatPanel.transform.GetChild(0).GetChild(0);

        meetingWindow.SetActive(false);

        timeT.text = chatTime.ToString("0");

        height = msgOwn.transform.GetChild(1).GetChild(1).GetComponent<Text>().preferredHeight;

        voteManager = GetComponent<VoteManager>();

        for(int i = 0; i < 10; i++) playerChat[i] = false;
    }

    int idx = 0;
    bool once1 = false;
    bool once2 = false;
    bool once3 = false;
    bool[] playerChat = new bool[10];
    int myidx = -1;

    // Chat Manage
    private void Update()
    {
        if(chatStart)
        {
            // Execute once to print announcement.
            if(!once1) 
            {
                if(PV.IsMine) PV.RPC("sendAnnounceMessage", RpcTarget.All, "회의를 시작합니다.");
                obotimeStart = true;
                chatTime = 10f;
                once1 = true;
            }

            // each of alive players is given 10 seconds to say any words.
            if (obotimeStart)
            {
                // Find a player to get voice using idx value
                // and set myidx in these progress
                // Player Index Set
                if(playersInfo[idx].nickName == myPlayerInfo.nickName) myidx = idx;
                // skip dead player among player list
                while(!(playersInfo[idx].isAlive))
                {
                    if(playersInfo[idx].nickName == myPlayerInfo.nickName) myidx = idx;
                    idx++;
                    if(idx >= players.Length)
                    {
                        obotimeStart = false;
                        freetalkStart = true;

                        for(int i = 0; i < 10; i++) playerChat[i] = true;

                        return;
                    }
                }

                // Player Alive => Get Voice
                if (chatTime > 0)
                {
                    // Time decrease
                    chatTime -= Time.deltaTime;
                    timeT.text = chatTime.ToString("0");

                    // Execute once to send announcement
                    if(once2 == false)
                    {
                        if(PV.IsMine) PV.RPC("sendAnnounceMessage", RpcTarget.All, playersInfo[idx].nickName + "님의 발언 차례입니다.");
                        once2 = true;
                    }

                    // idx-th player get the voice
                    playerChat[idx] = true;
                }
                else
                {
                    // If the time's over, pass the voice to next player
                    once2 = false;
                    chatTime = 10f;
                    timeT.text = chatTime.ToString("0");

                    playerChat[idx] = false;
                    idx++;
                }

                // if all alive players had a voice, start free talk
                if(idx >= players.Length)
                {
                    obotimeStart = false;
                    freetalkStart = true;

                    for(int i = 0; i < 10; i++) playerChat[i] = true;
                }
            }
            else if (freetalkStart)
            {
                // Execute once to send announcement
                if(once3 == false)
                {
                    if(PV.IsMine) PV.RPC("sendAnnounceMessage", RpcTarget.All, "자유 발언 시간입니다.");
                    once3 = true;
                    chatTime = 10f;
                }

                // Time decrease
                if (chatTime > 0f)
                {
                    chatTime -= Time.deltaTime;
                    timeT.text = chatTime.ToString("0");
                }
                else
                {
                    // If time's over, end chat and start vote
                    timeT.text = "";
                    if (!startVoting)
                    {
                        endChat();
                        startVoting = true;
                    }
                }
            }
        }
    }

    public void startMeeting()
    {
        initialSetting();

        if(myPlayerInfo.missionRemained < 1)
        {
            print("1");
            if(meetingStack[myPlayerInfo.nickName] < 2)
            {
                print("2");
                meetingStack[myPlayerInfo.nickName]++;
                PV.RPC("startMeetingRPC", RpcTarget.All);
            }
        }
        else
        {
            print("3");
            if(meetingStack[myPlayerInfo.nickName] == 0)
            {
                print("4");
                meetingStack[myPlayerInfo.nickName]++;
                PV.RPC("startMeetingRPC", RpcTarget.All);
            }
        }
    }

    // [PunRPC]
    void initialSetting()
    {
        print("Initial Setting");

        players = GameManagerScript.instance.getPlayers();
        playersInfo = GameManagerScript.instance.getPlayersInfo();
        myPlayer = GameManagerScript.instance.getMyPlayer();
        myPlayerInfo = GameManagerScript.instance.getMyPlayerInfo();
        
        // Player list order spread
        string playerList = playersInfo[0].name;
        for(int i = 1; i < playersInfo.Length; i++)
        {
            playerList += "," + playersInfo[i].name;
        }
        gameObject.GetComponent<PhotonView>().RPC("playerListSetting", RpcTarget.All, playerList);

        // initialsetting = false;

        GameManagerScript.instance.readyObj.transform.Find("PlayerUI").GetComponent<PlayerUISettingScript>().offMeetingButton();
    }

    [PunRPC]
    void playerListSetting(string _data)
    {
        players = GameManagerScript.instance.getPlayers();
        playersInfo = GameManagerScript.instance.getPlayersInfo();
        myPlayer = GameManagerScript.instance.getMyPlayer();
        myPlayerInfo = GameManagerScript.instance.getMyPlayerInfo();

        string[] data = _data.Split(',');
        for(int i = 0; i < data.Length; i++)
        {
            for(int j = i; j < data.Length; j++)
            {
                if(i == j && playersInfo[i].name == data[i]) break;
                else if(playersInfo[i].name == data[i])
                {
                    GameObject temp = players[i];
                    players[i] = players[j];
                    players[j] = temp;
                    CharacterInfoSetting temp2 = playersInfo[i];
                    playersInfo[i] = playersInfo[j];
                    playersInfo[j] = temp2;

                }
            }
        }

        for(int i = 0; i < playerChat.Length; i++)
        {
            playerChat[i] = true;
        }

        for(int i = 0; i < playersInfo.Length; i++)
        {
            try
            {
                meetingStack.Add(playersInfo[i].nickName, 0);
            }
            catch(System.ArgumentException ae)
            {
                print("ChatManager.cs/playerListSetting: Duplicate Key for meeting stack.");
            }
        }
    }

    [PunRPC]
    void startMeetingRPC()
    {
        removeAllChatData();
        // players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++) if (players[i].GetComponent<PhotonView>().IsMine) myPlayer = players[i];
        playerPV = myPlayer.GetComponent<PhotonView>();
        if(!(playerPV.IsMine)) return;  // 각자 맡고있는 플레이어만 작동되도록 함.

        for(int i = 0; i < players.Length; i++)
        {
            playerStack.Add(playersInfo[i].nickName, 0);
        }
        meetingWindow.GetComponent<WindowAnimation>().AnimPanelSizeChange("ChatPanel");
        meetingWindow.GetComponent<WindowAnimation>().OnAnimUI();
        chatStart = true;
    }

    public void sendMsg()
    {
        if(myidx < 0) return;
        int imageNum = myPlayerInfo.animalInd;
        if(!(myPlayerInfo.isAlive) && inputField.text != "")
        {
            PV.RPC("sendMsgRPC", RpcTarget.All, myPlayer.GetComponent<PlayerMulti>().NickNameText.text, inputField.text, imageNum, false);
            inputField.text = "";

            return;
        }

        if(playerChat[myidx] && inputField.text != "")
        {
            PV.RPC("sendMsgRPC", RpcTarget.All, myPlayer.GetComponent<PlayerMulti>().NickNameText.text, inputField.text, imageNum, true);
            inputField.text = "";
        }
    }

    [PunRPC]
    void sendMsgRPC(string _NickName, string _Msg, int imageNum, bool senderAlive)
    {
        // sender == alive || sender == myPlayer == died
        if(senderAlive || !(myPlayerInfo.isAlive))
        {
            //Setting
            GameObject msg = new GameObject();
            //Vector2 msgPos;
            if (myPlayer.GetComponent<PlayerMulti>().NickNameText.text == _NickName)
            {
                msgOwn.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = _NickName;
                msgOwn.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = _Msg;
                msgOwn.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = chatImages[imageNum];
                if(!senderAlive) msgOwn.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.red;
                else msgOwn.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.black;

                msg = Instantiate(msgOwn, Vector2.zero, Quaternion.identity, msgListPanel);
            }
            else
            {
                msgOther.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = _NickName;
                msgOther.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = _Msg;
                msgOther.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = chatImages[imageNum];
                if(!senderAlive) msgOther.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.red;
                else msgOther.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.black;

                msg = Instantiate(msgOther, Vector2.zero, Quaternion.identity, msgListPanel);
            }

            msg.GetComponent<MessageControl>().boxResize();

            //Position
            msg.transform.localPosition = msgPosition(msg);
            msgList.Add(msg);

            prevMsg = msg;

            chatPanel.transform.GetChild(1).GetComponent<MsgListDrag>().msgListPosition(0);
        }
    }

    Vector2 msgPosition(GameObject _msg)
    {
        Vector2 pos = _msg.transform.position;

        if(prevMsg != null)
        {
            float prevMsgy = prevMsg.transform.localPosition.y;
            float distancey = prevMsg.GetComponent<MessageControl>().getHeight();

            pos = new Vector2(0, (prevMsgy - distancey));

            MsgListDrag.lastMsg = _msg;
        }
        else
        {
            MsgListDrag.firstMsg = _msg;
            MsgListDrag.lastMsg = _msg;
        }


        //Instantiate(testPoint, Vector2.zero, Quaternion.identity, chatPanel.transform.GetChild(0).GetChild(0)).transform.localPosition = pos;
        return pos;
    }
    
    public void extendTime()
    {
        if(myPlayerInfo.isAlive == false) return;
        if(playerStack[myPlayerInfo.nickName] == 0 && !obotimeStart)
        {
            PV.RPC("sendExtendMessage", RpcTarget.All, myPlayerInfo.nickName);
        }
        else return;
    }

    [PunRPC]
    void sendExtendMessage(string _NickName)
    {
        chatTime += 5f;
        playerStack[_NickName] = 1;
        GameObject msg;
        extendCutDownText.GetComponent<Text>().text = _NickName + "님이 시간을 연장하셨습니다.";
        msg = Instantiate(extendCutDownText, Vector2.zero, Quaternion.identity, msgListPanel);
        msg.transform.localPosition = msgPosition(msg);
        prevMsg = msg;
        timeRecordList.Add(msg);

        chatPanel.transform.GetChild(1).GetComponent<MsgListDrag>().msgListPosition(0);
    }

    public void cutDownTime()
    {
        if(myPlayerInfo.isAlive == false) return;
        if(playerStack[myPlayerInfo.nickName] == 0 && !obotimeStart)
        {
            PV.RPC("sendCutDownMessage", RpcTarget.All, myPlayerInfo.nickName);   
        }
        else return;
    }

    [PunRPC]
    void sendCutDownMessage(string _NickName)
    {
        chatTime -= 5f;
        playerStack[_NickName] = 1;
        GameObject msg;
        extendCutDownText.GetComponent<Text>().text = _NickName + "님이 시간을 단축시키셨습니다.";
        msg = Instantiate(extendCutDownText, Vector2.zero, Quaternion.identity, msgListPanel);
        msg.transform.localPosition = msgPosition(msg);
        prevMsg = msg;
        timeRecordList.Add(msg);

        chatPanel.transform.GetChild(1).GetComponent<MsgListDrag>().msgListPosition(0);
    }

    [PunRPC]
    void sendAnnounceMessage(string announce)
    {
        GameObject msg;
        extendCutDownText.GetComponent<Text>().text = announce;
        msg = Instantiate(extendCutDownText, Vector2.zero, Quaternion.identity, msgListPanel);
        msg.transform.localPosition = msgPosition(msg);
        prevMsg = msg;
        timeRecordList.Add(msg);

        chatPanel.transform.GetChild(1).GetComponent<MsgListDrag>().msgListPosition(0);
    }

    public void endChat()
    {
        meetingWindow.GetComponent<WindowAnimation>().AnimPanelSizeChange("VotingPanel");
        chatStart = false;
        voteManager.voteSetting();
    }
    
    public void offMeetingWindow()
    {
        meetingWindow.GetComponent<WindowAnimation>().OffAnimUI();
        removeAllChatData();

        // Deads clear
        GameObject[] deads = GameObject.FindGameObjectsWithTag("Dead");
        for(int i = 0; i < deads.Length; i++)
        {
            GameObject.Destroy(deads[i]);
        }


        // Scene Change
        gameObject.GetComponent<SceneChanger>().changeScene();
    }

    private void removeAllChatData()
    {
        chatTime = 10.0f;
        playerStack.Clear();
        startVoting = false;
        prevMsg = null;
        inputField.text = "";
        for(int i = 0; i < msgList.Count; i++)
        {
            Destroy(msgList[i]);
        }
        msgList.Clear();
        for(int i = 0; i < timeRecordList.Count; i++)
        {
            Destroy(timeRecordList[i]);
        }
        timeRecordList.Clear();
        
        once1 = false;
        once2 = false;
        once3 = false;
        for(idx = 0; idx < 10; idx++) playerChat[idx] = false;
        idx = 0;
    }

}
