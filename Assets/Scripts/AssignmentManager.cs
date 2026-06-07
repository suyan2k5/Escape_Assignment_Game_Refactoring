using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;


public class AssignmentManager : MonoBehaviourPunCallbacks
{
    //professor can manage assignments(set) through this script.
    private GameObject myPlayer;  //me
    private GameObject[] players;
    private bool findStudent = false;
    private CharacterInfoSetting student;
    private PhotonView PV; // if it is null, this script is attached at Assignment Button. And it is not, this is attached at Players.
    private bool isButton;
    public bool isUsable = false, timeReset = true;
    public bool sabotaged = false;
    public float cooltime = 5f, curtime;
    public Text cooltimeText;

    private void Start()
    {
        PV = gameObject.GetComponent<PhotonView>();
        isButton = PV == null ? true : false;

        // cooltimeText = GameObject.Find("Test/MainMobileUI").transform.GetChild(4).GetChild(3).gameObject.GetComponent<Text>();
        curtime = cooltime;
    }

    private void Update()
    {
        if(myPlayer == null) findPlayer();
        if (isUsable)
        {
            if(isButton) cooltimeText.text = "";
        }
        else
        {
            if (isButton) {
                gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }

            if (curtime > 0f)
            {
                curtime -= Time.deltaTime;
                cooltimeText.text = Mathf.Ceil(curtime).ToString();
                if (sabotaged)
                {
                    if (isButton) curtime += cooltime * 0.2f;
                    sabotaged = false;
                }
            }
            else
            {
                if (isButton)
                {
                    if(findStudent) curtime = cooltime;
                    gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    cooltimeText.text = Mathf.Ceil(curtime).ToString();
                }
                isUsable = true;
            }
        }
    }

    private void findPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PhotonView>().IsMine) myPlayer = players[i];
        }
    }

    public void assignmentButton()
    {
        //room number and room's student check
        //room could be that there are no allocated student 
        //if the allocated student exists, get players list
        //find exact room student from the list
        //get assignmentstack component of the student and active func 'getAssignment()'
        if (isUsable)
        {
            isUsable = false;
            roomNameCheck();
            if (findStudent)
            {
                CharacterInfoSetting myChar = myPlayer.GetComponent<CharacterInfoSetting>();
                if((myChar.currentSceneName == "Lecture1")
                    || (myChar.currentSceneName == "Lecture2")
                    || (myChar.currentSceneName == "Lecture3")
                    || (myChar.currentSceneName == "Lecture4")
                    || (myChar.currentSceneName == "Lecture5")
                    || (myChar.currentSceneName == "Lecture6"))
                {
                    if (myChar.role == "professor")
                    {
                        student.GetComponent<PhotonView>().RPC("setAssignmentStack", RpcTarget.All);
                        curtime = sabotaged ? cooltime * 1.2f : cooltime;
                        print("findstudent");
                    }
                    else if (myChar.role == "graduate")
                    {
                        if(GameManagerScript.instance.getProfAuthority && (myChar.currentSceneName != "Lecture" + myChar.myRoomNum))
                        {
                            student.GetComponent<PhotonView>().RPC("setAssignmentStack", RpcTarget.All);
                            curtime = sabotaged ? cooltime * 1.2f : cooltime;
                        }
                        else if (myChar.assignmentStack > 0 && (myChar.currentSceneName != "Lecture" + myChar.myRoomNum))
                        {
                            // Send => graduate: dec, student: inc
                            myPlayer.GetComponent<PhotonView>().RPC("GraduateStackSend", RpcTarget.All);
                            student.GetComponent<PhotonView>().RPC("setAssignmentStack", RpcTarget.All);
                            print("graduate send sb assignment");
                        }
                    }
                }
            }
            findStudent = false;
        }
    }

    public void roomNameCheck()
    {
        int roomNum = 0;
        string playerscene = myPlayer.GetComponent<CharacterInfoSetting>().currentSceneName;
        GameObject door = GameObject.Find(playerscene + "Background").transform.Find("Door1").gameObject;
        if(door != null)
        {
            roomNum = door.GetComponent<RoomName>().lectureRoomNum;
        }
        if (roomNum != -1)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<CharacterInfoSetting>().myRoomNum == roomNum &&
                    players[i].GetComponent<CharacterInfoSetting>().role != "professor")
                {
                    student = players[i].GetComponent<CharacterInfoSetting>();
                    findStudent = true;
                    print("room name check");
                    break;
                }
                else
                {
                    print("didnt find student");
                }
            }
        }
        else return;
    }

    public void pointerEnter()
    {
        gameObject.GetComponent<Animation>().Play("AssignmentButton");
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void pointerExit()
    {
        gameObject.GetComponent<Animation>().Play("AssignmentButtonClose");
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
}
