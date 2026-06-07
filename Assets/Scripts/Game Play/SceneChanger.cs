using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class SceneChanger : MonoBehaviourPunCallbacks
{
    //씬 이동 시 발동 함수
    public string SceneName;
    GameObject player;
    
    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for(int i = 0; i < players.Length; i++)
            {
                player = players[i];
                if(player.GetComponent<PhotonView>().IsMine) break;
            }
        }
    }


    //씬 이동 함수, startbutton의 경우 게임 시작 상태를 지정
    public void startChangeScene()
    {
        string ButtonName = EventSystem.current.currentSelectedGameObject.name;
        // if(ButtonName == "StartButton")
        // {
        //     GameManagerScript.instance.setGameStart();
        // }
        //gameObject.GetComponent<PhotonView>().RPC("startPLobby", RpcTarget.AllBuffered);
        PhotonNetwork.LoadLevel(SceneName);
    }

    // [PunRPC]
    // public void startPLobby()
    // {
    //     GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //     for(int i = 0; i < players.Length; i++) players[i].GetComponent<CharacterInfoSetting>().currentSceneName = SceneName;
    // }

    // Scene Change and Set position to zero when scene changes(Maybe the condition of if is not necessary)
    
    public void changeScene()
    {
        print("Try to change your scene to " + SceneName);

        CharacterInfoSetting info = GameManagerScript.instance.getMyPlayerInfo();

        // 플레이어가 다른 방으로 이동했음에도 여전히 방의 위치가 "Lobby"로 저장되는 버그가 있다.
        // + 씬 합치기 과정 중, 다른 씬이 구현이 안 되었음에도 로비 씬을 통해 미구현된 씬으로 이동 시 스폰 위치가 Spawn씬 내부로 설정되어 발생하는 오류로 보임.
        // + 개발 완료 시 버그는 사라질 것으로 생각됨.
        // if(info.currentSceneName == SceneName)
        // {
        //     print("Something wrong with player's room name info. Start Recover.");
        //     info.currentSceneName = GameObject.FindGameObjectWithTag("Door").GetComponent<RoomName>().roomName;
        //     print("Please try to collide again. If the problem still goes on, idk :>");
        //     return;
        // }
  
        GameObject prevmiscanv = GameObject.Find(info.currentSceneName + "MissionCanvas");
        if(prevmiscanv != null) prevmiscanv.GetComponent<Canvas>().worldCamera = GameObject.Find("Camera").transform.Find(info.currentSceneName + " Camera").GetComponent<Camera>(); else print("PrevCanvas Missing");
        
        //player.transform.position = Vector2.zero;
        switch (SceneName)
        {
            case "Lobby":
                positioning(info.currentSceneName + "ILSP");
                break;

            case "Lecture1":    case "Lecture2":    case "Lecture3":    case "Lecture4":    case "Lecture5":    case "Lecture6":
            case "ManToilet":   case "WomanToilet": case "Store":       case "Kitchen":     
            case "StudyRoom":   case "Spawn":       case "Entrance":    case "Garden":
                positioning(SceneName + "SP");
                break;
            case "DiningRoom":
                if(info.currentSceneName == "Kitchen")
                {
                    positioning(SceneName + "SP2");
                    break;
                }  
                else
                {
                    positioning(SceneName + "SP");
                    break;
                }

            default:
                positioning("");
                break;
        }

        GameObject miscanv = GameObject.Find(SceneName + "MissionCanvas");
        if(miscanv != null) miscanv.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Move Camera").GetComponent<Camera>(); else print("CurCanvas Missing");
        
        info.currentSceneName = SceneName;
        info.SetMyScene(SceneName);

        Camera.main.GetComponent<CameraPosition>().cameraSetting(SceneName);
        print("Scene Change Done");
    }

    IEnumerator moveToSpawn()
    {
        GameObject miscanv = null;
        while(true)
        {
            miscanv = GameObject.Find("SpawnMissionCanvas");

            if(miscanv != null) break;
            // else print("??");

            yield return new WaitForSeconds(0.2f);
        }
        print("Spawn missioncanvas Pass");
        // Camera mainCamera = GameObject.Find("Main Move Camera").GetComponent<Camera>();
        miscanv.GetComponent<Canvas>().worldCamera = Camera.main;

        Transform readyObj = GameManagerScript.instance.readyObj.transform;
        readyObj.Find("MainMobileUI").GetComponent<Canvas>().worldCamera = Camera.main;
        readyObj.Find("WindowUI").GetComponent<Canvas>().worldCamera = Camera.main;
        
        // GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // for(int i = 0; i < players.Length; i++)
        // {
        //     // players[i].transform.GetChild(1).GetComponent<Canvas>().worldCamera = mainCamera;
        //     if(players[i].GetComponent<PhotonView>().IsMine) { player = players[i]; break; }
        // }

        positioning("");

        // Destroy(GameObject.Find("ReadyManager"));
        // gameObject.GetComponent<CountdownUI>().countDownDone();

        Camera.main.GetComponent<CameraPosition>().cameraSetting("Spawn");

        print("Spawn Done");
    }
    
    void  positioning(string _spawnPoint)
    {
        Vector2 spawnPoint = Vector2.zero;
        if(_spawnPoint != "") spawnPoint = GameObject.Find(_spawnPoint).transform.position;
        else spawnPoint = Vector2.zero;

        player = GameManagerScript.instance.getMyPlayer();
        print("얘 있는데 왜 안 되냐?? " + player.name);
        player.transform.position = spawnPoint;
    }

    public void exitScene()
    {
        Application.Quit();
    }
}
