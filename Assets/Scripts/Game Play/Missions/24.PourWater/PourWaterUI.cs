using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PourWaterUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject pourer;
    [SerializeField] private GameObject Mask;
    [SerializeField] private Text Announce;
    float time = 2f;

    public AudioClip clear;
    AudioSource audioSource;
    public bool isPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 24;
    private void Start()
    {
        Announce.text = "";
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        //this.audioSource = GetComponent<AudioSource>();
        isPlayed = false;
    }

    private void Update()
    {
        if (pourer.transform.rotation.eulerAngles.z > 44f)
        {
            time -= Time.deltaTime;
            Mask.GetComponent<RectTransform>().localScale = new Vector2(1, 0);
            if (time > 1.5f)
            {
                Announce.text = "Pouring";
            }
            else if (time > 1f)
            {
                Announce.text = "Pouring.";
            }
            else if (time > 0.5f)
            {
                Announce.text = "Pouring..";
            }
            else if (time > 0f)
            {
                Announce.text = "Pouring...";
                if (isPlayed == false)
                {
                    //audioSource.clip = clear;
                    //audioSource.Play();
                    AudioSource sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
                    sfx.clip = clear;
                    sfx.Play();
                    isPlayed = true;
                }
            }
            else
            {
                int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
                playerUI.missionDoneUI(number);
                MissionManager.mission.setMissionSolved(number);
                ExitPourWater();
            }
        }
        else
        {
            Mask.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            time = 2f;
            Announce.text = "";
        }
    }

    public void ExitPourWater()
    {
        missionWindowUI.SetActive(false);
        //correctUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(gameObject);
    }
}
