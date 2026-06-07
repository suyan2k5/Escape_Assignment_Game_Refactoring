using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitBreakerUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject CirecuitBreakerPanelUI;
    [SerializeField] private GameObject exitButton;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 6;
    private AudioSource sfx;
    public AudioClip DragAudioClip;
    public AudioClip clearAudioClip;

    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        sfx.clip = DragAudioClip;
        sfx.Play();
        if(other.gameObject.tag == "collide")
        {
            sfx.clip = DragAudioClip;
            sfx.Play();
            Destroy(gameObject.GetComponent<ObjectDrag>());
            clearMission();
        }
    }
    

    public void clearMission()
    {
        StartCoroutine("Clear");
    }

    IEnumerator Clear()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        Destroy(missionWindowUI);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
        sfx.clip = clearAudioClip;
        sfx.Play();
    }

    public void exitMission()
    {
        //CirecuitBreakerPanelUI.SetActive(false);
        missionWindowUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(missionWindowUI);
    }
}
