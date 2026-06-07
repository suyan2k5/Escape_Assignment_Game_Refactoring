using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject BoardPanelUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private Image chalkImage;
    private int collideNum = 0;
    private Color imgColor;
    private float imgAlpha = 1.0f;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 5;
    private AudioSource sfx;
    public AudioClip clearAudioClip;
    public AudioClip DragAudioClip;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }

    private void Update() 
    {
        if(gameObject.GetComponent<ObjectDrag>().isDrag && !(sfx.isPlaying))
        {
            sfx.clip = DragAudioClip;
            sfx.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "collide")
        {
            imgColor = chalkImage.color;
            if(collideNum != 10)
            {
                imgAlpha -= 0.1f;
                imgColor.a = imgAlpha;
                chalkImage.color = imgColor;
                collideNum ++;
                
            }
            else if(collideNum == 10)
            {
                clearMission();
            }
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
