using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DoorCollide : MonoBehaviourPunCallbacks
{
    // 문 UI와 접촉 시 씬 이동 스크립트
    private SceneChanger sceneChanger;

    Canvas canvas;

    private void Start() 
    {
        sceneChanger = GetComponent<SceneChanger>();

        canvas = GetComponentInParent<Canvas>();

        // canvas.worldCamera = GameObject.Find("Main Move Camera").GetComponent<Camera>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterInfoSetting info = collision.gameObject.GetComponent<CharacterInfoSetting>();
        Debug.Log("Collide Occur");
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            sceneChanger.changeScene();
        }
    }
}
