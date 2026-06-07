using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private GameObject Nambi;
    [SerializeField] private GameObject Img;
    [SerializeField] private GameObject Btn;
    [SerializeField] private GameObject SplashObj;
    public bool isOn, isPut;
    Vector2 mousepos;
    new Camera camera;
    float scale;

    public GameObject LeftBottomObject;
    public GameObject RightTopObject;
    float minx, miny, maxx, maxy;
    bool isClick;

    public AudioClip pong;
    AudioSource sfx;

    void Start()
    {
        isOn = false;
        isPut = false;
        Img.SetActive(true);
        Btn.SetActive(false);
        camera = GameObject.Find("Main Move Camera").GetComponent<Camera>();
        if (LeftBottomObject == null)
        {
            LeftBottomObject = gameObject;
        }
        if (RightTopObject == null)
        {
            RightTopObject = gameObject;
        }
        minx = LeftBottomObject.transform.position.x;
        miny = LeftBottomObject.transform.position.y;
        maxx = RightTopObject.transform.position.x;
        maxy = RightTopObject.transform.position.y;
        isClick = false;

        sfx = GameObject.Find("SFx").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isClick)
        {
            mousepos = camera.ScreenToWorldPoint(Input.mousePosition);
            posModify(ref mousepos);
            gameObject.transform.position = mousepos;
        }
    }

    private void OnMouseDown()
    {
        isClick = true;
    }

    private void OnMouseUp()
    {
        isClick = false;
        isIngOn();
    }

    public void isIngOn()
    {
        if (new Vector2(Nambi.transform.position.x - mousepos.x, Nambi.transform.position.y - mousepos.y).magnitude <= 2.5)
        {
            isOn = true;
            Btn.transform.position = gameObject.transform.position;
            // Btn.SetActive(true);
            // Img.SetActive(false);
        }
        else
        {
            isOn = false;
        }
    }

    public void click()
    {
        if(isOn && !isPut)
        {
            // Btn.SetActive(false);
            // Img.SetActive(true);
            StartCoroutine("FoodFall");
        }
    }

    public void posModify(ref Vector2 pos)
    {
        bool inRangex = minx <= pos.x && pos.x <= maxx;
        bool inRangey = miny <= pos.y && pos.y <= maxy;
        if (inRangex == false)
        {
            pos.x = pos.x < minx ? minx : maxx;
        }
        if (inRangey == false)
        {
            pos.y = pos.y < miny ? miny : maxy;
        }
    }

    IEnumerator FoodFall()
    {
        Transform imgTrans = gameObject.transform;
        scale = imgTrans.localScale.x;
        bool isInWater = false;
        bool isSplashed = false;
        while(scale > 0.6f)
        {
            if(isInWater == false && scale < 95)
            {
                gameObject.transform.SetAsFirstSibling();
                isInWater = true;
            }
            if(isSplashed == false && isInWater == true)
            {
                StartCoroutine("Splash");
                isSplashed = true;
            }
            scale -= 0.01f;
            imgTrans.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForSeconds(0.01f);
        }
        isPut = true;
    }

    IEnumerator Splash()
    {
        SplashObj.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        SplashObj.SetActive(false);

        sfx.clip = pong;
        sfx.Play();
    }
}
