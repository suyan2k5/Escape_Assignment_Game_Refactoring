using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GraduateManager : MonoBehaviourPunCallbacks
{
    // Change Role
    CharacterInfoSetting ownInfo;
    GameObject ChangeRoleWindow;
    Sprite[] changeWindows;
    public static bool canGraduateWin = false;
    
    Button stu, prof, gra;
    // Start is called before the first frame update
    void Start()
    {
        ownInfo = gameObject.GetComponent<CharacterInfoSetting>();
        GameObject windowUI = GameObject.Find("WindowUI");
        ChangeRoleWindow = windowUI.transform.GetChild(2).gameObject;
        stu = ChangeRoleWindow.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        gra = ChangeRoleWindow.transform.GetChild(1).GetChild(1).GetComponent<Button>();
        prof = ChangeRoleWindow.transform.GetChild(1).GetChild(2).GetComponent<Button>();
        stu.onClick.AddListener(ChangeToStudent);
        prof.onClick.AddListener(ChangeToProfessor);
        gra.onClick.AddListener(RemainGraduate);
    }

    // Update is called once per frame
    void Update()
    {
        if(ownInfo.role == "graduate" && ownInfo.isMissionAssigned && ownInfo.missionRemained <= 0 && ownInfo.isAlive)
        {
            if(gameObject.GetComponent<PhotonView>().IsMine)
            {
                ChangeRoleWindow.GetComponent<WindowAnimation>().OnAnimUI();
                int num = Random.Range(0,3);
                ChangeRoleWindow.GetComponent<Image>().sprite = changeWindows[num];
            }
        }
    }

    public void SetImages(Sprite[] image)
    {
        changeWindows = image;
    }

    public void ChangeToStudent()
    {
        gameObject.GetComponent<PhotonView>().RPC("RoleChange", RpcTarget.AllBuffered, "student");
        // GameObject missionProgressChecker = GameManagerScript.instance.readyObj.transform.Find("MainMobileUI/MissionProgressBar").gameObject;
        // missionProgressChecker.GetComponent<MissionProgressChecker>().BarInfoChangeRunner(1, ownInfo.assignedMission.Count);
        gameObject.GetComponent<CharacterInfoSetting>().gradAdditionalMission();
        // missionProgressChecker.GetComponent<MissionProgressChecker>().BarInfoChangeRunner(0, ownInfo.assignedMission.Count);
        MissionManager.mission.SetSolvedRate();
        ChangeRoleWindow.GetComponent<WindowAnimation>().OffAnimUI();
        GameObject[] missionObjects = GameObject.FindGameObjectsWithTag("Mission Object");
        for(int i = 0; i < missionObjects.Length; i++)
        {
            missionObjects[i].GetComponent<MissionDistanceCheck>().checkMission();
        }
        Destroy(this);
    }

    public void ChangeToProfessor()
    {
        gameObject.GetComponent<PhotonView>().RPC("RoleChange", RpcTarget.AllBuffered, "professor");
        GameManagerScript.instance.readyObj.transform.Find("MainMobileUI/AssignmentButton").gameObject.SetActive(true);
        ChangeRoleWindow.GetComponent<WindowAnimation>().OffAnimUI();
        Destroy(this);
    }

    public void RemainGraduate()
    {
        ChangeRoleWindow.GetComponent<WindowAnimation>().OffAnimUI();
        canGraduateWin = true;
        Destroy(this);
    }
}
