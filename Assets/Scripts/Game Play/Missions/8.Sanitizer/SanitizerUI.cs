using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanitizerUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject sanitizerPanelUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject waterImage;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 8;
    private bool isCollide = false;
    private Vector2 waterPos;
    private AudioSource sfx;
    public AudioClip clearAudioClip;
    public AudioClip fluidFallAudioClip;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }
    private void OnCollideEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "collide")
        {
            isCollide = true;
            waterPos = waterImage.GetComponent<RectTransform>().anchoredPosition;
        }
    }
    
    private void Update() 
    {
        if(isCollide)
        {
            waterImage.GetComponent<RectTransform>().anchoredPosition = waterPos;
        }    
    }

    public void getSanitizer()
    {
        StartCoroutine("getWaterSanitizer");
    }
    IEnumerator getWaterSanitizer()
    {
        sfx.clip = fluidFallAudioClip;
        sfx.Play();
        waterImage.SetActive(true);
        waterImage.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        yield return new WaitForSecondsRealtime(2.0f);
        sfx.clip = clearAudioClip;
        sfx.Play();
        exitSanitizerMission();
    }

    public void exitSanitizerMission()
    {
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
