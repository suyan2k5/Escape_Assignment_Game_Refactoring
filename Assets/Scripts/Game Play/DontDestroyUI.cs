using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyUI : MonoBehaviour
{
    //시작 시에 설정한 게임 오브젝트를 dontDestroyOnLoad 로 지정
    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);
    }

}
