using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject fireExtinguisherPanelUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject fireExtinguisher1;
    [SerializeField] private GameObject fireExtinguisher2;
    [SerializeField] private GameObject fireExtinguisher3;
    private Vector2 pos1 = new Vector2(-372, -60);
    private Vector2 pos2 = new Vector2(0, -60);
    private Vector2 pos3 = new Vector2(401, -60);
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 3;
    private AudioSource sfx;
    public AudioClip clearAudioClip;
    public AudioClip failAudioClip;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }
    private void Awake()
    {
        randomPosition();
    }

    private void randomPosition()
    {
        int ranNum = Random.Range(1, 4);
        switch(ranNum)
        {
            case 1:
                fireExtinguisher1.GetComponent<RectTransform>().anchoredPosition = pos1;
                fireExtinguisher2.GetComponent<RectTransform>().anchoredPosition = pos2;
                fireExtinguisher3.GetComponent<RectTransform>().anchoredPosition = pos3;
                break;
            case 2 :
                fireExtinguisher1.GetComponent<RectTransform>().anchoredPosition = pos3;
                fireExtinguisher2.GetComponent<RectTransform>().anchoredPosition = pos1;
                fireExtinguisher3.GetComponent<RectTransform>().anchoredPosition = pos2;
                break;
            case 3 : 
                fireExtinguisher1.GetComponent<RectTransform>().anchoredPosition = pos1;
                fireExtinguisher2.GetComponent<RectTransform>().anchoredPosition = pos3;
                fireExtinguisher3.GetComponent<RectTransform>().anchoredPosition = pos2;
                break;
        }
    }

    public void wrongFireExtinguisher()
    {
        sfx.clip = failAudioClip;
        sfx.Play();
    }

    public void missionClear()
    {
        StartCoroutine("Clear");
    }

    IEnumerator Clear()
    {
        sfx.clip = clearAudioClip;
        sfx.Play();
        yield return new WaitForSecondsRealtime(2.0f);
        Destroy(gameObject);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
    }
    
    public void exitMission()
    {
        missionWindowUI.SetActive(false);
        Destroy(missionWindowUI);
    }
}
