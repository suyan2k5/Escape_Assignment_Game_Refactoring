using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    public GameObject axisObject;
    public bool useAxisObject, isDrag;
    Vector2 mousepos, axispos, topos, mypos;
    float angle;
    new Camera camera;

    public GameObject LeftBottomObject;
    public GameObject RightTopObject;
    float minx, miny, maxx, maxy;
    bool isClick;

    void Start()
    {
        camera = GameObject.Find("Main Move Camera").GetComponent<Camera>();
        mypos = gameObject.transform.position;
        isClick = false;
        isDrag = false;
        if(axisObject == null)
        {
            axisObject = gameObject;
        }
        if(LeftBottomObject == null)
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
    }

    private void Update()
    {
        if(isClick)
        {
            if (useAxisObject == false)
            {
                mousepos = camera.ScreenToWorldPoint(Input.mousePosition);
                posModify(ref mousepos);
                gameObject.transform.position = mousepos;
            }
            else
            {
                mousepos = (Vector2)camera.ScreenToWorldPoint(Input.mousePosition) - mypos;
                axispos = (Vector2)axisObject.transform.position - mypos;
                angle = Vector2.Angle(mousepos, axispos);
                topos = mousepos.magnitude * Mathf.Cos(angle * Mathf.PI / 180) * axispos.normalized + mypos;
                posModify(ref topos);
                gameObject.transform.position = topos;
            }
        }
    }

    private void OnMouseDown()
    {
        isClick = true;
        isDrag = true;
    }

    private void OnMouseUp()
    {
        isClick = false;
        isDrag = false;
    }

    public void posModify(ref Vector2 pos)
    {
        bool inRangex = minx <= pos.x && pos.x <= maxx;
        bool inRangey = miny <= pos.y && pos.y <= maxy;
        if(inRangex == false)
        {
            pos.x = pos.x < minx ? minx : maxx;
        }
        if(inRangey == false)
        {
            pos.y = pos.y < miny ? miny : maxy;
        }
    }
}
