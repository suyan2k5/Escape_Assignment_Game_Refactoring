using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageControl : MonoBehaviour
{
    public string msgType;  // User, Announcement
    public Vector2 boxProperDist = new Vector2(29.5f, 18f); // (1139-1080)/2, (94-58)/2
    public float heightProperDist = 30f;
    [SerializeField] private GameObject imgObj; //Sender Image
    [SerializeField] private GameObject nicknameObj; //Sender Nickname
    [SerializeField] private GameObject boxObj; //Message Box
    [SerializeField] private GameObject textObj; //Received text
    private float msgHeight;
    
    public void boxResize()
    {
        //Box Resize
        boxObj.transform.localPosition = new Vector2(textObj.transform.localPosition.x, textObj.transform.localPosition.y + boxProperDist.y);
        RectTransform boxrect = boxObj.GetComponent<RectTransform>();
        Text text = textObj.GetComponent<Text>();
        boxrect.sizeDelta = new Vector2(textObj.GetComponent<RectTransform>().rect.width + 2 * boxProperDist.x, text.preferredHeight + 2 * boxProperDist.y);
    }

    public float getHeight()
    {
        if(msgType == "User")
            msgHeight = getVerticalPosition(imgObj, "top") - getVerticalPosition(boxObj, "bottom") + heightProperDist;
        else if(msgType == "Announcement")
            msgHeight = gameObject.GetComponent<RectTransform>().rect.height + heightProperDist;

        return msgHeight;
    }

    // for non-message object
    public float getVerticalPosition(GameObject obj, string mode)
    {
        //Can Use only 0 <= pivoty <= 1
        //minus will get bottom y-position, and add will get top y-position
        float pivoty = obj.GetComponent<RectTransform>().pivot.y;
        if(mode == "bottom") return obj.transform.localPosition.y - obj.GetComponent<RectTransform>().rect.height * pivoty;
        else if(mode == "top") return obj.transform.localPosition.y + obj.GetComponent<RectTransform>().rect.height * (1 - pivoty);
        else return 0;
    }

    // for msg object
    public float getVerticalPosition(string mode)
    {
        if(mode == "bottom") return gameObject.transform.localPosition.y - this.getHeight();
        else if(mode == "top") return gameObject.transform.localPosition.y;
        else return 0;
    }
}
