using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutIngredientUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject[] Ing = new GameObject[4];
    public int numIng;
    bool isAllPut;

    public AudioClip boil;
    public AudioClip clear;
    AudioSource audioSource;
    public bool isPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 18;
    GameObject boilMusic;

    private void Start()
    {
        isAllPut = false;
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        this.audioSource = GetComponent<AudioSource>();
        isPlayed = false;
        boilMusic = new GameObject("BoilMusic");
        AudioSource boilSrc = boilMusic.AddComponent<AudioSource>();
        boilSrc.loop = true;
        boilSrc.clip = boil;
        boilSrc.Play();
    }

    private void Update()
    {
        isAllPut = true;
        for(int i = 0; i < numIng; i += 1)
        {
            if(Ing[i].GetComponent<Ingredient>().isPut == false)
            {
                isAllPut = false;
            }
        }
        if(isAllPut)
        {
            if (isPlayed == false)
            {
                //audioSource.clip = clear;
                //audioSource.Play();
                AudioSource sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
                sfx.clip = clear;
                sfx.Play();
                isPlayed = true;
            }
            StartCoroutine("Clear");
        }
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(1f);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
        ExitPutIngredient();
    }

    public void ExitPutIngredient()
    {
        missionWindowUI.SetActive(false);
        //correctUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(boilMusic);
        Destroy(gameObject);
    }
}
