using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Sabotage : MonoBehaviourPunCallbacks
{
    [SerializeField] Button sabotageButton;
    private GameObject[] players;
    CharacterInfoSetting playerInfo;
    private List<SabotagePlayer> sabotageTargets = new List<SabotagePlayer>();
    public GameObject profSabotagePanel, stuNGraSabotagePanel;
    public GameObject sabotageLightWindow;
    public float profLightCooltime = 0.0f, stuNGraLightCooltime = 0.0f;
    public bool profMode = false, stuNGraMode = false;
    public Text lightCoolTimeText;
    public PhotonView PV;

    public void MyAwake()
    {
        stuNGraLightCooltime = 15.0f;
        players = GameManagerScript.instance.getPlayers();
        playerInfo = GameManagerScript.instance.getMyPlayerInfo();
        for (int i = 0; i < players.Length; i++)
        {
            sabotageTargets.Add(players[i].GetComponent<SabotagePlayer>());
        }
        lightCoolTimeText.text = "";

        // if (playerInfo.role == "professor")
        // {
        //     profSabo = true;
        //     sabotageButton.onClick.RemoveAllListeners();
        //     sabotageButton.onClick.AddListener(ProfSabotageButton);
        // }
        // else if (playerInfo.isAlive == false)
        // {
        //     stuNGraSabo = true;
        //     sabotageButton.onClick.RemoveAllListeners();
        //     sabotageButton.onClick.AddListener(StuNGraSabotageButton);
        // }
    }

    private void Update()
    {
        if (stuNGraMode && stuNGraLightCooltime > 0)
        {
            print("여길 인식하지 못함");
            stuNGraLightCooltime -= Time.deltaTime; //안됨
            lightCoolTimeText.text = Mathf.Ceil(stuNGraLightCooltime).ToString();
        }
        // Cool time decrease for professor sabotage button
        else if (profMode && profLightCooltime > 0)
        {
            profLightCooltime -= Time.deltaTime;
            lightCoolTimeText.text = Mathf.Ceil(profLightCooltime).ToString();
        }
        // Cool time decrease for student & graduate sabotage button
        else
        {
            lightCoolTimeText.text = "";
        }
    }
    
    [PunRPC]
    void SetProfCooltime(float num)
    {
        // Professor Cooltime Set
        profLightCooltime = num;
    }

    [PunRPC]
    void SetStuNGraCooltime(float num)
    {
        // Student & Graduate Cooltime Set
        stuNGraLightCooltime = num;
    }

    public void SetProfessorMode()
    {
        this.profMode = true;

        print("Sabotage.cs: Professor Mode On");
    }

    public void SetStuNGraMode()
    {
        this.stuNGraMode = true;

        print("Sabotage.cs: Student & Graduate Mode On");
    }

    public void SabotageButtonClick()
    {
        playerInfo = GameManagerScript.instance.getMyPlayerInfo();
        print("cpol time : " + stuNGraLightCooltime);
        if (stuNGraMode && stuNGraLightCooltime <= 0.0f)
        {
            StuNGraSabotagePanelOn();
        }
        else if (profMode && profLightCooltime <= 0.0f)
        {
            ProfSabotagePanelOn();
        }
    }

    #region Professor -> Undergraduate & Graduate Sabotage Button
    public void ProfSabotagePanelOn()
    {
        profSabotagePanel.GetComponent<WindowAnimation>().OnAnimUI();
    }

    public void ProfSabotagePanelOff()
    {
        sabotageLightWindow.SetActive(false);
        profSabotagePanel.GetComponent<WindowAnimation>().OffAnimUI();
    }

    public void ProfLightButton()
    {
        sabotageLightWindow.SetActive(true);
    }

    public void ProfLightOff(string sceneName)
    {
        print("Choose Light Off");
        for (int i = 0; i < sabotageTargets.Count; i++)
        {
            if (sabotageTargets[i].GetComponent<CharacterInfoSetting>().currentSceneName == sceneName)
            {
                sabotageTargets[i].GetComponent<PhotonView>().RPC("RoomLightOff", RpcTarget.AllBuffered);
            }
        }
        sabotageLightWindow.SetActive(false);
        ProfSabotagePanelOff();
        print("Panel Off");
        PV.RPC("SetProfCooltime", RpcTarget.AllBuffered, 15f);
        print("Set Cooltime");
    }

    public void ProfInterruptButton()
    {
        print("Choose Interrupt Off");
        for (int i = 0; i < sabotageTargets.Count; i++)
        {
            if (sabotageTargets[i].GetComponent<CharacterInfoSetting>().missionSolvingNum >= 0)
            {
                sabotageTargets[i].GetComponent<PhotonView>().RPC("MissionOff", RpcTarget.AllBuffered);
                sabotageTargets[i].GetComponent<CharacterInfoSetting>().missionSolvingNum = -1;
            }
        }
        ProfSabotagePanelOff();
        print("Panel Off");
        PV.RPC("SetProfCooltime", RpcTarget.AllBuffered, 15f);
        print("Set Cooltime");
    }
    #endregion

    #region Student & Graduate Sabotage Button
    public void StuNGraSabotagePanelOn()
    {
        stuNGraSabotagePanel.GetComponent<WindowAnimation>().OnAnimUI();
    }

    public void StuNGraSabotagePanelOff()
    {
        stuNGraSabotagePanel.GetComponent<WindowAnimation>().OffAnimUI();
    }

    // UG & G can use when they're died.
    public void StuNGraCooltimeExpand()
    {
        print("Choose Cooltime Expand");
        for (int i = 0; i < sabotageTargets.Count; i++)
        {
            if (sabotageTargets[i].GetComponent<CharacterInfoSetting>().role == "professor")
            {
                sabotageTargets[i].GetComponent<PhotonView>().RPC("AssignmentCooltimeExpand", RpcTarget.AllBuffered);
            }
        }
        print("Panel Off");
        StuNGraSabotagePanelOff();
        print("Set Cooltime");
        PV.RPC("SetStuNGraCooltime", RpcTarget.AllBuffered, 15f);
    }

    public void StuNGraLightOff()
    {
        print("Choose Light Off");
        for(int i = 0; i < sabotageTargets.Count; i++)
        {
            if(sabotageTargets[i].GetComponent<CharacterInfoSetting>().role == "professor")
            {
                sabotageTargets[i].GetComponent<PhotonView>().RPC("ProfLightOff", RpcTarget.AllBuffered);
            }
        }
        print("Panel Off");
        StuNGraSabotagePanelOff();
        print("Set Cooltime");
        PV.RPC("SetStuNGraCooltime", RpcTarget.AllBuffered, 20f);
    }
    #endregion
}
