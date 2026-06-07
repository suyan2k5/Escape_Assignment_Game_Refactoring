using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CustomizeItem : MonoBehaviour
{
    [Header("Images for Customize")]
    [SerializeField] Sprite[] TopImages;
    [SerializeField] Sprite[] BottomImages;
    [SerializeField] Sprite[] BottomWalkImages;
    [SerializeField] Sprite[] HatImages;
    [SerializeField] Sprite[] AccessoriesImages;
    [SerializeField] Sprite[] ImagesForCustomize;
    [SerializeField] Sprite[] BottomsForCustomize;

    [Header("Character Images")]
    [SerializeField] Image CharacterAvatarImage;
    [SerializeField] Sprite[] CharImages;
    public GameObject selectedAcc, selectedTop, selectedHat, selectedBottom;

    [Header("Windows")]
    public GameObject customizeWindow;
    public GameObject[] Panels;
    public GameObject savePanel;

    [Header("Others")]
    [SerializeField] Image[] LineImages;
    // [SerializeField] CountdownUI countdownUI;
    public PhotonView PV;

    private Vector2 pos, size;
    private GameObject myPlayer;
    private GameObject[] players;
    private Sprite saveTopImage, saveBottomImage, saveHatImage, saveAccImage;
    private int topIndex, bottomIndex, hatIndex, accIndex = -1;
    private int playerIndex;
    private GameObject customPlayer;
    private string customNickname;


    public void onCustomizeWindow()
    {
        print("teststst");
        // if(!countdownUI.isCounting)
        // {
        customizeWindow.GetComponent<WindowAnimation>().OnAnimUI();
        changeToTop();
        // }
        setPlayer();
    }
    private void setPlayer() 
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++) if (players[i].GetComponent<PhotonView>().IsMine)
        {
            myPlayer = players[i];
            customNickname = players[i].GetComponent<CharacterInfoSetting>().nickName;
            playerIndex = i;
        }
        CharacterAvatarImage.sprite = CharImages[myPlayer.GetComponent<CharacterInfoSetting>().animalInd];
    }

    private void changeLine(int index)
    {
        for(int i = 0; i < 4; i++)
        {
            if(i == index) LineImages[i].gameObject.SetActive(false);
            else LineImages[i].gameObject.SetActive(true);
        }
    }

    private void changePanel(int index)
    {
        for(int i = 0; i < 4; i++)
        {
            if(i == index) Panels[i].SetActive(true);
            else Panels[i].SetActive(false);
        }
    }

    public void changeToTop()
    {
        changePanel(0);
        changeLine(0);
    }

    public void changeToBottom()
    {
        changePanel(1);
        changeLine(1);
        
    }

    public void changeToHat()
    {
        changePanel(2);
        changeLine(2);
    }

    public void changeToAcc()
    {
        changePanel(3);
        changeLine(3);
    }

    public void showTopExample(int index)
    {
        if(index == -1)
        {
            size = new Vector2(0, 160);    //없음
            saveTopImage = null;
        }
        else if(index == 0)       //빨간 맨투맨
        {
            pos = new Vector2(11, -25);
            size = new Vector2(230, 160);
        }
        else if(index == 1)    //정장 상의
        {
            pos = new Vector2(11, -19);
            size = new Vector2(230, 150);
        }
        else if(index == 2)    //흰 후드티
        {
            pos = new Vector2(13, -15);
            size = new Vector2(230, 150);
        }
        else if(index == 3)    //회색 후드티
        {
            pos = new Vector2(11, -19);
            size = new Vector2(230, 150);
        }
        selectedTop.GetComponent<RectTransform>().anchoredPosition = pos;
        selectedTop.GetComponent<RectTransform>().sizeDelta = size;
        if(index != -1)
        {
            selectedTop.GetComponent<Image>().sprite = TopImages[index];
            saveTopImage = TopImages[index];
            topIndex = index;
        }
        else topIndex = -1;
    }

    public void showBottomExample(int index)
    {
        if(index == -1)
        {
            size = new Vector2(0, 160);    //없음
            saveBottomImage = null;
        }
        else if(index == 0)     //나팔바지
        {
            pos = new Vector2(9, -117);
            size = new Vector2(230, 85);
        }
        else if(index == 1)     //정장 바지
        {
            pos = new Vector2(9, -113);
            size = new Vector2(185, 85);
        }
        else if(index == 2)     //청바지
        {
            pos = new Vector2(9, -117);
            size = new Vector2(190, 85);
        }
        selectedBottom.GetComponent<RectTransform>().anchoredPosition = pos;
        selectedBottom.GetComponent<RectTransform>().sizeDelta = size;
        if(index != -1)
        {
            selectedBottom.GetComponent<Image>().sprite = BottomImages[index];
            saveBottomImage = BottomImages[index];
            bottomIndex = index;
        } 
        else bottomIndex = -1;
    }

    public void showHatExample(int index)
    {
        if(index == -1)
        {
            size = new Vector2(0, 160);    //없음
            saveHatImage = null;
        }
        else if(index == 0)       //꽃
        {
            pos = new Vector2(24, 283);
            size = new Vector2(90, 120);
        }
        else if(index == 1)    //사과
        {
            pos = new Vector2(32, 254);
            size = new Vector2(120, 90);
        }
        else if(index == 2)    //푸딩
        {
            pos = new Vector2(24, 252);
            size = new Vector2(90, 100);
        }
        else if(index == 3)    //새싹
        {
            pos = new Vector2(22, 262);
            size = new Vector2(100, 80);
        }
        else if(index == 4)    //귤
        {
            pos = new Vector2(24, 241);
            size = new Vector2(80, 80);
        }
        else if(index == 5)    //리본
        {
            pos = new Vector2(24, 228);
            size = new Vector2(120, 80);
        }
        selectedHat.GetComponent<RectTransform>().anchoredPosition = pos;
        selectedHat.GetComponent<RectTransform>().sizeDelta = size;
        if(index != -1)
        {
            selectedHat.GetComponent<Image>().sprite = HatImages[index];
            saveHatImage = HatImages[index];
            hatIndex = index;
        } 
        else hatIndex = -1;
    }

    public void showAccExample(int index)
    {
        if(index == -1)
        {
            size = new Vector2(0, 160);    //없음
            saveAccImage = null;
        }
        else if(index == 0)     //실험도구
        {
            pos = new Vector2(-111, -58);
            size = new Vector2(80, 120);
        }
        else if(index == 1)     //지팡이
        {
            pos = new Vector2(120, -116);
            size = new Vector2(60, 140);
        }
        else if(index == 2)     //책
        {
            pos = new Vector2(132, -73);
            size = new Vector2(90, 95);
        }

        selectedAcc.GetComponent<RectTransform>().anchoredPosition = pos;
        selectedAcc.GetComponent<RectTransform>().sizeDelta = size;
        if(index != -1)
        {
            selectedAcc.GetComponent<Image>().sprite = AccessoriesImages[index];
            saveAccImage = AccessoriesImages[index];
            accIndex = index;
        }
        else accIndex = -1;
    }

    
    public void exitWindow()
    {
        if((selectedTop.GetComponent<Image>().sprite == null) &&
            (selectedBottom.GetComponent<Image>().sprite == null) && (selectedAcc.GetComponent<Image>().sprite == null))
        {
            selectedTop.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 160);
            selectedBottom.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 160);
            selectedHat.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 160);
            selectedAcc.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 160);
            customizeWindow.GetComponent<WindowAnimation>().OffAnimUI();
        }
        else    savePanel.SetActive(true);
    }

    public void saveChanges()
    {        
        //change prefab
        customizeWindow.GetComponent<WindowAnimation>().OffAnimUI();
        savePanel.SetActive(false);
        
        if(saveBottomImage != null)
        {
            if(myPlayer.GetComponent<PhotonView>().IsMine) PV.RPC("applyBottomCustomize", RpcTarget.AllBuffered, (bottomIndex + TopImages.Length) ,customNickname);
        }
        else
        {
            if(myPlayer.GetComponent<PhotonView>().IsMine) PV.RPC("applyBottomCustomize", RpcTarget.AllBuffered, -1, customNickname);
        }

        if(saveTopImage != null)
        {
            if(myPlayer.GetComponent<PhotonView>().IsMine) PV.RPC("applyTopCustomize", RpcTarget.AllBuffered, topIndex, customNickname);
        }
        else
        {
            if(myPlayer.GetComponent<PhotonView>().IsMine) PV.RPC("applyTopCustomize", RpcTarget.AllBuffered, -1, customNickname);
        }
        
        if(saveHatImage != null)
        {
            if(myPlayer.GetComponent<PhotonView>().IsMine) PV.RPC("applyHatCustomize", RpcTarget.AllBuffered, (hatIndex + TopImages.Length + BottomImages.Length) ,customNickname);
        }
        else
        {
            if(myPlayer.GetComponent<PhotonView>().IsMine) PV.RPC("applyHatCustomize", RpcTarget.AllBuffered, -1, customNickname);
        }
        
        if(saveAccImage != null)
        {
            if(myPlayer.GetComponent<PhotonView>().IsMine) PV.RPC("applyAccCustomize", RpcTarget.AllBuffered, (accIndex + TopImages.Length + BottomImages.Length + HatImages.Length), customNickname);
        }
        else
        {
            if(myPlayer.GetComponent<PhotonView>().IsMine) PV.RPC("applyAccCustomize", RpcTarget.AllBuffered, -1, customNickname);
        }
                                                                                                                
    }

    [PunRPC]
    void applyTopCustomize(int idx, string pName)
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < characters.Length; i++)
        {
            if(characters[i].GetComponent<CharacterInfoSetting>().nickName == pName)
            {
                customPlayer = characters[i];
            }
        }
        if(idx == -1)
        {
            customPlayer.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            customPlayer.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = ImagesForCustomize[idx];
        }
    }

    [PunRPC]
    void applyBottomCustomize(int idx, string pName)
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < characters.Length; i++)
        {
            if(characters[i].GetComponent<CharacterInfoSetting>().nickName == pName)
            {
                customPlayer = characters[i];
            }
        }
        if(idx == -1)
        {
            customPlayer.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;
            customPlayer.GetComponent<PlayerMulti>().customAnimNum = 0;
        }
        else
        {
            customPlayer.transform.GetChild(5).gameObject.SetActive(true);
            customPlayer.transform.GetChild(5).GetComponent<SpriteRenderer>().sprite = ImagesForCustomize[idx];
            customPlayer.GetComponent<PlayerMulti>().customAnimNum = bottomIndex + 1;
        }
    }

    [PunRPC]
    void applyHatCustomize(int idx, string pName)
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < characters.Length; i++)
        {
            if(characters[i].GetComponent<CharacterInfoSetting>().nickName == pName)
            {
                customPlayer = characters[i];
            }
        }
        if(idx == -1)
        {
            customPlayer.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = null;
        }
        else    customPlayer.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = ImagesForCustomize[idx];
    }


    [PunRPC]
    void applyAccCustomize(int idx, string pName)
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < characters.Length; i++)
        {
            if(characters[i].GetComponent<CharacterInfoSetting>().nickName == pName)
            {
                customPlayer = characters[i];
            }
        }
        if(idx == -1)
        {
            customPlayer.transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = null;
        }
        else    customPlayer.transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = ImagesForCustomize[idx];
        
    }

    public void discardChanges()
    {
        selectedTop.GetComponent<Image>().sprite = null;
        selectedBottom.GetComponent<Image>().sprite = null;
        selectedHat.GetComponent<Image>().sprite = null;
        selectedAcc.GetComponent<Image>().sprite = null;
        customizeWindow.GetComponent<WindowAnimation>().OffAnimUI();
        savePanel.SetActive(false);
        selectedTop.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 160);
        selectedBottom.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 160);
        selectedHat.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 160);
        selectedAcc.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 160);
    }

}
