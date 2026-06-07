using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calculator : MonoBehaviour
{
    [SerializeField] private Text calculatorResult;
    public int missionAnswer;
    public bool isMissionClear = false;
    public bool isWrongAnswer = false;
    private string previousVal;
    public AudioClip clickClip;
    AudioSource sfx;

    void Start()
    {
        previousVal = "0";
        calculatorResult.text = previousVal;
        previousVal = "";
        sfx = GameObject.Find("SFx").GetComponent<AudioSource>();
    }

    private void createButtonNum(int newNumber)
    {
        int num  = newNumber;
        string newNum = num.ToString();
        previousVal = previousVal + newNum;
        calculatorResult.text = previousVal;
    }

    public void getAnswer(int answer)
    {
        this.missionAnswer = answer;
    }

    public void button1()
    {
        createButtonNum(1);
        sfx.clip = clickClip;
        sfx.Play();
    }

    public void button2()
    {
        createButtonNum(2);
        sfx.clip = clickClip;
        sfx.Play();
    }

    public void button3()
    {
        createButtonNum(3);
        sfx.clip = clickClip;
        sfx.Play();
    }

    public void button4()
    {
        createButtonNum(4);
        sfx.clip = clickClip;
        sfx.Play();
    }

    public void button5()
    {
        createButtonNum(5);
        sfx.clip = clickClip;
        sfx.Play();
    }

    public void button6()
    {
        createButtonNum(6);
        sfx.clip = clickClip;
        sfx.Play();
    }

    public void button7()
    {
        createButtonNum(7);
        sfx.clip = clickClip;
        sfx.Play();
    }

    public void button8()
    {
        createButtonNum(8);
        sfx.clip = clickClip;
        sfx.Play();
    }

    public void button9()
    {
        createButtonNum(9);
        sfx.clip = clickClip;
        sfx.Play();
    }

    public void button0()
    {
        createButtonNum(0);
        sfx.clip = clickClip;
        sfx.Play();
    }

    public void buttonClear()
    {
        int num = 0;
        calculatorResult.text = num.ToString();
        previousVal = "";
        sfx.clip = clickClip;
        sfx.Play();
    }

    public void buttonOK()
    {
        int result = int.Parse(previousVal);
        if(missionAnswer == result)
        {
            isMissionClear = true;
        }
        else
        {
            isWrongAnswer = true;
        }
    }

    public void changeMissionClear()
    {
        isMissionClear = false;
    }

    public void changeWrongAnswer()
    {
        isWrongAnswer = false;
    }
}
