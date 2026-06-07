using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StandLightUI : MonoBehaviour
{
    [SerializeField] private GameObject StandLightMissionWindowUI;
    [SerializeField] private GameObject StandLightPanelUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject lightOff;
    [SerializeField] private GameObject whiteImage;
    [SerializeField] private float setClickTime = 3.0f;
    private float clickTime;
    private bool isClick = false;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 12;
    private AudioSource sfx;
    public AudioClip clearAudioClip;
    public AudioClip switchAudioClip;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }

    private void Update() 
    {
        if(isClick)
        {
            clickTime += Time.deltaTime;
        }
        else
        {
            clickTime = 0;
        }
    }

    public void ButtonDown()
    {
        isClick = true;
        sfx.clip = switchAudioClip;
        sfx.Play();
    }

    public void ButtonUp()
    {
        isClick = false;
        if(clickTime >= setClickTime)
        {
            whiteImage.SetActive(true);
            lightOff.SetActive(true);
            sfx.clip = clearAudioClip;
            sfx.Play();
            StartCoroutine("Clear");
        }
    }
    
    public void exitStandLightMission()
    {
        StartCoroutine("Clear");
    }

    IEnumerator Clear()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        whiteImage.SetActive(false);
        lightOff.SetActive(false);
        StandLightPanelUI.SetActive(false);
        StandLightMissionWindowUI.SetActive(false);
        exitButton.SetActive(false);
        Destroy(gameObject);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
    }

    public void exitMission()
    {
        StandLightMissionWindowUI.SetActive(false);
        Destroy(StandLightMissionWindowUI);
    }
}
