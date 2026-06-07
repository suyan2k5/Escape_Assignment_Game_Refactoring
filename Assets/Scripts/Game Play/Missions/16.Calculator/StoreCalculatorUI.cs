using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCalculatorUI : MonoBehaviour
{
    [SerializeField] Calculator16 calculator;
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject wrongUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private Text num1T;
    [SerializeField] private Text num2T;
    [SerializeField] private Text operatorT;
    private int op;
    private int num1, num2;

    public AudioClip babam; // f
    public AudioClip clear; // a
    AudioSource audioSource;
    public bool isPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 16;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
    }

    private void Awake()
    {
        op = Random.Range(1, 3);
        if(op == 1)
        {
            operatorT.text = "+";
            num1 = Random.Range(1, 21);
            num1T.text = "" + num1;
            num2 = Random.Range(1, 21);
            num2T.text = "" + num2;
            calculator.missionAnswer = num1 + num2;
        }
        else if(op == 2)
        {
            operatorT.text = "X";
            num1 = Random.Range(1, 11);
            num1T.text = "" + num1;
            num2 = Random.Range(10, 51);
            num2T.text = "" + num2;
            calculator.missionAnswer = num1 * num2;
        }

        //this.audioSource = GetComponent<AudioSource>();
        isPlayed = false;
    }

    private void Update()
    {
        if (calculator.isMissionClear)
        {
            if (isPlayed == false)
            {
                //audioSource.clip = clear;
                //audioSource.Play();
                AudioSource sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
                sfx.clip = clear;
                sfx.Play();
                isPlayed = true;
                
                StartCoroutine("Clear");
            }
        }
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(0.5f);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
        exitCorrectUI();
    }

    public void exitCorrectUI()
    {
        //correctUI.SetActive(false);
        missionWindowUI.SetActive(false);
        calculator.changeMissionClear();
        //exitButton.SetActive(false);
        Destroy(gameObject);
        
    }

    public void exitWrongUI()
    {
        wrongUI.SetActive(false);
        calculator.changeWrongAnswer();
    }
}
