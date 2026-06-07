using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class PlayerUISettingScript : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text RoomInfoText;      //처음에 반짝 거리는 위치 정보 출력
    [SerializeField] private Text RoomInInfo;    //정보창 내의 위치 정보
    [SerializeField] private Text Mission1;
    [SerializeField] private Text Mission2;
    [SerializeField] private Text Mission3;
    [SerializeField] private Text Mission4;
    [SerializeField] private Text Mission5;
    [SerializeField] private Text Mission6;
    [SerializeField] private GameObject MissionDone1;
    [SerializeField] private GameObject MissionDone2;
    [SerializeField] private GameObject MissionDone3;
    [SerializeField] private GameObject MissionDone4;
    [SerializeField] private GameObject MissionDone5;
    [SerializeField] private GameObject MissionDone6;    
    [SerializeField] private GameObject AssignmentStack1;
    [SerializeField] private GameObject AssignmentStack2;
    [SerializeField] private GameObject AssignmentStack3;
    [SerializeField] private GameObject AssingmentButton;
    [SerializeField] private GameObject SabotageButton;
    [SerializeField] private GameObject InteractionButton;
    [SerializeField] private GameObject InfoButton;
    [SerializeField] private GameObject meetingButton;
    [SerializeField] private GameObject customizeButton, customizePanelButton;
    [SerializeField] private GameObject readyChatButton;
    [SerializeField] private GameObject exitRoomButton;
    [SerializeField] private Sprite[] sabotageImages;
    [SerializeField] private GameObject locationSpot;
    public bool isClassroomDetermined = false;
    GameObject player;

    public void roomSettingUI(int roomNum)
    {
        if(roomNum != -1)   RoomInfoText.text = "My Room : " + roomNum;
        RoomInInfo.text = "You are in Room : " + "Spawn";
        isClassroomDetermined = true;
    }

    public void setStudentRoomUI()
    {
        StartCoroutine("roomInfoUI");
    }

    //초기 화면에 위치 지정시키는 함수
    IEnumerator roomInfoUI()
    {
        if(isClassroomDetermined)
        {
            RoomInfoText.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(5f);
        RoomInfoText.gameObject.SetActive(false);
    }

    public IEnumerator setInfoRoom(string roomName)
    {
        while(!RoomInInfo)
        {
            print("Wait...");
            Transform readyObj = GameManagerScript.instance.readyObj.transform;
            if(readyObj.Find("WindowUI")) RoomInInfo = readyObj.Find("WindowUI/GameInfoWindow/Panel/RoomInfo").GetComponent<Text>();
            yield return new WaitForSeconds(0.5f);
        }
        RoomInInfo.text = "You are in Room : " + roomName;
        
        if (roomName == "Lecture1")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-647, 52);
        }
        else if (roomName == "Lecture2")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-544, 81);
        }
        else if (roomName == "Lecture3")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-425, 81);
        }
        else if (roomName == "Lecture4")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-644, -282);
        }
        else if (roomName == "Lecture5")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-524, -327);
        }
        else if (roomName == "Lecture6")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-373, -327);
        }
        else if (roomName == "Store")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-195, 74);
        }
        else if (roomName == "DiningRoom")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-37, -106);
        }
        else if (roomName == "Entrance")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-647, -113);
        }
        else if (roomName == "Garden")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-420, -113);
        }
        else if (roomName == "Kitchen")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-46, 74);
        }
        else if (roomName == "Lobby")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-118, -113);
        }
        else if (roomName == "ManToilet")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-346, 83);
        }
        else if (roomName == "Spawn")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-222, -325);
        }
        else if (roomName == "WomanToilet")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-295, 83);
        }
        else if (roomName == "StudyRoom")
        {
            locationSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-56, -321);
        }
    }

    public void missionSettingUI(List<Mission> missions)
    {
        Mission1.GetComponent<Text>().text = missions[0].mission;
        Mission2.GetComponent<Text>().text = missions[1].mission;
        
        if(missions.Count > 2)
        {
            Mission3.GetComponent<Text>().text = missions[2].mission;
        }
        if(missions.Count > 3)
        {
            Mission4.GetComponent<Text>().text = missions[3].mission;
        }
        if(missions.Count > 4)
        {
            Mission5.GetComponent<Text>().text = missions[4].mission;
        }
        if(missions.Count > 5)
        {
            Mission6.GetComponent<Text>().text = missions[5].mission;
        }
    }

    public void gradMissionSettingUI(List<Mission> gradMissions)
    {
        Mission1.GetComponent<Text>().text = gradMissions[0].mission;
        Mission2.GetComponent<Text>().text = gradMissions[1].mission;
        Mission3.GetComponent<Text>().text = gradMissions[2].mission;
        Mission4.GetComponent<Text>().text = gradMissions[3].mission;
        if(gradMissions.Count > 4)
        {
            Mission5.GetComponent<Text>().text = gradMissions[4].mission;
        }
        if(gradMissions.Count > 5)
        {
            Mission6.GetComponent<Text>().text = gradMissions[5].mission;
        }
    }

    public void missionDoneUI(int num)
    {
        switch(num)
        {
            case 0 :
                MissionDone1.SetActive(true);
                break;
            case 1 :
                MissionDone2.SetActive(true);
                break;
            case 2 :
                MissionDone3.SetActive(true);
                break;
            case 3 :
                MissionDone4.SetActive(true);
                break;
            case 4 :
                MissionDone5.SetActive(true);
                break;
            case 5 :
                MissionDone6.SetActive(true);
                break;
        }

        // GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // GameObject player = players[0];
        // for(int i = 0; i < players.Length; i++) if(players[i].GetComponent<PhotonView>().IsMine) player = players[i];
        // if(player.GetComponent<CharacterInfoSetting>().role == "student" && player.GetComponent<CharacterInfoSetting>().isAlive)
        // {
        //     MissionProgressChecker missionProgressChecker = GameManagerScript.instance.readyObj.transform.Find("MainMobileUI/MissionProgressBar").GetComponent<MissionProgressChecker>();
        //     missionProgressChecker.missionClear();
        // }

        // player.GetComponent<CharacterInfoSetting>().missionRemained -= 1;
    }

    public void addAssignmentStack(int num)
    {
        GameObject [] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PhotonView>().IsMine) player = players[i];
        }
        if(player.GetComponent<CharacterInfoSetting>().role == "graduate")
        {
            AssingmentButton.SetActive(true);
        }
        AssignmentStack1.SetActive(false);
        AssignmentStack2.SetActive(false);
        AssignmentStack3.SetActive(false);
        if(num > 0)
        {
            AssignmentStack1.SetActive(true);
        }
        if(num > 1)
        {
            AssignmentStack2.SetActive(true);
        }
        if(num > 2)
        {
            AssignmentStack3.SetActive(true);
        }
    }

    public void decAssignmentStack(int num)
    {
        GameObject [] players = GameObject.FindGameObjectsWithTag("Player");
        AssignmentStack1.SetActive(false);
        AssignmentStack2.SetActive(false);
        AssignmentStack3.SetActive(false);
        if(num > 0)
        {
            AssignmentStack1.SetActive(true);
        }
        if(num > 1)
        {
            AssignmentStack2.SetActive(true);
        }
        if(num > 2)
        {
            AssignmentStack3.SetActive(true);
        }
    }

    public void OnAssignmentButton()
    {
        AssingmentButton.SetActive(true);
        AssingmentButton.GetComponent<AssignmentManager>().curtime = 5.0f;
    }

    public void OnSabotageButton()
    {
        SabotageButton.SetActive(true);
        GameManagerScript.instance.uiManager.GetComponent<Sabotage>().MyAwake();
    }

    public void SetProfessorSabotage()
    {
        SabotageButton.GetComponent<Image>().sprite = sabotageImages[0];
        GameManagerScript.instance.uiManager.GetComponent<Sabotage>().SetProfessorMode();
    }

    public void SetStuNGraSabotage()
    {
        SabotageButton.GetComponent<Image>().sprite = sabotageImages[1];
        GameManagerScript.instance.uiManager.GetComponent<Sabotage>().SetStuNGraMode();
    }

    public void onCustomizeButton()
    {
        customizeButton.SetActive(true);
        customizePanelButton.SetActive(true);
    }

    public void offCustomizeButton()
    {
        customizeButton.SetActive(false);
        customizePanelButton.SetActive(false);
    }

    public void ButtonsForInGame()
    {
        InteractionButton.SetActive(true);
        InfoButton.SetActive(true);
        offCustomizeButton();
        readyChatButton.SetActive(false);
        exitRoomButton.SetActive(false);
    }

    public void onMeetingButton()
    {
        meetingButton.SetActive(true);
    }

    public void offMeetingButton()
    {
        meetingButton.SetActive(false);
    }

}
