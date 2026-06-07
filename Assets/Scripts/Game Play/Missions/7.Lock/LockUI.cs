using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject LockPanelUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject LockedImage;
    [SerializeField] private GameObject UnLockedImage;
    [SerializeField] private GameObject supportImage;
    [SerializeField] private GameObject KeyRotated;
    private Vector3 curPos;
    private bool isClear = false;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 7;
    private AudioSource sfx;
    public AudioClip dragAudioClip;
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
            
            StartCoroutine("InsertKey");
        }
    }

    private void Update() 
    {
        if(isClear)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-72, -210);
        }
    }

    IEnumerator InsertKey()
    {
        sfx.clip = dragAudioClip;
        sfx.Play();
        yield return new WaitForSeconds(0.5f);
        supportImage.SetActive(true);
        KeyRotated.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        KeyRotated.SetActive(false);
        supportImage.SetActive(false);
        clearMission();
    }
    
    public void clearMission()
    {
        StartCoroutine("Clear");
    }

    IEnumerator Clear()
    {
        sfx.clip = clearAudioClip;
        sfx.Play();
        yield return new WaitForSeconds(0.5f);
        LockedImage.SetActive(false);
        UnLockedImage.SetActive(true);
        yield return new WaitForSecondsRealtime(2.0f);
        Destroy(missionWindowUI);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
    }

    public void exitMission()
    {
        //CirecuitBreakerPanelUI.SetActive(false);
        missionWindowUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(missionWindowUI);
    }
}
