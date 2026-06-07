using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCutUI : MonoBehaviour
{
    [SerializeField] private GameObject missionWindowUI;
    [SerializeField] private GameObject correctUI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject[] branch = new GameObject[4];
    public int numScratchedBranch;
    public float waitingT = 1.5f;
    // ArrayList rand = new ArrayList();
    // bool isSame;
    int rand;
    public AudioClip cut;
    public AudioClip clear;
    AudioSource sfx;
    public bool isPlayed;
    private PlayerUISettingScript playerUI;
    [SerializeField] int missionNumber = 25;

    private void Start()
    {

        rand = Random.Range(0, 4);
        for(int i = 0; i < branch.Length; i++)
        {
            if(i == rand) continue;

            branch[i].transform.GetChild(0).gameObject.SetActive(true);
            branch[i].GetComponent<Branch>().isChoose = true;
        }

        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
        //this.audioSource = GetComponent<AudioSource>();
        isPlayed = false;

        sfx = GameObject.Find("SFx").gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(numScratchedBranch <= 0)
        {
            waitingT -= Time.deltaTime;
            if (isPlayed == false)
            {
                //audioSource.clip = clear;
                //audioSource.Play();
                sfx.clip = clear;
                sfx.Play();
                isPlayed = true;
            }
            if (waitingT < 0)
            {
                int number = MissionManager.mission.myMissionNum.IndexOf(missionNumber);
                playerUI.missionDoneUI(number);
                MissionManager.mission.setMissionSolved(number);
                ExitTreeCut();
            }
        }
    }

    public void CutBranch(GameObject branch)
    {
        if (branch.GetComponent<Branch>().isChoose == true && branch.GetComponent<Branch>().isCut == false)
        {
            numScratchedBranch -= 1;
            branch.GetComponent<Branch>().isCut = true;
            branch.AddComponent<Rigidbody2D>();
            sfx.clip = cut;
            sfx.Play();
        }
    }

    public void ExitTreeCut()
    {
        missionWindowUI.SetActive(false);
        //correctUI.SetActive(false);
        //exitButton.SetActive(false);
        Destroy(gameObject);
    }
}
