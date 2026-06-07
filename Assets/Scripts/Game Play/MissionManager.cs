using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MissionManager : MonoBehaviourPunCallbacks
{
    //미션들을 전체적으로 관리하는 스크립트
    //미션 리스트 받아와서 랜덤 생성, 미션 생성 기능 담당
    [SerializeField] private CSVReader _csvReader;
    public Dictionary<int, GameObject> missionArrows = new Dictionary<int, GameObject>();
    public CharacterInfoSetting character;
    public static MissionManager mission;
    public int missionNum; 
    private List<Mission> missionList;
    private List<Mission> myMission = new List<Mission>();
    private List<Mission> gradMissionList = new List<Mission>();
    public List<int> myMissionNum = new List<int>();
    public List<int> gradMissionNum = new List<int>();
    public Dictionary<int, bool> missionObject_Active = new Dictionary<int, bool>();
    private int randomNum;
    private GameObject[] players;
    private double solvedMissionRate = 0;
    private int remainedNum;
    private int totalMissionNum; // Count the valid missions(student's mission)

    
    private void Start()
    {
        mission = this;
        missionArrows.Clear();
    }

    //CSV file reader로부터 미션 리스트 받아옴
    public void setMissionList()
    {
        missionList = _csvReader.GetMissionList();
        
        for(int i = 0; i < missionList.Count; i ++)
        {
            missionObject_Active.Add(i, false);
        }
    }

    //미션 랜덤 생성 위한 랜덤 함수(난수만 생성)
    private int ranNum()
    {
        int range = missionList.Count;
        randomNum = Random.Range(0, range);
        return randomNum;
    }

    [PunRPC]
    void getMyMission(List<Mission> missions)
    {
        myMission = missions;
    }

    public List<Mission> setMyMission(int missionNumber)
    {
        // number of missions set by master
        missionNum = missionNumber;
        
        // If a player's role is graduate, mission num 6->3, 5->3, 4->2.
        if(character.role == "graduate")
        {
            missionNumber = Mathf.CeilToInt((float)missionNum/2);
        }

        setMissionList();
        int i = 0;
        while(myMission.Count < missionNumber)
        {
            int ran = ranNum();
            Mission newMission = missionList[ran];
            myMission.Add(newMission);
            myMissionNum.Add(newMission.getMissionNum());
            myMission = myMission.Distinct().ToList();
            myMissionNum = myMissionNum.Distinct().ToList();
            if(i + 1 == myMission.Count)
            {
                i ++;
                Debug.Log(ran + " " + myMission[i - 1].mission);
                missionObject_Active[ran] = true;
            }
        }
        return myMission;   
    }

    public List<Mission> gradMission(List<Mission> prevMission)
    {
        gradMissionList = prevMission;
        // setMissionList();
        for(int i = 0; i < prevMission.Count; i++)
        {
            gradMissionNum.Add(prevMission[i].getMissionNum());
        }
        while(gradMissionList.Count < missionNum)
        {
            int ran = ranNum();
            Mission newMission = missionList[ran];
            if(!isThereSame(gradMissionList, newMission.mission))
            {
                gradMissionList.Add(newMission);
                gradMissionNum.Add(newMission.getMissionNum());
                character.missionRemained++;
                character.assignedMissionNum++;
            }
        }
        myMission = gradMissionList;
        myMissionNum = gradMissionNum;
        return gradMissionList;
    }

    private bool isThereSame(List<Mission> prevList, string newTarget)
    {
        for(int i = 0; i < prevList.Count; i ++)
        {
            if(prevList[i].mission == newTarget)    return true;
        }
        return false;
    }

    public void masterMissionSetting(int missionNumber)
    {
        //missionNum = missionNumber;
        players = GameManagerScript.instance.getPlayers();
        missionNum = missionNumber;
        for(int j = 0; j < players.Length; j++)
        {
            CharacterInfoSetting pinfo = players[j].GetComponent<CharacterInfoSetting>();
            // if(pinfo.role == "graduate")
            // {
            //     int gradMissionNum = 0;
            //     if(missionNum == 6)
            //     {
            //         gradMissionNum = 3;
            //     }
            //     else if(missionNum == 5)
            //     {
            //         gradMissionNum = 3;
            //     }
            //     else if(missionNum == 4)
            //     {
            //         gradMissionNum = 2;
            //     }
            //     players[j].GetComponent<PhotonView>().RPC("MissionSetting", RpcTarget.All, gradMissionNum); // CharacterInfoSetting
            // }
            // else
            // {
                players[j].GetComponent<PhotonView>().RPC("MissionSetting", RpcTarget.All, missionNumber); // CharacterInfoSetting
            // }
        }
    }

    public void setMissionSolved(int num)
    {
        character.getMission()[num].isSolved = true;
        missionObject_Active[num] = false;
        missionArrows[num].SetActive(false);
        if(character.missionRemained > 0) {character.missionRemained--;Debug.LogError(character.missionRemained);}
        else Debug.LogError("CharacterInfoSetting.missionRemained < 0");
        SetSolvedRate();
    }

    public void SetSolvedRate()
    {
        totalMissionNum = 0;
        remainedNum = 0;

        CharacterInfoSetting[] playersInfo = GameManagerScript.instance.getPlayersInfo();

        foreach(CharacterInfoSetting pInfo in playersInfo)
        {
            if(pInfo.role == "student" && pInfo.isAlive)
            {
                totalMissionNum += pInfo.assignedMissionNum;
                remainedNum += pInfo.missionRemained;
            }
        }
        solvedMissionRate = (double)(totalMissionNum - remainedNum) / (double)totalMissionNum;
        //mission progress bar setting
        MissionProgressChecker mpc = GameManagerScript.instance.uiManager.missionProgressBar.GetComponent<MissionProgressChecker>();
        mpc.ChangeBar((float)solvedMissionRate);
    }
}
