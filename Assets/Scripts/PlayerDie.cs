using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PlayerDie : MonoBehaviourPunCallbacks
{
    private GameObject[] players;
    private GameObject myPlayer;
    private CharacterInfoSetting myPlayerInfo;
    public SpriteRenderer[] customizes;
    public Sprite[] deadImages;
    public GameObject deadPrefab;
    private SettingPlayerManager settingPlayerManager;
    private Image[] profileImages;
    private Text[] profileNicknames;
    private PhotonView PV;
    private PlayerUISettingScript playerUISettingScript;

    private void Awake() 
    {
        MyAwake();
    }

    public void MyAwake() 
    {
        settingPlayerManager = GameManagerScript.instance.settingPlayerManager;
        PV = gameObject.GetComponent<PhotonView>();
        players = GameManagerScript.instance.getPlayers();
    }

    private void SetMyPlayer()
    {
        myPlayer = GameManagerScript.instance.getMyPlayer();
        myPlayerInfo = GameManagerScript.instance.getMyPlayerInfo();
    }

    public void StuNGraPlayerDie()
    {
        playerUISettingScript = GameManagerScript.instance.playerUISettingScript;
        SetMyPlayer();
        int profileIndex = -1;
        profileIndex = settingPlayerManager.myProfileIndex;
        PV.RPC("spreadDeadInfo", RpcTarget.AllBuffered, myPlayerInfo.nickName, profileIndex);
        PlayerMulti playerMulti = myPlayer.GetComponent<PlayerMulti>();
        playerMulti.NickNameText.color = Color.red;
        myPlayer.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
        playerUISettingScript.OnSabotageButton();

        if (myPlayer.GetComponent<PhotonView>().IsMine)
        {
            MissionProgressChecker missionProgressChecker = GameManagerScript.instance.readyObj.transform.Find("MainMobileUI/MissionProgressBar").GetComponent<MissionProgressChecker>();
            CharacterInfoSetting characterInfoSetting = myPlayer.GetComponent<CharacterInfoSetting>();
            missionProgressChecker.BarInfoChangeRunner(1, characterInfoSetting.assignedMission.Count - characterInfoSetting.missionRemained);
            missionProgressChecker.BarInfoChangeRunner(0, characterInfoSetting.assignedMission.Count);
        }
    }

    
    [PunRPC]
    void spreadDeadInfo(String deadNickName, int profileIndex)
    {
        SetMyPlayer();
        if(myPlayer.CompareTag("Dead")) return;

        GameObject deadPlayer = null;
        players = GameManagerScript.instance.getPlayers();

        foreach(GameObject p in players)
        {
            if(p.GetComponent<CharacterInfoSetting>().nickName == deadNickName)
            {
                deadPlayer = p;
            }
        }
        if(deadPlayer != null)
        {
            Color halfOpacity = new Color(255, 255, 255, 0.5f);
            if(deadPlayer != null) deadPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>().color = halfOpacity;
            for (int i = 0; i < customizes.Length; i++)
            {
                if(customizes[i] != null) customizes[i].color = halfOpacity;
            }

            CharacterInfoSetting deadInfo = deadPlayer.GetComponent<CharacterInfoSetting>();

            // 죽은 유저 정보 수정 및 사용
            deadInfo.isAlive = false;
            // scene name
            deadPrefab.GetComponent<CharacterInfoSetting>().currentSceneName = deadInfo.currentSceneName;
            // dead img
            deadPrefab.transform.Find("Image").GetComponent<SpriteRenderer>().sprite = deadImages[deadInfo.animalInd];
            // img flip
            if (deadInfo.transform.Find("Image").GetComponent<SpriteRenderer>().flipX) deadPrefab.transform.Find("Image").GetComponent<SpriteRenderer>().flipX = true;
            // nickname
            Text deadtext = deadPrefab.transform.Find("Canvas/NicknameT").GetComponent<Text>();
            deadtext.text = deadNickName;
            deadtext.color = Color.red;
            // object instantiate
            if (myPlayer.GetComponent<PhotonView>().IsMine) /*dead = */PhotonNetwork.Instantiate(deadPrefab.name, deadPlayer.transform.position, Quaternion.identity);
            profileImages = settingPlayerManager.profileImages;
            profileNicknames = settingPlayerManager.profileNicknames;

            profileImages[profileIndex].color = Color.gray;
            profileNicknames[profileIndex].color = Color.red;

            GameManagerScript.instance.SetPlayerList(deadInfo.role, -1);
        }
        else
        {
            print("Cannot find the dead player!");
        }
        
        
    }


    //투표로 교수나 대학원생 죽을 때
    [PunRPC]
    void spreadDeadInfoWithoutInstantiate(String deadNickName, int profileIndex)
    {
        SetMyPlayer();
        if(myPlayer.CompareTag("Dead")) return;
        
        GameObject deadPlayer = null;
        players = GameManagerScript.instance.getPlayers();

        foreach(GameObject p in players)
        {
            if(p.GetComponent<CharacterInfoSetting>().nickName == deadNickName)
            {
                deadPlayer = p;
            }
        }
        if(deadPlayer != null)
        {
            deadPlayer.GetComponent<CharacterInfoSetting>().isAlive = false;
            deadPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
            //deadInfo.isAlive = false;
            profileImages = settingPlayerManager.profileImages;
            profileNicknames = settingPlayerManager.profileNicknames;

            profileImages[profileIndex].color = Color.gray;
            profileNicknames[profileIndex].color = Color.red;

            GameManagerScript.instance.SetPlayerList(myPlayerInfo.role, -1);
        }
        //if(deadPlayer != null) deadPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
        //CharacterInfoSetting deadInfo = deadPlayer.GetComponent<CharacterInfoSetting>();
        
        //죽은 유저 정보 수정 및 사용
        // if(deadNickName == myPlayerInfo.nickName)
        // {
        // myPlayerInfo.isAlive = false;
        // myPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
        // }
        
    }

    [ContextMenu("dodie")]
    public void dodie()
    {
        SetMyPlayer();
        PV.RPC("spreadDeadInfo", RpcTarget.AllBuffered, myPlayerInfo.nickName);
    }
}
