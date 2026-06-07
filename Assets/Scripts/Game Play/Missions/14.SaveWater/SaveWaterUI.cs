using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveWaterUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject handle;
    [SerializeField] private GameObject[] water = new GameObject[2];
    public float rotate;
    float time;

    public AudioClip waterfall;
    public AudioClip clear;
    public bool isPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 14;

    AudioSource sfx;

    private void Start()
    {
        water[0].SetActive(true);
        water[1].SetActive(false);
        time = 0f;
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        //this.audioSource = GetComponent<AudioSource>();
        isPlayed = false;

        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
        sfx.clip = waterfall;
        sfx.loop = true;
        sfx.Play();
    }

    private void Update()
    {
        rotate = handle.GetComponent<Handle>().rotate;
        // Debug.Log(rotate);
        if(!isPlayed) sfx.volume = (39f - rotate) / 39f;
        if (rotate < 5f)
        {
            water[0].SetActive(true);
            water[1].SetActive(false);
            // Debug.Log("1");
        }
        else if (rotate < 39f)
        {
            time = 0f;
            isPlayed = false;
            water[0].SetActive(false);
            water[1].SetActive(true);
            water[1].transform.localScale = new Vector2((39f - rotate) / 34f, 1f);
            // Debug.Log("2");
        }
        else
        {
            // Debug.Log("3");
            water[1].SetActive(false);
            time += Time.deltaTime;
            if (isPlayed == false)
            {
                sfx.clip = clear;
                sfx.volume = 1;
                sfx.loop = false;
                sfx.Play();
                isPlayed = true;
            }
            if (time >= 0.5)
            {
                StartCoroutine("Clear");
            }
        }
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(1f);
        ExitSaveWater();
        Destroy(gameObject);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
    }

    public void ExitSaveWater()
    {
        missionWindowUI.SetActive(false);
        //correctUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(gameObject);
    }
}
