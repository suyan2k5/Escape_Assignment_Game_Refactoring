using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInput : MonoBehaviour
{
    [SerializeField] private MenuServeUI UIScript;
    [SerializeField] private GameObject GoalInput;
    public int FoodNum;
    bool isOn, isPut;

    public AudioClip put;
    AudioSource sfx;

    private void Start()
    {
        isOn = false;
        isPut = false;
        sfx = GameObject.Find("SFx").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == GoalInput.name)
        {
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == GoalInput.name)
        {
            isOn = false;
        }
    }

    private void OnMouseUp()
    {
        if (isOn && !isPut)
        {
            isPut = true;
            gameObject.transform.position = GoalInput.transform.GetChild(1).transform.position;
            UIScript.isPut[FoodNum] = isPut;
            sfx.clip = put;
            sfx.Play();
        }
        else
        {
            gameObject.transform.position = UIScript.gameObject.transform.GetChild(0).transform.GetChild(FoodNum).transform.position;
        }
    }
}
