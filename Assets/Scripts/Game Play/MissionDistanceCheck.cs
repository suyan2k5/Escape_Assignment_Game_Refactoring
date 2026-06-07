using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MissionDistanceCheck : MonoBehaviourPunCallbacks
{
    // 플레이어와 미션 오브젝트의 거리에 따라 미션의 실행 여부를 결정하는 스크립트
    // + 미션 오브젝트가 플레이어가 수행 가능한 미션일 경우에 오브젝트 표시 스프라이트(화살표 모양) 활성화
    [SerializeField] private int roomNumber;        // 해당 미션 오브젝트가 있는 방의 번호
    [SerializeField] private int missionNumber;
    [SerializeField] private GameObject missionUI;
    [SerializeField] private GameObject missionObject;
    [SerializeField] private GameObject missionArrow;
    [SerializeField] private float range = 1.8f;
    GameObject player;
    Vector2 playerPos;
    Vector2 missionObjPos;
    private float distance;
    List<Mission> missionCheck;    
    private GameObject[] players;

    void Start()
    {
        checkMission();
    }

    public void checkMission()
    {
        player = GameManagerScript.instance.getMyPlayer();
        missionCheck = player.GetComponent<CharacterInfoSetting>().getMission();

        missionArrow.SetActive(false);

        for (int i = 0; i < missionCheck.Count; i++)
        {
            if (missionNumber == missionCheck[i].missionNum)
            {
                try
                {
                    MissionManager.mission.missionArrows.Add(i, missionArrow);
                    missionArrow.SetActive(true);
                }
                catch(System.ArgumentException ae)
                {
                    Debug.Log("MissionDistanceCheck.cs : MissionManager.missionArrow Key Duplicate -> Ignore");
                }
            }
        }
    }
    

    // 플레이어와 미션 오브젝트 사이의 거리를 재어 기준 거리와 비교하는 함수
    public void DistanceCheck()
    {
        // 거리 구하기
        playerPos = player.transform.position;
        missionObjPos = missionObject.transform.position;
        distance = new Vector2(missionObjPos.x - playerPos.x, missionObjPos.y - playerPos.y).magnitude;
        // Debug.Log(missionNumber + "번 미션 오브젝트와의 거리는 요종도: " + distance);

        // 거리 조건 true라면 미션 체크
        if(distance < range)
        {
            for(int i = 0; i < 27; i++)
            {
                print(i + " " + MissionManager.mission.missionObject_Active[i]);
                if (MissionManager.mission.missionObject_Active[i])
                {
                    //missionUI.SetActive(true);
                    if(null == GameObject.Find(Mission.getRoomName(missionNumber) + "MissionCanvas").transform.Find("MissionWindow" + missionNumber.ToString() + "(Clone)"))
                    {
                        GameObject mission = Instantiate(missionUI);
                        GameObject parent = GameObject.Find(Mission.getRoomName(missionNumber) + "MissionCanvas");
                        mission.transform.SetParent(parent.transform, false);
                        mission.GetComponent<WindowAnimation>().OnAnimUI();
                        mission.SetActive(true);
                        player.GetComponent<CharacterInfoSetting>().GetComponent<PhotonView>().RPC("setMissionNumber", RpcTarget.AllBuffered, missionNumber);
                    }
                    else return;
                }
            }
        }
    }
}
