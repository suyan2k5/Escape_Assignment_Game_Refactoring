using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    [SerializeField] private GameObject LU;
    [SerializeField] private GameObject RD;
    Vector2 pos;
    Image objImage;
    bool isPut, isfaded;
    float fadecount, xmax, xmin, ymax, ymin;
    public FoodTrashUI UI;

    AudioSource sfx;
    public AudioClip put;

    private void Start()
    {
        fadecount = 1.0f;
        isPut = false;
        isfaded = false;
        xmax = RD.transform.position.x;
        xmin = LU.transform.position.x;
        ymax = LU.transform.position.y;
        ymin = RD.transform.position.y;
        objImage = gameObject.GetComponent<Image>();
        sfx = GameObject.Find("SFx").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isfaded && isPut)
        {
            Destroy(gameObject.GetComponent<ObjectDrag>());
            fadecount -= Time.deltaTime;
            if(fadecount >= 0) objImage.color = new Color(objImage.color.r, objImage.color.g, objImage.color.b, fadecount);
            else
            {
                isfaded = true;
                UI.numFood -= 1;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnMouseUp()
    {
        pos = gameObject.transform.position;
        if (isInRange(pos))
        {
            sfx.clip = put;
            sfx.Play();
            isPut = true;
        }
    }

    private bool isInRange(Vector2 position)
    {
        return xmin < position.x && position.x < xmax && ymin < position.y && position.y < ymax;
    }
}
