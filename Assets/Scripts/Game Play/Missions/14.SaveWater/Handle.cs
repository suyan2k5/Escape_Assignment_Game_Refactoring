using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    [SerializeField] private GameObject[] pos = new GameObject[2];
    float minPy, maxPy;
    float curPy;
    public float rotate;
    new Camera camera;

    private void Start()
    {
        camera = GameObject.Find("Main Move Camera").GetComponent<Camera>();
        minPy = pos[0].transform.position.y;
        maxPy = pos[1].transform.position.y;
        rotate = 0f;
    }

    private void OnMouseDrag()
    {
        curPy = camera.ScreenToWorldPoint(Input.mousePosition).y;
        if (minPy < curPy && curPy < maxPy)
        {
            rotate = 40f * (maxPy - curPy) / (maxPy - minPy);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, rotate);
        }
        else if(curPy >= maxPy)
        {
            rotate = 0f;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, rotate);
        }
        else if (minPy >= curPy)
        {
            rotate = 40f;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, rotate);
        }
    }
}
