using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// To-Do
// Top & Bottom Boundary add

public class MsgListDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject msgList;
    GameObject msgMask;
    float prevy;
    bool isInput = false;

    private static GameObject _firstMsg, _lastMsg;
    public static GameObject firstMsg
    { 
        set { if(value.GetComponent<MessageControl>() != null) _firstMsg = value; }
        get { return _firstMsg; }
    }
    public static GameObject lastMsg
    {
        set { if(value.GetComponent<MessageControl>() != null) _lastMsg = value; }
        get { return _lastMsg; }
    }

    public GameObject testP;

    private void Awake()
    {
        msgMask = msgList.transform.parent.gameObject;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        prevy = pos.y;

        isInput = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        msgListPosition(1);
    }

    public void msgListPosition(int mode)
    {
        // mode == 0 => list must go down
        // mode == else => normal drag

        if(_firstMsg == null) return;
        float msgListTop = _firstMsg.GetComponent<MessageControl>().getVerticalPosition("top") + msgList.transform.localPosition.y - 275f;
        float msgListBottom = _lastMsg.GetComponent<MessageControl>().getVerticalPosition("bottom") + msgList.transform.localPosition.y - 275f;
        float msgListHeight = msgListTop - msgListBottom;
        float maskHeight = msgMask.GetComponent<RectTransform>().rect.height;
        // print(msgListTop);
        // print(msgListBottom);

        //메시지 전체 길이가 마스크 길이보다 작을 때
        if(msgListHeight < maskHeight)
        {
            print("short");
            msgList.transform.localPosition = new Vector2(0, 275f);
        }
        //메시지 전체 길이가 마스크 길이보다 클 때
        else
        {
            if(mode == 0)
            {
                msgList.transform.localPosition = new Vector2(0, -275f + msgListHeight);
                return;
            }
            print("long");
            if(msgListTop < 25f)
            {
                print("up");
                msgList.transform.localPosition = new Vector2(0, 275f);
            }
            else if(msgListBottom > -575f)
            {
                print("down");
                msgList.transform.localPosition = new Vector2(0, -275f + msgListHeight);
            }
        }

        isInput = false;
    }

    void Update()
    {
        if(isInput)
        {
            float mousePosy = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            msgList.transform.Translate(new Vector2(0, mousePosy - prevy));

            prevy = mousePosy;
        }
    }
}
