using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewToiletPaper : MonoBehaviour
{
    [SerializeField] private ToiletPaperUI toiletPaperUI;
    [SerializeField] private PaperTube paperTube;

    AudioSource sfx;
    public AudioClip skk;

    private void Start() 
    {
        sfx = GameObject.Find("SFx").GetComponent<AudioSource>();
        gameObject.GetComponent<ObjectDrag>().axisObject = null;   
    }

    private void Update()
    {
        if(GetComponent<ObjectDrag>().isDrag)
        {
            if(sfx.isPlaying) return;
            sfx.clip = skk;
            sfx.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.name);
        if(other.gameObject.tag == "Finish" && paperTube.isCollideTube)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-77, 10);
            toiletPaperUI.clearMission();
        }
    }
}
