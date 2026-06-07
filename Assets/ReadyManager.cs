using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ReadyManager : MonoBehaviour
{
    [SerializeField] private Text countdownText;
    [SerializeField] private Text announceScene;
    public PhotonView PV;

    public void GameStartClick()
    {
        // Game Start
        PV.RPC("WaitBeforeGameStartRPC", RpcTarget.AllBuffered);
        GameManagerScript.instance.OnlyForMaster();
    }

    [PunRPC]
    void WaitBeforeGameStartRPC()
    {
        StartCoroutine(WaitBeforeGameStart());
    }

    IEnumerator WaitBeforeGameStart()
    {
        // Wait for 5 seconds
        countdownText.gameObject.SetActive(true);
        float time = 5.0f;
        while(time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(time).ToString();
        }

        // count off and ready to enter spawn scene
        countdownText.gameObject.SetActive(false);
        announceScene.gameObject.SetActive(true);

        GameManagerScript.instance.GameStart();
        while(GameManagerScript.instance.getMyPlayerInfo().currentSceneName.CompareTo("Spawn") != 0)
        {
            yield return null;
        }
        announceScene.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
