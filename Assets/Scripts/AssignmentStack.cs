using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AssignmentStack : MonoBehaviourPunCallbacks
{
    // //Managing assignment information script for students and graduate students
    // public int assignmentStack = 0;
    // private PlayerUISettingScript playerUISettingScript;

    // GameObject[] players;
    // GameObject player;

    // private void Start()
    // {
    //     if(!gameObject.GetPhotonView().IsMine) return;
    //     playerUISettingScript = GameManagerScript.instance.readyObj.transform.Find("PlayerUI").GetComponent<PlayerUISettingScript>();
    // }

    // private void Update()
    // {
    //     if (gameObject.GetComponent<PhotonView>().IsMine)
    //     {
    //         players = GameObject.FindGameObjectsWithTag("Player");
    //         for(int i = 0; i < players.Length; i++) if(players[i].GetComponent<PhotonView>().IsMine) player = players[i];
    //         string playerscene = player.GetComponent<CharacterInfoSetting>().currentSceneName;
    //         bool inLecture = false;
    //         for(int i = 1; i <= 6; i++)
    //         {
    //             if(playerscene.CompareTo("Lecture" + i) == 0)
    //             {
    //                 inLecture = true;
    //                 break;
    //             }
    //         }
    //         if(!inLecture) return;
    //         GameObject door = GameObject.Find(playerscene + "Background").transform.Find("Door1").gameObject;
    //         int roomNum = door.GetComponent<RoomName>().lectureRoomNum;
    //         if (roomNum != -1)
    //         {
    //             if (roomNum == gameObject.GetComponent<CharacterInfoSetting>().myRoomNum)
    //             {
    //                 playerUISettingScript.addAssignmentStack(assignmentStack);
    //             }
    //         }
    //     }
    //     if (gameObject.GetComponent<CharacterInfoSetting>().isAlive && gameObject.GetComponent<CharacterInfoSetting>().role == "student" && assignmentStack == 3)
    //     {
    //         StuNGraPlayerDie();
    //     }
    //     else if((gameObject.GetComponent<CharacterInfoSetting>().isAlive && gameObject.GetComponent<CharacterInfoSetting>().role == "graduate" && assignmentStack == 2))
    //     {
    //         StuNGraPlayerDie();
    //     }
    // }


    // [PunRPC]
    // void setAssignmentStack()
    // {
    //     assignmentStack++;
    //     print("get assignment " + assignmentStack);
    // }

    // [PunRPC]
    // void GraduateStackSend()
    // {
    //     --assignmentStack;
    //     playerUISettingScript.decAssignmentStack(assignmentStack);
    //     print("get assignment " + assignmentStack + "from graduate");
    // }

    // public void StuNGraPlayerDie()
    // {
    //     // gameObject.GetComponent<CharacterInfoSetting>().die();
    //     gameObject.GetComponent<PhotonView>().RPC("spreadDeadInfo", RpcTarget.AllBuffered, gameObject.GetComponent<CharacterInfoSetting>().nickName);
    //     PlayerMulti playerMulti = gameObject.GetComponent<PlayerMulti>();
    //     playerMulti.NickNameText.color = Color.red;
    //     gameObject.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
    //     //Debug.Log("you are dead");

    //     if (gameObject.GetComponent<PhotonView>().IsMine)
    //     {
    //         MissionProgressChecker missionProgressChecker = GameManagerScript.instance.readyObj.transform.Find("MainMobileUI/MissionProgressBar").GetComponent<MissionProgressChecker>();
    //         CharacterInfoSetting characterInfoSetting = gameObject.GetComponent<CharacterInfoSetting>();
    //         //missionProgressChecker.curClearNum -= characterInfoSetting.missionRemained;
    //         //missionProgressChecker.missionNum -= characterInfoSetting.assignedMission.Count;
    //         missionProgressChecker.BarInfoChangeRunner(1, characterInfoSetting.assignedMission.Count - characterInfoSetting.missionRemained);
    //         missionProgressChecker.BarInfoChangeRunner(0, characterInfoSetting.assignedMission.Count);
    //     }
    // }
}
