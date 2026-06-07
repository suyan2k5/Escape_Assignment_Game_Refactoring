using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MissionProgressChecker : MonoBehaviourPunCallbacks//, IPunObservable
{
    public Slider slider;
    bool setDone = false, gameEnd = false;
    public int missionNum = -1, curClearNum;

    public PhotonView PV;
    private bool isMissionNumInitialized = false;
    public List<int> pmns = new List<int>(); // Player Mission Numbers
    private GameObject[] players;
    public Toggle test;
    public Text testt;

    // Update is called once per frame
    void Update()
    {return;
        // test.isOn = setDone;
        if (setDone)
        {
            // slider.maxValue = missionNum;
            if (curClearNum <= missionNum) slider.value = curClearNum;
            else { slider.value = slider.maxValue; curClearNum = missionNum; }
            if (!gameEnd && (curClearNum == missionNum) && curClearNum != 0)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].GetComponent<PhotonView>().RPC("StudentWin", RpcTarget.AllBuffered);
                }
                gameEnd = true;
                print("Student Win! If it is printed with starting game, RESTART is recommanded.");
            }
        }
    }

    // public void countMissions()
    // {
    //     print("Count Mission");
    //     // missionNum = 0;
    //     // GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //     // for (int i = 0; i < players.Length; i++)
    //     // {
    //     //     CharacterInfoSetting info = players[i].GetComponent<CharacterInfoSetting>();
    //     //     if (info.role == "student")
    //     //     {
    //     //         // if (info.assignedMission == null)
    //     //         // {
    //     //         //     return;
    //     //         // }
    //     //         // missionNum += info.assignedMission.Count;
    //     //     }
    //     // }
    //     PV.RPC("setDoneTrue", RpcTarget.AllBuffered);
    //     PV.RPC("SetSlider", RpcTarget.AllBuffered); print("SetSlider RPC Run");
    // }

    // public void missionClear()
    // {
    //     BarInfoChangeRunner(1, 1);
    // }

    // [PunRPC]
    // public void missionClearRPC()
    // {
    //     curClearNum++;
    // }

    // public void gameStart()
    // {
    //     countMissions();
    // }

    // [PunRPC]
    public void setDoneTrue()
    {
        setDone = true;
        print("Set Done True");
    }

    // [PunRPC]
    public void SetSlider()
    {
        slider.maxValue = missionNum;
        print("Set Slider");
    }

    // CharacterInfoSetting / [RPC] sendMissionSettingRPC

    public void BarInfoChangeRunner(int mode, int num)
    {
        print("Call BarInfoChange RPC with mode " + mode + " and num " + num);
        PV.RPC("BarInfoChange", RpcTarget.AllBuffered, mode, num);
    }

    [PunRPC]
    public void BarInfoChange(int mode, int num)
    {
        // testt.text += sender + " : Call BarInfoChange RPC with mode " + mode + " and num " + num + "\n";
        if (mode == 0)
        {
            if(!isMissionNumInitialized)
            {
                missionNum = num;
                isMissionNumInitialized = true;
            }
            else
            {
                missionNum += num;
            }

            if(!setDone)
            {
                int studentNum = 0;
                CharacterInfoSetting[] psi = GameManagerScript.instance.getPlayersInfo();
                for(int i = 0; i < psi.Length; i++)
                {
                    if(psi[i].role == "student") studentNum++;
                }
                pmns.Add(num);
                if(pmns.Count == studentNum) setDoneTrue();
            }
        }
        else if (mode == 1)
        {
            curClearNum += num;
        }
        print("Bar Info Change: missionNum = " + missionNum + ", curClearNum = " + curClearNum);
        SetSlider();
    }

    public void ChangeBar(float missionSolvedRate)
    {
        PV.RPC("ChangeBarRPC", RpcTarget.All, missionSolvedRate);
    }

    [PunRPC]
    public void ChangeBarRPC(float missionSolvedRate)
    {
        slider.value = missionSolvedRate;

        if(missionSolvedRate != 1) return;
        
        GameObject[] players = GameManagerScript.instance.getPlayers();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PhotonView>().RPC("StudentWin", RpcTarget.All);
        }
        gameEnd = true;
        print("Student Win! If it is printed with starting game, RESTART is recommanded.");
    }

    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     if (stream.IsWriting)
    //     {
    //         // stream.SendNext(missionNum);
    //         stream.SendNext(curClearNum);
    //     }
    //     else
    //     {
    //         // missionNum = (int)stream.ReceiveNext();
    //         curClearNum = (int)stream.ReceiveNext();
    //     }
    // }
}
