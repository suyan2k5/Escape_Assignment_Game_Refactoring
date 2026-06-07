using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThermometerUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject thermometerPanelUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject RedLight;
    [SerializeField] private GameObject RedEffect;
    [SerializeField] private Text countDown;
    [SerializeField] private Text bodyTemperature;
    [SerializeField] private float setCount = 3.0f;
    [SerializeField] private float temperature = 36.4f;
    private bool isCountDown = false;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 10;
    public AudioClip clearAudioClip;
    public AudioClip measureAudioClip;
    private AudioSource sfx;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }

    private void Update() 
    {
        if(isCountDown)
        {
            RedLight.SetActive(true);
            if(setCount > 0)
            {
                setCount -= Time.deltaTime;
            }
            else if(setCount <= 0)
            {
                isCountDown = false;
            }
            countDown.text = Mathf.Ceil(setCount).ToString();
            StartCoroutine(WaitSecond());

            if(!isCountDown)
            {
                RedLight.SetActive(false);
                if(countDown.text == "0")
                {
                    temperature = Random.Range(35.9f, 37.5f);
                    bodyTemperature.text = temperature.ToString("F1");
                    RedLight.SetActive(true);
                    RedEffect.SetActive(true);
                }
                sfx.clip = clearAudioClip;
                sfx.Play();
                clearMission();
            }
        }
    }

    public void clearMission()
    {
        StartCoroutine("Clear");
    }

    IEnumerator Clear()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        thermometerPanelUI.SetActive(false);
        missionWindowUI.SetActive(false);
        exitButton.SetActive(false);
        Destroy(gameObject);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
    }

    public void checkBodyTemperature()
    {        
        isCountDown = true;
        sfx.clip = measureAudioClip;
        sfx.Play();
    }

    IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(1.0f);
    }

    public void exitMission()
    {
        missionWindowUI.SetActive(false);
        Destroy(missionWindowUI);
    }
}
