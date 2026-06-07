using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWindowUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject OpenWindowPanelUI;
    [SerializeField] private GameObject exitButton;
    private bool isClear = false;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 13;
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
        if(other.gameObject.tag == "collide")
        {
            isClear = true;
            clearMission();
        }
        // if(sfx.isPlaying) return;
        // sfx.clip = DragAudioClip;
        // sfx.Play();
    }

    private void Update() 
    {
        if(gameObject.GetComponent<ObjectDrag>().isDrag && !(sfx.isPlaying))
        {
            sfx.clip = DragAudioClip;
            sfx.Play();
        }

        if(isClear)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-175, 0);
        }
    }
    
    public void clearMission()
    {
        if(sfx.isPlaying) sfx.Stop();
        sfx.clip = clearAudioClip;
        sfx.Play();
        StartCoroutine("Clear");
    }

    IEnumerator Clear()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        OpenWindowPanelUI.SetActive(false);
        missionWindowUI.SetActive(false);
        exitButton.SetActive(false);
        Destroy(missionWindowUI);
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
