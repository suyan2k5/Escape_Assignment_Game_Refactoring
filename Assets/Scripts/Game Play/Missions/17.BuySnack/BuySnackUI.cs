using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySnackUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private Text goalT;
    public int underBound;
    public int upperBound;
    public int cur;
    private int goal;
    public bool isDisappearing;

    public AudioClip fail;
    public AudioClip clear;
    AudioSource audioSource;
    public bool isPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 17;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
    }

    private void Awake()
    {
        goal = Random.Range(underBound, upperBound);
        print(goal);
        goalT.text = "Goal: " + (char)(goal - 1 + (int)'A');
        cur = 0;
        isDisappearing = false;

        this.audioSource = GetComponent<AudioSource>();
        isPlayed = true;
    }

    private void Update()
    {
        AudioSource sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
        if(cur == goal)
        {
            /*********************
            성공 효과음
            *********************/
            if(isPlayed == false)
            {
                //audioSource.clip = clear;
                //audioSource.Play();
                sfx.clip = clear;
                sfx.Play();
                isPlayed = true;

                //correctUI.SetActive(true);
                StartCoroutine("Clear");
            }
        }
        else
        {
            /*********************
            실패 효과음
            *********************/
            if(isPlayed == false)
            {
                //audioSource.clip = fail;
                //audioSource.Play();
                // sfx.clip = fail;
                // sfx.Play();
                // isPlayed = true;
            }
        }
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(2f);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
        ExitBuySnack();
    }

    public void ExitBuySnack()
    {
        missionWindowUI.SetActive(false);
        //correctUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(gameObject);
        
    }
}
