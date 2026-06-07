using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    // private void Start() {
    //     Debug.LogError("TEstsetsst");
    //     print("It Passed");
    // }
    public MissionProgressChecker mpc;
    
    public void MissionClear() {
        mpc.BarInfoChangeRunner(1, 1);
    }
}
