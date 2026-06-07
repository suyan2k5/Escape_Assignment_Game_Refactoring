using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    // 상호 작용 버튼 클릭 시 작동될 기능 정의
    GameObject[] missionObject;
    int length;

    public void ButtonClick()
    {
        missionObject = GameObject.FindGameObjectsWithTag("Mission Object");
        length = missionObject.Length;
        if (length != 0)
        {
            // Debug.Log("미션 오브젝트 발견했다!");
            for(int i = 0; i < missionObject.Length; i++)
            {
                missionObject[i].GetComponent<MissionDistanceCheck>().DistanceCheck();
            }
        }
        else
        {
            Debug.Log("미션 오브젝트가 아무것도 없어용");
        }
    }
}
