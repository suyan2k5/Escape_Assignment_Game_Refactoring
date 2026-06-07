using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuServeUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject[] menu = new GameObject[5];
    [SerializeField] private GameObject[] foodTray = new GameObject[5];
    public bool[] isPut = new bool[5];
    bool isClear;

    public AudioClip clear;
    AudioSource audioSource;
    public bool isPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 20;
    private void Start()
    {
        isClear = false;
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        //this.audioSource = GetComponent<AudioSource>();
        isPlayed = false;
    }

    void Update()
    {
        for (int i = 0; i < 5; i += 1)
        {
            if(menu[i].GetComponent<ObjectDrag>().isDrag)
            {
                foodTray[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                foodTray[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        isClear = true;
        for(int i = 0; i < 5; i += 1)
        {
            if(!isPut[i])
            {
                isClear = false;
            }
        }
        if(isClear)
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
        ExitMenuServe();
    }

    public void ExitMenuServe()
    {
        missionWindowUI.SetActive(false);
        //correctUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(gameObject);
    }
}
