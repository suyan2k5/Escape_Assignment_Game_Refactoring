using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pourer : MonoBehaviour
{
    [SerializeField] private GameObject StartP;
    [SerializeField] private GameObject MaxP;
    float startPy, curPy, maxPy;
    float rotate;
    new Camera camera;

    AudioSource sfx;
    public AudioClip pour;

    private void Start()
    {
        camera = GameObject.Find("Main Move Camera").GetComponent<Camera>();
        startPy = StartP.transform.position.y;
        maxPy = MaxP.transform.position.y;
        sfx = GameObject.Find("SFx").GetComponent<AudioSource>();
    }

    private void OnMouseDrag()
    {
        curPy = camera.ScreenToWorldPoint(Input.mousePosition).y;
        if (startPy < curPy && curPy < maxPy)
        {
            rotate = 45f * (curPy - startPy) / (maxPy - startPy);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, rotate);
            if(sfx.isPlaying) return;
            sfx.clip = pour;
            sfx.Play();
        }
        else if(curPy >= maxPy)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 45f);
        }
        else if(curPy <= startPy)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0f);
        }
    }
}
