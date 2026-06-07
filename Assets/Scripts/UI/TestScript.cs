using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestScript : MonoBehaviour
{
    GameObject[] players; GameObject me;
    public void Die()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++) if(players[i].GetComponent<PhotonView>().IsMine) me = players[i];
        //me.GetComponent<CharacterInfoSetting>().dodie();
    }
}
