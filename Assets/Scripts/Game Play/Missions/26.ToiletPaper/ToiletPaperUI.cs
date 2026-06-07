using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaperUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject toiletPaperPanelUI;
    [SerializeField] private GameObject exitButton;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 26;

    public AudioClip clear;
    AudioSource sfx;

    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").GetComponent<AudioSource>();
    }

    public void clearMission()
    {
        StartCoroutine("Clear");
    }

    IEnumerator Clear()
    {
        sfx.clip = clear;
        sfx.Play();
        yield return new WaitForSecondsRealtime(2.0f);
        Destroy(gameObject);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
    }

    public void exitMission()
    {
        Destroy(gameObject);
    }
}
