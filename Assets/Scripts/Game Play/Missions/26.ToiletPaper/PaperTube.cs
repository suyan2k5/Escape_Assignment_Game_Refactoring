using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperTube : MonoBehaviour
{
    [SerializeField] private ToiletPaperUI toiletPaperUI;
    [SerializeField] private GameObject newToiletPaper;
    public bool isCollideTube = false;

    AudioSource sfx;
    public AudioClip skk;

    private void Awake()
    {
        sfx = GameObject.Find("SFx").GetComponent<AudioSource>();
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
        print("colllideeee");
        if(other.gameObject.tag == "collide")
        {
            print("collide1");
            newToiletPaper.GetComponent<ObjectDrag>().enabled = false;
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(343, 10);
            gameObject.GetComponent<ObjectDrag>().useAxisObject = false;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.8f;
        }
        else if(other.gameObject.name == "floor")
        {
            print("collide2");
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(277, -232);
            newToiletPaper.GetComponent<ObjectDrag>().enabled = true;
            isCollideTube = true;
        }
    }
}
