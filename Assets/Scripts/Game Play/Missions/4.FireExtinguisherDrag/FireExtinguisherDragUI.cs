using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherDragUI : MonoBehaviour
{
    [SerializeField] private GameObject scale;
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject fireExtinguisherDragPanelUI;
    [SerializeField] private GameObject exitButton;
    private int collideNum = 0;
    private bool isClear = false;
    private Vector3 curPos;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 4;
    private AudioSource sfx;
    public AudioClip clearAudioClip;
    public AudioClip DragAudioClip;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "collide")
        {
            if(collideNum != 10)
            {
                scale.transform.Rotate(new Vector3(0, 0, 12f));
                collideNum ++;
            }
            else if(collideNum == 10)
            {
                curPos = gameObject.transform.position;
                isClear = true;
                Destroy(gameObject.GetComponent<ObjectDrag>());
                clearMission();
            }
        }
        
    }
    private void Update() 
    {
        if(isClear)
        {
            gameObject.transform.position = curPos;
            return;
        }
        
        if(gameObject.GetComponent<ObjectDrag>().isDrag && !(sfx.isPlaying))
        {
            sfx.clip = DragAudioClip;
            sfx.Play();
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
        Destroy(gameObject.transform.parent.gameObject.transform.parent.gameObject);
    }
} 
