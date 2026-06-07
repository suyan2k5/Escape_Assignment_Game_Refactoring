using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculatorTestUI : MonoBehaviour
{
    [SerializeField] Calculator calculator;
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject coverTestUI;
    [SerializeField] private GameObject missionQuestionUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private Text QuestionText;
    public AudioClip nextPageClip;
    public AudioClip failAudioClip;
    public AudioClip clearAudioClip;
    private int num1;
    private int num2;
    private int answer = 57;
    private bool isPlayed = true;
    private PlayerUISettingScript playerUI;
    private AudioSource sfx;
    [SerializeField] int missionNumber = 0;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }
    private void Awake()
    {
        randomQuesion();
    }

    private void Update() 
    {
        if(calculator.isMissionClear)
        {
            if(isPlayed)
            {
                playClearClip();
            }
            missionClear();
        }
        else if(calculator.isWrongAnswer)
        {
            calculator.buttonClear();
            calculator.changeWrongAnswer();
            sfx.clip = failAudioClip;
            sfx.Play();
            print("wrong");
        }
    }

    private void randomQuesion()
    {
        num1 = Random.Range(1, 100);
        num2 = Random.Range(2, 101);
        QuestionText.text = "Q. " + num1.ToString() + " + " + num2.ToString() + " = ?";
        answer = num1 + num2;
        calculator.getAnswer(answer);
    }

    public void onTestUI()
    {
        coverTestUI.SetActive(false);
        sfx.clip = nextPageClip;
        sfx.Play();
        missionQuestionUI.SetActive(true);
    }

    public void exitTestQuestion()
    {   
        coverTestUI.SetActive(true);
        missionQuestionUI.SetActive(true);
        missionWindowUI.SetActive(false);
        Destroy(gameObject);
    }

    IEnumerator Clear()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        QuestionText.text = "Q. " + num1.ToString() + " + " + num2.ToString() + " = " + answer;
        calculator.changeMissionClear();
        Destroy(gameObject);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
    }

    public void missionClear()
    {
        StartCoroutine("Clear");
    }

    private void playClearClip()
    {
        sfx.clip = clearAudioClip;
        sfx.Play();
        isPlayed = false;
    }
}
