using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecieveOrderUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject SendPannel;
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private Slider progressBar;
    [SerializeField] private GameObject[] DotPosition = new GameObject[5];
    [SerializeField] private GameObject File;
    [SerializeField] private GameObject ClearImage;
    public float timer;
    bool isStart, isFileSend;
    float time;
    public AudioClip click;
    public AudioClip clear;
    AudioSource audioSource;
    public bool isPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 19;

    private void Start()
    {
        startButton.SetActive(true);
        progressBar.gameObject.SetActive(false);
        isStart = false;
        isFileSend = false;
        for(int i = 0; i < 5; i += 1)
        {
            DotPosition[i].SetActive(false);
        }
        File.SetActive(false);
        progressBar.maxValue = timer;
        time = 0;
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        this.audioSource = GetComponent<AudioSource>();
        isPlayed = false;
    }

    private void Update()
    {
        if(isStart)
        {
            // 파일 전송 모션 실행
            if(isFileSend == false)
            {
                isFileSend = true;
                SendPannel.SetActive(true);
                for (int i = 0; i < 5; i += 1)
                {
                    DotPosition[i].SetActive(true);
                }
                File.SetActive(true);
                StartCoroutine("FileSend");
            }

            // 진행 바 표시
            progressBar.gameObject.SetActive(true);
            time += Time.deltaTime;
            if(time <= timer)
            {
                progressBar.value = time;
            }
            else
            {
                isStart = false;
                for (int i = 0; i < 5; i += 1)
                {
                    DotPosition[i].SetActive(false);
                }
                File.SetActive(false);
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
    }

    IEnumerator Clear()
    {
        ClearImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
        playerUI.missionDoneUI(number);
        MissionManager.mission.setMissionSolved(number);
        ExitRecieveOrder();
    }

    IEnumerator FileSend()
    {
        int k = 0;
        while(time <= timer)
        {
            File.transform.position = DotPosition[k % 5].transform.position;
            k += 1;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void clickStart()
    {
        audioSource.clip = click;
        audioSource.Play();
        isStart = true;
        StartPanel.SetActive(false);
    }

    public void ExitRecieveOrder()
    {
        missionWindowUI.SetActive(false);
        //correctUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(gameObject);
    }
}
