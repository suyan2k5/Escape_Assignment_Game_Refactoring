using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerPower : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject computerPanelUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject computerPowerOn1;
    [SerializeField] private GameObject computerPowerOn2;
    SpriteRenderer spriteRenderer;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 1;
    private AudioSource sfx;
    public AudioClip clearAudioClip;
    public AudioClip pcOnClip;

    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }

    public void computerPowerOn()
    {
        missionClear();
    }

    IEnumerator Clear()
    {
        sfx.clip = pcOnClip;
        sfx.Play();
        yield return new WaitForSecondsRealtime(0.5f);
        computerPowerOn1.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        computerPowerOn2.SetActive(true);
        yield return new WaitForSecondsRealtime(3.0f);
        Destroy(gameObject);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
        sfx.clip = clearAudioClip;
        sfx.Play();
    }

    public void missionClear()
    {
        StartCoroutine("Clear");
    }

    public void exitMission()
    {
        missionWindowUI.SetActive(false);
        Destroy(gameObject);
    }
}
