using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    Canvas canvas;

    void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        StartCoroutine("cameraSetting");
    }

    IEnumerator cameraSetting()
    {
        GameObject camera = null;
        while(true)
        {
            camera = GameObject.Find("Main Move Camera");
            if(camera != null) break;
            yield return new WaitForSeconds(0.2f);
        }
        canvas.worldCamera = camera.GetComponent<Camera>();
    }
}
