using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void pointerEnter()
    {
        if(!(gameObject.transform.GetChild(0).name == "Exit"))  gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(55f, 3f);
        else    gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(33f, 0.143f);
        gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.0f);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void pointerExit()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1.0f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
