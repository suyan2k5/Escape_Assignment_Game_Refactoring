using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerPasswordUI : MonoBehaviour
{
    [SerializeField] private Calculator calculator;
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject computerUI;
    [SerializeField] private GameObject passwordUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private Text announceText;
    [SerializeField] private Text passwordText;
    private int password;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 2;
    private AudioSource sfx;
    public AudioClip failAudioClip;
    public AudioClip clearAudioClip;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }
 
    private void Awake() 
    {
        randomPassword();
    }
    private void Update() 
    {
        if(calculator.isMissionClear)
        {
            announceText.text = "환영합니다";
            clearMission();
        }
        else if(calculator.isWrongAnswer)
        {
            announceText.text = "비밀번호가 틀렸습니다";
            failMission();
        }
    }

    private void randomPassword()
    {
        password = Random.Range(10001, 99999);
        passwordText.text = "Password : " + password.ToString();
        calculator.getAnswer(password);
    }

    public void onPasswordMemoUI()
    {
        passwordUI.SetActive(true);
    }
    
    public void offPasswordMemoUI()
    {
        passwordUI.SetActive(false);
    }

    public void exitComputerMission()
    {
        missionWindowUI.SetActive(false);
        Destroy(gameObject);
    }

    public void clearMission()
    {
        StartCoroutine("RightAnswer");
    }

    IEnumerator RightAnswer()
    {
        sfx.clip = clearAudioClip;
        sfx.Play();
        yield return new WaitForSecondsRealtime(1.0f);
        calculator.changeMissionClear();
        Destroy(gameObject);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
    }

    public void failMission()
    {
        StartCoroutine("WrongAnswer");
    }
    IEnumerator WrongAnswer()
    {
        sfx.clip = failAudioClip;
        sfx.Play();
        yield return new WaitForSecondsRealtime(1.0f);
        announceText.text = "비밀번호를 입력하세요";
        calculator.buttonClear();
        calculator.changeWrongAnswer();
    }

}
