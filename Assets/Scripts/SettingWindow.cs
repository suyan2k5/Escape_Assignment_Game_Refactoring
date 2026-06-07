using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SettingWindow : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject settingWindow;
    [SerializeField] GameObject contactOrLicenseWindow;
    [SerializeField] Text contactOrLicenseText;
    bool isOnContactWindow;
    // Joystick Fix Setting
    [SerializeField] Slider joystickFix;
    [SerializeField] JoyStick joyStick;

    // Other Option
    public GameObject exitGameBtn;

    public void MyAwake()
    {
        joystickFix = settingWindow.transform.Find("Panel/JoystickFixSlider").GetComponent<Slider>();
        joyStick = GameManagerScript.instance.readyObj.transform.Find("MainMobileUI/JoyStickObject/JoyStick").GetComponent<JoyStick>();
        joystickFixOption();
        exitGameBtn.SetActive(true);
        print(exitGameBtn.gameObject.name);
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(isOnContactWindow)
            {
                contactOrLicenseWindow.SetActive(false);
                isOnContactWindow = false;
            }
        }
    }

    public void joystickFixOption()
    {
        if(joystickFix.value > 0.5)
        {
            // Mathf.Lerp(joystickFix.value, 1, Time.deltaTime * 10);
            joystickFix.value = 1;
            joyStick.fixOption = false;
        }
        else
        {
            // Mathf.Lerp(joystickFix.value, 0, Time.deltaTime * 10);
            joystickFix.value = 0;
            joyStick.fixOption = true;
        }

        joyStick.imgSet(1);
    }

    public void onContactUs()
    {
        contactOrLicenseWindow.SetActive(true);
        isOnContactWindow = true;
        contactOrLicenseText.text = "kampy1013@ajou.ac.kr";
    }

    public void onLicense()
    {
        contactOrLicenseWindow.SetActive(true);
        isOnContactWindow = true;
        contactOrLicenseText.text = "최시유 simonjm@ajou.ac.kr \n\n이용규 neli3307@ajou.ac.kr \n\n조은진 whdmswls9207@ajou.ac.kr \n\n임수연 kampy1013@ajou.ac.kr";
    }

    public void exitGame()
    {
        GameManagerScript.instance.ExitGame();
    }

    IEnumerator playerLeaveRoom()
    {
        PhotonNetwork.Destroy(GameManagerScript.instance.getMyPlayer());
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().BeforeLeftRoom();
        PhotonNetwork.LeaveRoom();
        while(PhotonNetwork.InRoom)     yield return null;

        print("test leave room");
        PhotonNetwork.LoadLevel("Mainmenu");

        GameObject[] objects = FindObjectsOfType<GameObject>();
        for(int i = 0; i < objects.Length; i++)
        {
            if(objects[i].tag != "MainObject" && objects[i].name != "Main Camera" && objects[i].name != "PhotonMono")
            {
                Destroy(objects[i]);
            }
        }
    }

    public void leaveRoomBtnOff() => exitGameBtn.SetActive(false);
}
