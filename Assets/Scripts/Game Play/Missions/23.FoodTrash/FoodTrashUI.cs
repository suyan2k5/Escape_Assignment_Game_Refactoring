using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTrashUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject[] Food = new GameObject[5];
    public int numFood;

    public AudioClip clear;
    AudioSource audioSource;
    public bool isPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 23;
    private void Start() 
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
    }

    private void Awake()
    {
        //this.audioSource = GetComponent<AudioSource>();
        isPlayed = false;
    }

    private void Update()
    {
        if(numFood <= 0)
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
        yield return new WaitForSeconds(1f);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
        ExitFoodTrash();
    }

    public void ExitFoodTrash()
    {
        missionWindowUI.SetActive(false);
        //correctUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(gameObject);
    }
}
