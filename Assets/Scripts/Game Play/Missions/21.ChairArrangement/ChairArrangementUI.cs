using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairArrangementUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject[] Chair = new GameObject[3];
    new Camera camera;
    int rand;

    public AudioClip drrr;
    public AudioClip clear;
    AudioSource sfx;
    public bool isPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 21;
    private Transform pickedChair;

    private void Start()
    {
        camera = GameObject.Find("Main Move Camera").GetComponent<Camera>();
        rand = Random.Range(0, 3);
        float posx = 312f * (rand - 1);
        pickedChair = Chair[rand].transform;
        pickedChair.localPosition = new Vector2(posx, -200f);
        pickedChair.parent.Find("Axis").transform.localPosition = new Vector2(posx, 250f);
        pickedChair.GetComponent<ObjectDrag>().enabled = true;
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        //this.audioSource = GetComponent<AudioSource>();
        isPlayed = false;
        sfx = GameObject.Find("SFx").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(pickedChair.localPosition.y >= -50f)
        {
            pickedChair.GetComponent<ObjectDrag>().enabled = false;
            if (isPlayed == false)
            {
                //audioSource.clip = clear;
                //audioSource.Play();
                sfx.clip = clear;
                sfx.Play();
                isPlayed = true;
                StartCoroutine("Clear");
            }
        }
        if(pickedChair.GetComponent<ObjectDrag>().isDrag)
        {
            if(sfx.isPlaying) return;
            sfx.clip = drrr;
            sfx.Play();
        }
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(1f);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
        ExitChairArrangement();
    }

    public void ExitChairArrangement()
    {
        missionWindowUI.SetActive(false);
        //correctUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(gameObject);
    }
}
