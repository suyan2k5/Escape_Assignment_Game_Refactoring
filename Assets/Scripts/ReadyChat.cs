using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ReadyChat : MonoBehaviour
{
    GameObject[] players;
    GameObject myPlayer;
    PhotonView PV, playerPV;

    // chat flow control variables
    public bool chatStart = false;
    public bool freetalkStart = false; // after obo, 60seconds free talking time is given.

    // objects
    // [SerializeField] private CountdownUI countdownUI;
    [SerializeField] private GameObject meetingWindow, chatPanel;
    [SerializeField] private GameObject msgOther, msgOwn;
    [SerializeField] private GameObject extendCutDownText;
    [SerializeField] private InputField inputField;
    [SerializeField] private Sprite[] chatImages;
    [SerializeField] private GameObject redPoint;
    Transform msgListPanel;
    
    float height;
    GameObject prevMsg;
    VoteManager voteManager;
    List<GameObject> msgList = new List<GameObject>();
    List<GameObject> timeRecordList = new List<GameObject>();
    private bool initialsetting = true;

    // Position Test
    [SerializeField] private GameObject testPoint;
    int idx = 0;
    bool test = true;
    bool once1 = false;
    bool once2 = false;
    bool once3 = false;
    int playerIndex;
    GameObject sendPlayer;
    bool isStart = false;

    private void Awake()
    {
        PV = gameObject.GetComponent<PhotonView>();
        msgListPanel = chatPanel.transform.GetChild(0).GetChild(0);

        meetingWindow.SetActive(false);

        height = msgOwn.transform.GetChild(1).GetChild(1).GetComponent<Text>().preferredHeight;

        voteManager = GetComponent<VoteManager>();
    }

    private void Update()
    {
        if(test)
        {
            if (chatStart)
            {
                if(once1 == false) 
                {
                    if(PV.IsMine) PV.RPC("sendAnnounceMessage", RpcTarget.AllBuffered, "시작 전 채팅을 시작합니다.");
                    once1 = true;
                }
            }
        }
    }

    public void startMeeting()
    {
        // if(!countdownUI.isCounting)
        // {
            PV.RPC("initialSetting", RpcTarget.AllBuffered);
            players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<PhotonView>().IsMine)
                {
                    myPlayer = players[i];
                    playerIndex = i;
                }
            }
            meetingWindow.GetComponent<WindowAnimation>().OnAnimUI();
            redPoint.SetActive(false);
        // }
        
    }
    [PunRPC]
    void initialSetting()
    {
        if(initialsetting)
        {
            print("initial");
            players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++) if (players[i].GetComponent<PhotonView>().IsMine) myPlayer = players[i];
            
            // GameObject Serialize
            string playerList = "";
            for(int i = 0; i < players.Length; i++)
            {
                playerList += i == 0 ? players[i].GetComponent<CharacterInfoSetting>().name : "," + players[i].GetComponent<CharacterInfoSetting>().name;
            }
            initialsetting = false;
            meetingWindow.SetActive(true);
            meetingWindow.SetActive(false);
            chatStart = true;
        }
        else return;
    }

    public void sendMsg()
    {
        int imageNum = myPlayer.GetComponent<CharacterInfoSetting>().animalInd;
        if(!(myPlayer.GetComponent<CharacterInfoSetting>().isAlive) && inputField.text != "")
        {
            PV.RPC("sendMsgRPC", RpcTarget.All, myPlayer.GetComponent<PlayerMulti>().NickNameText.text, inputField.text, imageNum, false);
            inputField.text = "";
            return;
        }

        if(inputField.text != "")
        {
            PV.RPC("sendMsgRPC", RpcTarget.All, myPlayer.GetComponent<PlayerMulti>().NickNameText.text, inputField.text, imageNum, true);
            inputField.text = "";
        }
    }

    [PunRPC]
    void sendMsgRPC(string _NickName, string _Msg, int imageNum, bool senderAlive)
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < characters.Length; i++) if (characters[i].GetComponent<PhotonView>().IsMine) myPlayer = characters[i];
        
        if(senderAlive || !(myPlayer.GetComponent<CharacterInfoSetting>().isAlive))
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
                if(!meetingWindow.activeSelf) redPoint.SetActive(true);
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
            pos = msgListPanel.transform.position;
            MsgListDrag.firstMsg = _msg;
            MsgListDrag.lastMsg = _msg;
        }
        return pos;
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

    public void nicknameChanged(string fromNick, string toNick)
    {
        PV.RPC("nicknameChangedRPC", RpcTarget.AllBuffered, fromNick, toNick);
    }

    [PunRPC]
    void nicknameChangedRPC(string fromNick, string toNick)
    {
        print("find msgs...");
        MessageControl[] playerMsgTextList = msgListPanel.GetComponentsInChildren<MessageControl>();
        print(playerMsgTextList.Length);
        for(int i = 0; i < playerMsgTextList.Length; i++)
        {
            if(!(playerMsgTextList[i].CompareTag("PlayerMsg"))) continue;

            Text msgNick = playerMsgTextList[i].transform.GetChild(0).GetChild(1).GetComponent<Text>();
            if(msgNick.text == fromNick)
            {
                msgNick.text = toNick;
            }
        }
    }

    public void exitChat()
    {
        meetingWindow.GetComponent<WindowAnimation>().OffAnimUI();
    }
    
    public void offMeetingWindow()
    {
        meetingWindow.GetComponent<WindowAnimation>().OffAnimUI();
        removeAllChatData();
    }
    private void removeAllChatData()
    {
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
        idx = 0;
    }
}

