using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calculator16 : MonoBehaviour
{
    [SerializeField] private Text calculatorResult;
    public int missionAnswer;
    public bool isMissionClear = false;
    public bool isWrongAnswer = false;
    private string previousVal = string.Empty;
    public AudioClip touchNum; // e
    public AudioClip touchC; // c
    AudioSource sfx;

    void Start()
    {
        previousVal = "0";
        calculatorResult.text = previousVal;
        previousVal = "";
        sfx = GameObject.Find("SFx").GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(previousVal.Length != 0)
        {
            int result = int.Parse(previousVal);
            if (missionAnswer == result)
            {
                isMissionClear = true;
            }
            else
            {
                //isWrongAnswer = true;
            }
        }
    }

    private void createButtonNum(int newNumber)
    {
        int num = newNumber;
        string newNum = num.ToString();
        previousVal = previousVal + newNum;
        calculatorResult.text = previousVal;
    }

    public void button1()
    {
        if(previousVal.Length >= 9) return;
        createButtonNum(1);
        sfx.clip = touchNum;
        sfx.Play();
    }

    public void button2()
    {
        if(previousVal.Length >= 9) return;
        createButtonNum(2);
        sfx.clip = touchNum;
        sfx.Play();
    }

    public void button3()
    {
        if(previousVal.Length >= 9) return;
        createButtonNum(3);
        sfx.clip = touchNum;
        sfx.Play();
    }

    public void button4()
    {
        if(previousVal.Length >= 9) return;
        createButtonNum(4);
        sfx.clip = touchNum;
        sfx.Play();
    }

    public void button5()
    {
        if(previousVal.Length >= 9) return;
        createButtonNum(5);
        sfx.clip = touchNum;
        sfx.Play();
    }

    public void button6()
    {
        if(previousVal.Length >= 9) return;
        createButtonNum(6);
        sfx.clip = touchNum;
        sfx.Play();
    }

    public void button7()
    {
        if(previousVal.Length >= 9) return;
        createButtonNum(7);
        sfx.clip = touchNum;
        sfx.Play();
    }

    public void button8()
    {
        if(previousVal.Length >= 9) return;
        createButtonNum(8);
        sfx.clip = touchNum;
        sfx.Play();
    }

    public void button9()
    {
        if(previousVal.Length >= 9) return;
        createButtonNum(9);
        sfx.clip = touchNum;
        sfx.Play();
    }

    public void button0()
    {
        if(previousVal.Length != 0)
        {
            if(previousVal.Length >= 9) return;
            createButtonNum(0);
            sfx.clip = touchNum;
            sfx.Play();
        }
    }

    public void buttonAllClear()
    {
        int num = 0;
        calculatorResult.text = num.ToString();
        previousVal = "";
        sfx.clip = touchC;
        sfx.Play();
    }

    public void buttonClearEntry()
    {
        if(previousVal != "")
        {
            if(previousVal.Length == 1)
            {
                buttonAllClear();
            }
            else
            {
                previousVal = previousVal.Substring(0, previousVal.Length - 1);
                calculatorResult.text = previousVal;
            }
            sfx.clip = touchC;
            sfx.Play();
        }
    }

    public void changeMissionClear()
    {
        isMissionClear = true;
    }

    public void changeWrongAnswer()
    {
        isWrongAnswer = false;
    }
}
