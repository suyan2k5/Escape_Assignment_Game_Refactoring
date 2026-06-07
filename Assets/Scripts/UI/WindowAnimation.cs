using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowAnimation : MonoBehaviour
{
    public GameObject animPanel;
    public GameObject origPanel;
    private Vector3 origScale;
    private float animTime = 0.1f;

    private void Start()
    {
        // animPanel.SetActive(false);
        animPanel.transform.localScale = Vector2.zero;
        origPanel.SetActive(false);
        origScale = origPanel.transform.localScale;
    }

    public void OnAnimUI()
    {
        gameObject.SetActive(true);
        StartCoroutine(OnAnimUIEnum());
    }

    IEnumerator OnAnimUIEnum()
    {
        animPanel.SetActive(true);

        float accTime = 0f;
        while(accTime < animTime)
        {
            animPanel.transform.localScale = origScale * accTime / animTime;
            accTime += Time.deltaTime;
            yield return null;
        }

        animPanel.SetActive(false);
        origPanel.SetActive(true);
    }

    public void OffAnimUI()
    {
        StartCoroutine(OffAnimUIEnum());
    }

    IEnumerator OffAnimUIEnum()
    {
        origPanel.SetActive(false);
        animPanel.SetActive(true);

        float accTime = animTime;
        while(accTime > 0f)
        {
            animPanel.transform.localScale = origScale * accTime / animTime;
            accTime -= Time.deltaTime;
            yield return null;
        }

        animPanel.transform.localScale = Vector2.zero;
        animPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    // Method for Chat -> Vote Panel
    public void AnimPanelSizeChange(string panelName)
    {
        RectTransform newPanelTransform = GameManagerScript.instance.readyObj.transform.Find("WindowUI/MeetingWindow/Panel/" + panelName).GetComponent<RectTransform>();
        animPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(newPanelTransform.rect.width, newPanelTransform.rect.height);
        origScale = origPanel.transform.localScale;
    }
}
