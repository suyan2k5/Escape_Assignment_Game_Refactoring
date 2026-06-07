using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScratchUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject Card;
    float starty, cleary;
    bool isClear;
    new Camera camera;
    Vector2 pos;

    AudioSource sfx;
    public AudioClip scratch;
    public AudioClip end;
    public AudioClip clear;
    AudioSource audioSource;
    private bool isEndPlayed;
    private bool isClearPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 15;

    private void Start()
    {
        isClear = false;
        starty = Card.transform.position.y;
        // Debug.Log(starty);
        cleary = Card.GetComponent<ObjectDrag>().axisObject.transform.position.y;
        // Debug.Log(cleary);
        camera = GameObject.Find("Main Move Camera").GetComponent<Camera>();
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        //this.audioSource = GetComponent<AudioSource>();
        isEndPlayed = false;
        isClearPlayed = false;

        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isClear)
        {
            if (Card.transform.position.y < cleary)
            {
                if (!isEndPlayed)
                {
                    isClear = true;
                    isEndPlayed = true;
                    StartCoroutine("backToStartPosition");
                    if (sfx.isPlaying) return;
                    sfx.clip = end;
                    sfx.Play();
                }
            }
            else if (Card.GetComponent<ObjectDrag>().isDrag)
            {
                if (sfx.isPlaying) return;
                sfx.clip = scratch;
                sfx.Play();
            }
        }
    }

    IEnumerator backToStartPosition()
    {
        Card.GetComponent<ObjectDrag>().enabled = false;
        pos = Card.transform.position;
        while (pos.y < starty)
        {
            Card.gameObject.transform.Translate(new Vector2(0, 0.2f));
            pos = Card.transform.position;
            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine("Clear");
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(0.5f);
        ExitCardScratch();
        Destroy(gameObject);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
    }


    public void ExitCardScratch()
    {
        if (isClear)
        {
            sfx.clip = clear;
            sfx.Play();
            isClearPlayed = true;
        }

        missionWindowUI.SetActive(false);
        //correctUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(gameObject);
    }
}
