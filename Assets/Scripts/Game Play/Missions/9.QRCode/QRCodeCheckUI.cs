using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QRCodeCheckUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject QRCodeCheckPanelUI;
    [SerializeField] private GameObject exitButton;
    private bool isClear = false;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 9;
    private AudioSource sfx;
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
            sfx.clip = clearAudioClip;
            sfx.Play();
            clearMission();
        }
    }

    private void Update() 
    {
        if(isClear)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-133, 50);
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
    }

    public void exitMission()
    {
        missionWindowUI.SetActive(false);
        Destroy(missionWindowUI);
    }
}
