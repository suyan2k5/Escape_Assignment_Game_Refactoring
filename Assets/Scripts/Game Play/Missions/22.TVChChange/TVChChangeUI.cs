using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVChChangeUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject UpButton;
    [SerializeField] private GameObject DownButton;
    [SerializeField] private Text GoalT;
    [SerializeField] private Text CurT1;
    [SerializeField] private Text CurT2;
    [SerializeField] private int underBound;    // 포함 O
    [SerializeField] private int upperBound;    // 포함 X
    [SerializeField] private Sprite clearSpr;
    private int goal;
    private int cur;

    public AudioClip btn;
    public AudioClip clear;
    AudioSource sfx;
    public bool isPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 22;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }

    private void Awake()
    {
        goal = Random.Range(underBound, upperBound) * 2 + 1;
        do
        {
            cur = Random.Range(underBound, upperBound) * 2 + 1;
        } while (goal == cur);
        underBound = underBound * 2 + 1;
        upperBound = (upperBound - 1) * 2 + 1;
        GoalT.text = "Goal: " + goal;
        CurT1.text = "Ch.  " + cur;
        CurT2.text = "Ch.  " + cur;
        
        //this.audioSource = GetComponent<AudioSource>();
        isPlayed = false;
    }

    private void Update()
    {
        GoalT.text = "Goal: " + goal;
        CurT1.text = "Ch.  " + cur;
        CurT2.text = "Ch.  " + cur;
        if (goal == cur)
        {
            if (isPlayed == false)
            {
                //audioSource.clip = clear;
                //audioSource.Play();
                sfx.clip = clear;
                sfx.Play();
                isPlayed = true;
            }
            StartCoroutine("Clear");
        }
    }

    IEnumerator Clear()
    {
        missionWindowUI.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = clearSpr;
        yield return new WaitForSeconds(1f);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
        ExitTVChChange();
    }

    public void UpButtonClick()
    {
        sfx.clip = btn;
        sfx.Play();
        cur += 2;
        if(cur > upperBound)
        {
            cur = underBound;
        }
    }

    public void DownButtonClick()
    {
        sfx.clip = btn;
        sfx.Play();
        cur -= 2;
        if (cur < underBound)
        {
            cur = upperBound;
        }
    }

    public void ExitTVChChange()
    {
        missionWindowUI.SetActive(false);
        //correctUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(gameObject);
    }
}
