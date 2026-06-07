using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SabotagePlayer : MonoBehaviourPunCallbacks
{
    public GameObject lightImage;
    private GameObject roomLight;
    private GameObject missionWindow;
    private GameObject sabotageButton;
    private GameObject lightObj;

    [PunRPC]
    void RoomLightOff()
    {
        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            lightObj = GameManagerScript.instance.readyObj.transform.Find("MainMobileUI/TestLight").gameObject;
            lightObj.SetActive(true);
            StartCoroutine(RoomLightOn());
        }
    }

    IEnumerator RoomLightOn()
    {
        yield return new WaitForSeconds(5.0f);
        //lightImage.SetActive(false);
        //Destroy(roomLight);
        lightObj.SetActive(false);
    }
    [PunRPC]
    void ProfLightOff()
    {
        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            roomLight = Instantiate(lightImage);
            GameObject parent = GameObject.Find(gameObject.GetComponent<CharacterInfoSetting>().currentSceneName + "MissionCanvas");
            //오류
            roomLight.transform.SetParent(parent.transform, false);
            roomLight.SetActive(true);
            StartCoroutine(ProfLightOn());
        }
    }

    IEnumerator ProfLightOn()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(roomLight);
    }

    [PunRPC]
    void MissionOff()
    {
        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            //오류
            CharacterInfoSetting info = gameObject.GetComponent<CharacterInfoSetting>();
            string mission = "MissionWindow" + info.missionSolvingNum.ToString();
            if(null == GameObject.Find(info.currentSceneName + "MissionCanvas").transform.Find(mission + "(Clone)"))
            {
                return;
            }
            else
            {
                missionWindow = GameObject.Find(info.currentSceneName + "MissionCanvas").transform.Find(mission + "(Clone)").gameObject;
                Destroy(missionWindow);
            }
            
            //missionWindow.SetActive(false);
        }
    }

    [PunRPC]
    void AssignmentCooltimeExpand()
    {
        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            Transform assignmentButton = GameManagerScript.instance.readyObj.transform.Find("MainMobileUI/AssignmentButton");
            if (assignmentButton != null) assignmentButton.GetComponent<AssignmentManager>().sabotaged = true;
        }
    }
}
