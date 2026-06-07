using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UIManager : MonoBehaviourPunCallbacks
{
    [Header("Windows")]
    [SerializeField] GameObject informationWindow;
    [SerializeField] GameObject settingWindow;
    [SerializeField] GameObject nicknameChangeWindow;
    [SerializeField] GameObject missionNumSetWindow;

    [Header("Buttons")]
    [SerializeField] InputField newNickname;

    [Header("Other Objects")]
    public GameObject readyManager;
    public SettingWindow setting;
    public GameObject readyObj;
    // public CountdownUI countdownUI;
    public GameObject missionProgressBar;
    public ReadyChat readyChat;
    public ChatManager chatManager;
    public CustomizeItem customizeItem;
    public Text warningText;
    public InputField missionNumSetting;
    public Slider missionNumSettingS;

    public void MyAwake() 
    {
        readyObj = GameManagerScript.instance.readyObj;
        // readyManager = readyObj.transform.Find("OnlyReadyScene/ReadyManager").gameObject;
        // print(readyManager.name);
        // countdownUI = readyManager.GetComponent<CountdownUI>();
        setting = gameObject.GetComponent<SettingWindow>();
        readyChat = readyObj.transform.Find("OnlyReadyScene/ReadyChatManager").GetComponent<ReadyChat>();
        chatManager = readyObj.transform.Find("MeetingManager").GetComponent<ChatManager>();
        customizeItem = gameObject.GetComponent<CustomizeItem>();
    }

    public void SettingAfterStart()
    {
        missionProgressBar = readyObj.transform.Find("MainMobileUI/MissionProgressBar").gameObject;
        Transform mpb = missionProgressBar.transform;

        Image img = mpb.GetChild(0).GetComponent<Image>();
        Color col = img.color;
        img.color = new Color(col.r, col.g, col.b, 1f);

        img = mpb.GetChild(1).GetChild(0).GetComponent<Image>();
        col = img.color;
        img.color = new Color(col.r, col.g, col.b, 1f);

        img = mpb.GetChild(2).GetChild(0).GetComponent<Image>();
        col = img.color;
        img.color = new Color(col.r, col.g, col.b, 1f);
        
        setting.leaveRoomBtnOff();
    }

    public void NicknameChangeBtn()
    {
        // Length Rejection
        int newNickLength = newNickname.text.Length;
        if (!(1 <= newNickLength && newNickLength <= 13))
        {
            newNickname.placeholder.GetComponent<Text>().text = "1~13자 이내의 닉네임을 입력해주세요";
            newNickname.text = "";
            return;
        }
        // Duplicate Rejection
        foreach (Player p in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (p.NickName == newNickname.text)
            {
                newNickname.placeholder.GetComponent<Text>().text = "같은 닉네임이 이미 존재합니다";
                newNickname.text = "";
                return;
            }
        }

        // Go
        // Photon Nickname Change
        readyChat.nicknameChanged(PhotonNetwork.LocalPlayer.NickName, newNickname.text);
        PhotonNetwork.LocalPlayer.NickName = newNickname.text;

        // Player Nickname Text Change
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = players[0];
        foreach(GameObject p in players) if(p.GetComponent<PhotonView>().IsMine) player = p;
        player.GetComponent<CharacterInfoSetting>().setNickName(newNickname.text);
        newNickname.text = "";

        OffNicknameChangeWindowBtn();
    }

    #region "On/OFF Panels"

    public void OnNicknameChangeWindowBtn()
    {
        // if(!countdownUI.isCounting)
        nicknameChangeWindow.GetComponent<WindowAnimation>().OnAnimUI();
    }

    public void OffNicknameChangeWindowBtn()
    {
        nicknameChangeWindow.GetComponent<WindowAnimation>().OffAnimUI();
    }

    public void OnGameInfoWindow()
    {
        informationWindow.GetComponent<WindowAnimation>().OnAnimUI();
    }

    public void OffGameInfoWindow()
    {
        informationWindow.GetComponent<WindowAnimation>().OffAnimUI();
    }

    public void OnSettingWindow()
    {
        settingWindow.GetComponent<WindowAnimation>().OnAnimUI();
    }

    public void OffSettingWindow()
    {
        settingWindow.GetComponent<WindowAnimation>().OffAnimUI();
    }

    public void OnReadyChatWindow()
    {
        readyChat = readyObj.transform.Find("OnlyReadyScene/ReadyChatManager").GetComponent<ReadyChat>();
        readyChat.startMeeting();
    }

    public void OffReadyChatWindow()
    {
        readyChat.exitChat();
    }

    // Attached at ReadyObject/MainMobileUI/MeetingStartButton
    public void OnChatWindow()
    {
        chatManager.startMeeting();
    }

    public void OnCustomizeWindow()
    {
        customizeItem.onCustomizeWindow();
    }

    public void OffCustomizeWindow()
    {
        customizeItem.exitWindow();
    }

    public void OnMissionWindow()
    {
        missionNumSettingS.value = GameManagerScript.instance.missionNumber;
        // if(countdownUI != null)
        // {
        //     if(!countdownUI.isCounting)
        //     {
        //         // if(GameManagerScript.instance.missionNumber != 0)
        //         // {
        //         //     missionNumSetting.text = GameManagerScript.instance.missionNumber.ToString();
        //         // }
                warningText.text = "미션 개수를 4 ~ 6 개로 설정해주세요.";
                warningText.color = Color.black;
                missionNumSetWindow.GetComponent<WindowAnimation>().OnAnimUI();
        //     }    
        // }
        // else missionNumSetWindow.GetComponent<WindowAnimation>().OnAnimUI();
    }

    public void MissionNumSet()
    {
        int num = (int)missionNumSettingS.value;

        // if(string.IsNullOrEmpty(missionNumSetting.text))
        // {
        //     GameManagerScript.instance.missionNumber = 6;
        // }
        // else if(int.TryParse(numString, out num))
        // {
        //     if(num > 3 && num < 7)
        //     {
        //         GameManagerScript.instance.missionNumber = num;
        //         missionNumSetWindow.GetComponent<WindowAnimation>().OffAnimUI();
        //     }
        //     else
        //     {
        //         warningText.text = "4 ~ 6 개로만 미션을 설정할 수 있습니다.";
        //         warningText.color = Color.red;
        //     }
        // }
        
        GameManagerScript.instance.missionNumber = num;
        missionNumSetWindow.GetComponent<WindowAnimation>().OffAnimUI();
    }

    public void OffMissionWindow()
    {
        missionNumSetWindow.GetComponent<WindowAnimation>().OffAnimUI();
    }

    #endregion

}
