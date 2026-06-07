using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using Unity.Jobs;
using Unity.Collections;

public class GameEndManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool wholeMissionComplete, isGameEnd, isStuWin, isProfWin, isGradWin, canProfWin;
    GameObject windowUI, EndWindow;
    GameObject WinText, LoseText;
    Button closebtn;
    PhotonView PV;
    Toggle test1;
    Toggle test2;

    void Start()
    {
        PV = gameObject.GetPhotonView();
        if(!PV.IsMine) return;
        wholeMissionComplete = false;
        isGameEnd = false;
        isStuWin = false;
        isProfWin = false;
        isGradWin = false;
        canProfWin = false;

        windowUI = GameManagerScript.instance.readyObj.transform.Find("WindowUI").gameObject;
        EndWindow = windowUI.transform.GetChild(3).gameObject;
        // WinText = windowUI.transform.GetChild(3).GetChild(1).GetChild(0).gameObject;
        // LoseText = windowUI.transform.GetChild(3).GetChild(1).GetChild(1).gameObject;
        // closebtn = windowUI.transform.GetChild(3).GetChild(1).GetChild(2).gameObject.GetComponent<Button>();
        // closebtn.onClick.AddListener(CloseEndWindow);


        // test1 = GameObject.Find("MainMobileUI").transform.GetChild(14).GetComponent<Toggle>();
        // test2 = GameObject.Find("MainMobileUI").transform.GetChild(16).GetComponent<Toggle>();
    }
    
    List<GameObject> PCLights = new List<GameObject>();
    void Update()
    {
        // Game End Check
        if (PV.IsMine && !isGameEnd)
        {
            CharacterInfoSetting[] psInfo = GameManagerScript.instance.getPlayersInfo();


            // If all missions are cleared, wholeMissionComplete will be true using StudentWin RPC method.
            // Then,
            if (wholeMissionComplete)
            {
                // Lecture PC Light ON
                if(PCLights.Count == 0)
                {
                    for(int num = 1; num <= 6; num++)
                    {
                        PCLights.Add(GameObject.Find("Lecture" + num + "Background/PCLight"));
                    }
                }
                for(int num = 0; num < 6; num++)
                {
                    PCLights[num].GetComponent<SpriteRenderer>().color = Color.blue;
                }

                // Finish Check
                for (int i = 0; i < psInfo.Length; i++)
                {
                    if(psInfo[i].role != "student" || psInfo[i].isAlive == false) continue;

                    if (psInfo[i].currentSceneName != "Entrance")
                    {
                        isGameEnd = false;
                        break;
                    }
                    isGameEnd = true;
                    if(!isStuWin && !isProfWin && !isGradWin) isStuWin = true;
                }
            }

            // Professor Win Condition
            // If there's at least one non-professor player, canProfWin will be true.
            // It is to prevent wrong win occured on 1 player play.
            // Then check the whole non-professor players are dead.
            
            canProfWin = false;
            isProfWin = true;
            foreach (var pinfo in psInfo)
            {
                if(pinfo.role != "professor")
                {
                    canProfWin = true;
                    if(pinfo.isAlive) isProfWin = false;
                }
            }
            if(canProfWin && isProfWin && (GameManagerScript.instance.PlayerList["professor"] > 0)) isGameEnd = true;

            // Graduate Win Condition
            // No Student and No professor.
            //1. Professor all die -> 2. Graduate gets authority of professor.
            //3. And then graduate should kill all the students.
            if((GameManagerScript.instance.PlayerList["student"] == 0) && (GameManagerScript.instance.PlayerList["professor"] == 0) && (GameManagerScript.instance.PlayerList["graduate"] >= 1))
            {
                if(GraduateManager.canGraduateWin)
                {
                    isGradWin = true;
                    isGameEnd = true;
                }
                
            }


            // Show Game Result
            if (isGameEnd)
            {
                // Image Setting
                if(isStuWin)
                {
                    EndWindow.transform.GetChild(1).GetComponent<Image>().sprite = GameManagerScript.instance.getStuWinSprite();
                }
                else if(isProfWin)
                {
                    EndWindow.transform.GetChild(1).GetComponent<Image>().sprite = GameManagerScript.instance.getProfWinSprite();
                }
                else if(isGradWin)
                {
                    EndWindow.transform.GetChild(1).GetComponent<Image>().sprite = GameManagerScript.instance.getGradWinSprite();
                }

                // Window On
                EndWindow.GetComponent<WindowAnimation>().OnAnimUI();
                StartCoroutine(moveToReadyScene());
            }
        }
    }

    // public void CloseEndWindow()
    // {
    //     EndWindow.GetComponent<WindowAnimation>().OffAnimUI();
    // }

    [PunRPC]
    public void StudentWin()
    {
        wholeMissionComplete = true;
    }

    // [PunRPC]
    // public void PlayerGameResult(bool result)
    // {
    //     isWin = result;
    // }

    void PhotonDestroyObject(ref GameObject[] obj)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            for(int i = 0; i < obj.Length; i++)
            {
                if(obj[i].transform.parent) continue;
                if(!obj[i].CompareTag("MainObject") && obj[i].name != "PhotonMono" && obj[i].name != "GameManager" && !obj[i].CompareTag("Player"))
                {
                    if(obj[i].TryGetComponent<PhotonView>(out PhotonView objPV))
                        PhotonNetwork.Destroy(obj[i]);
                    else
                        Destroy(obj[i]);
                }
            }
        }
    }

    void DestroyObject(ref GameObject[] obj)
    {
        for(int i = 0; i < obj.Length; i++)
        {
            if(obj[i].transform.parent) continue;
            if(!obj[i].CompareTag("MainObject") && obj[i].name != "PhotonMono" && obj[i].name != "GameManager" && !obj[i].CompareTag("Player"))
            {
                Destroy(obj[i]);
            }
        }
    }

    private bool _wait;
    public bool wait
    {
        get
        {
            return _wait;
        }
        set
        {
            _wait = value;
            Debug.LogError("GameEndManager.wait == " + wait + " in field");
        }
    }

    [PunRPC]
    public void DestroyStartRPC()
    {
        /*Debug.LogError("Make a new Thread");
        Thread thread = new Thread(DestroyStart);
        thread.Start();*/
        NativeArray<bool> bools = new NativeArray<bool>(1, Allocator.TempJob);
        TestJob testJob = new TestJob();
        testJob.bools = bools;
        JobHandle handle = testJob.Schedule();
        handle.Complete();
        wait = bools[0];
        bools.Dispose();
        Debug.LogError("GameEndManager.wait is " + wait + " in main thread");
        /*Debug.LogError("Thread Started");*/
    }

    public struct TestJob : IJob
    {
        public NativeArray<bool> bools;

        public void Execute()
        {
            bools[0] = false;
        }
    }

    /*public void DestroyStart()
    {
        if(!PV.IsMine) return;
        wait = false;
        Debug.LogError("GameEndManager.wait is " + wait + " in new thread");
        Debug.LogError("------Destroy Start------" + wait);
    }*/


    IEnumerator moveToReadyScene() {
        GameManagerScript.instance.updatePause = true;
        yield return new WaitForSeconds(5f);
        EndWindow.transform.GetChild(1).GetComponent<Image>().sprite = null;
        EndWindow.SetActive(false);

        // Destroy(GameManagerScript.instance.readyObj);

        // GameObject[] obj = GameObject.FindObjectsOfType<GameObject>();
        
        if(PhotonNetwork.IsMasterClient)
        {
            GameObject[] obj = GameObject.FindObjectsOfType<GameObject>();
            for(int i = 0; i < obj.Length; i++)
            {
                if(obj != null)
                {
                    if(obj[i].transform.parent) continue;
                    Debug.LogError(obj[i].name);
                    if(obj[i].TryGetComponent<PhotonView>(out PhotonView objPV))
                    {
                        if(objPV.AmOwner)
                        {
                            if(!obj[i].CompareTag("MainObject") && obj[i].name != "GameManager" && !obj[i].CompareTag("Player"))
                            {
                                if(obj[i].GetComponent<ReadyInstantiateTest>())
                                {
                                    Debug.LogError("ReadyObject are going to be destroied");
                                }

                                PhotonNetwork.Destroy(obj[i]);
                            }
                        }
                    }
                    else
                    {
                        if(!obj[i].CompareTag("MainObject") && obj[i].name != "PhotonMono" && obj[i].name != "Main Move Camera")
                        {
                            Destroy(obj[i]);
                        }
                    }
                }
            }
            
            Debug.LogError("RPC DestroyStart Executed?");
            PV.RPC("DestroyStartRPC", RpcTarget.Others);
            Debug.LogError("Yes");
        }
        else
        {
            while(wait)
            {
                Debug.LogError("Others Waitinggggg since wait is " + wait);
                yield return null;
            }
            Debug.LogError("Others Destroy Start");
            GameObject[] obj = GameObject.FindObjectsOfType<GameObject>();
            for(int i = 0; i < obj.Length; i++)
            {
                if (obj[i].transform.parent) continue;
                Debug.LogError(obj[i].name);
                if(obj[i].TryGetComponent<PhotonView>(out PhotonView objPV))
                {
                    if(objPV.AmOwner)
                    {
                        if(!obj[i].CompareTag("MainObject") && obj[i].name != "GameManager" && !obj[i].CompareTag("Player"))
                        {
                            if(obj[i].GetComponent<ReadyInstantiateTest>())
                            {
                                Debug.LogError("ReadyObject are going to be destroied");
                            }

                            PhotonNetwork.Destroy(obj[i]);
                        }
                    }
                }
                else if(!obj[i].CompareTag("MainObject") && obj[i].name != "PhotonMono" && obj[i].name != "Main Move Camera")
                {
                    Destroy(obj[i]);
                }
            }
            PV.RPC("DestroyStartRPC", RpcTarget.MasterClient);
        }
        while(wait)
        {
            Debug.LogError("Master Waitinggggg since wait is " + wait);
            yield return null;
        }
        
        
        Debug.LogError("Object Destroy Finished");

        // PhotonDestroyObject(ref obj);
        // DestroyObject(ref obj);

        // if(PhotonNetwork.IsMasterClient)
        // {
        //     for(int i = 0; i < obj.Length; i++)
        //     {
        //         if(obj[i].transform.parent) continue;
        //         if(!obj[i].CompareTag("MainObject") && obj[i].name != "PhotonMono" && obj[i].name != "GameManager" && !obj[i].CompareTag("Player"))
        //         {
        //             if(obj[i].TryGetComponent<PhotonView>(out PhotonView objPV))
        //                 PhotonNetwork.Destroy(obj[i]);
        //             else
        //                 Destroy(obj[i]);
        //         }
        //     }
        //     PV.RPC("gogo", RpcTarget.All);
        // }
        // else
        // {
        //     while(!go)
        //     {
        //         yield return new WaitForSeconds(0.1f);
        //     }
        //     print("Out go");
        //     for(int i = 0; i < obj.Length; i++)
        //     {
        //         if(obj[i].transform.parent) continue;
        //         if(!obj[i].CompareTag("MainObject") && obj[i].name != "PhotonMono" && obj[i].name != "GameManager" && !obj[i].CompareTag("Player"))
        //         {
        //             Destroy(obj[i]);
        //         }
        //     }
        //     PV.RPC("gogo2", RpcTarget.All);
        // }
        // while(!go2)
        // {
        //     print("go2go2");
        //     yield return new WaitForSeconds(0.1f);
        // }
        // print("Out go2");

        GameObject delteCamera = GameObject.Find("Main Move Camera");
        Destroy(delteCamera);

        PhotonNetwork.LoadLevel("Ready");
        float esctime = 3f;
        while(PhotonNetwork.LevelLoadingProgress != 1f) 
        {
            esctime -= Time.deltaTime;
            if(esctime < 0f)
            {
                Debug.Log("Destory Done, esctime < 0");
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        while(true)
        {
            Debug.Log("while loop...");
            if(GameManagerScript.instance.isReadyObjDestroyed)
            {
                Debug.Log("is ready Object destroyed 통과!");
                Debug.LogError("Is there a ready object? : " + (bool)GameObject.Find(GameManagerScript.instance.readyObjPref.name + "(Clone)"));
                Debug.LogError("Is there GMS.instance.readyObj? : " + (bool)GameManagerScript.instance.readyObj);
                if((bool)GameManagerScript.instance.readyObj) Debug.LogError("ReadyObj Destroy Failed");
                if(PhotonNetwork.IsMasterClient) GameManagerScript.instance.readyObj = PhotonNetwork.Instantiate(GameManagerScript.instance.readyObjPref.name, Vector3.zero, Quaternion.identity);
                // else
                // {
                //     GameObject robj = GameObject.Find(GameManagerScript.instance.readyObjPref.name + "(Clone)");
                //     while(!robj)
                //     {
                //         robj = GameObject.Find(GameManagerScript.instance.readyObjPref.name + "(Clone)");
                //         yield return new WaitForSeconds(0.1f);
                //     }
                //     GameManagerScript.instance.readyObj = robj;
                // }
                
                GameObject robj = GameObject.Find(GameManagerScript.instance.readyObjPref.name + "(Clone)");
                while(!robj)
                {
                    robj = GameObject.Find(GameManagerScript.instance.readyObjPref.name + "(Clone)");
                    yield return new WaitForSeconds(0.1f);
                }
                GameManagerScript.instance.readyObj = robj;

                CharacterInfoSetting pInfo = GameManagerScript.instance.getMyPlayerInfo();
                pInfo.gameObject.transform.position = Vector2.zero;
                pInfo.currentSceneName = "Ready";
                GameManagerScript.instance.ResetValues();
                GameManagerScript.instance.setGameStart();
                GameManagerScript.instance.StartAll();
                pInfo.restart();

                while(!Camera.main)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                Camera.main.GetComponent<CameraPosition>().cameraSetting("Ready");
                GameManagerScript.instance.updatePause = false;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isGameEnd);
        }
        else
        {
            isGameEnd = (bool)stream.ReceiveNext();
        }
    }
}
