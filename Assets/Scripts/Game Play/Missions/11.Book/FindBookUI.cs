using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindBookUI : MonoBehaviour
{
    [SerializeField] private Calculator calculator;
    [SerializeField] private GameObject findBookmissionWindowUI;
    [SerializeField] private GameObject bookPanelUI;
    [SerializeField] private GameObject[] bookCodes;
    [SerializeField] private GameObject[] bookCovers;
    [SerializeField] private GameObject[] miniBookCovers;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject findBookMemo;
    private bool[] isMemoOn = {false, false, false, false, false, false};
    private bool isFindMemoOn = false;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 11;
    [SerializeField] Text answerBookTitle;
    private int answerIndex;
    private AudioSource sfx;
    public AudioClip clearAudioClip;
    public AudioClip failAudioClip;
    public AudioClip dragAudioClip;
    private bool isClear = true;

    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
        randomBook();
    }

    private void randomBook()
    {
        int[] answerCodes = {0406, 0818, 1013, 1224, 6279, 3776};
        string[] bookTitles = {"걸리의 여행기", "오페라의 천사", "해리피터", "내일은 레벨업", "남궁세가 호의무사", "깁미더럭키짱"};

        int ran = Random.Range(0, 6);
        answerBookTitle.text = bookTitles[ran];
        calculator.missionAnswer = answerCodes[ran];
        answerIndex = ran;
    }

    private void Update() 
    {
        if(calculator.isMissionClear && isClear)
        {
            sfx.clip = dragAudioClip;
            sfx.Play();
            exitFindBookMission();
        }
        else if(calculator.isWrongAnswer)
        {
            sfx.clip = failAudioClip;
            sfx.Play();
            calculator.changeWrongAnswer();
        }

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("touch");
            if(isMemoOn[0])
            {
                offBookCodeMemo(0);
            }
            else if(isMemoOn[1])
            {
                offBookCodeMemo(1);
            }
            else if(isMemoOn[2])
            {
                offBookCodeMemo(2);
            }
            else if(isMemoOn[3])
            {
                offBookCodeMemo(3);
            }
            else if(isMemoOn[4])
            {
                offBookCodeMemo(4);
            }
            else if(isMemoOn[5])
            {
                offBookCodeMemo(5);
            }
            else if(isFindMemoOn)
            {
                findBookMemo.SetActive(false);
                isFindMemoOn = false;
            }
        }
    }

    public void showFindBookMemo()
    {
        findBookMemo.SetActive(true);
        isFindMemoOn = true;
    }

    public void onBookCodeMemo(int index)
    {
        bookCodes[index].SetActive(true);
        isMemoOn[index] = true;
    }

    public void offBookCodeMemo(int index)
    {
        bookCodes[index].SetActive(false);
        isMemoOn[index] = false;
    }

    public void exitFindBookMission()
    {
        isClear = false;
        miniBookCovers[answerIndex].GetComponent<RectTransform>().offsetMin = new Vector2(312, -30);
        miniBookCovers[answerIndex].GetComponent<RectTransform>().offsetMax = new Vector2(212, -30);
        
        StartCoroutine("Clear");
    }

    IEnumerator Clear()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        bookCovers[answerIndex].SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);
        bookCovers[answerIndex].SetActive(false);
        bookPanelUI.SetActive(false);
        findBookmissionWindowUI.SetActive(false);
        calculator.changeMissionClear();
        sfx.clip = clearAudioClip;
        sfx.Play();

        Destroy(gameObject);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
    }

    public void exitMission()
    {
        findBookmissionWindowUI.SetActive(false);
        Destroy(findBookmissionWindowUI);
    }

}
