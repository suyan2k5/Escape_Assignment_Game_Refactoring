using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{
    public Vector2 stickDir;
    public float moveSpeed;
    public PhotonView PV;
    Vector2 curPos;
    public int customAnimNum = 0;

    private void Update()
    {
        if(PV.IsMine)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(stickDir.x, stickDir.y) * moveSpeed;

            if(stickDir == Vector2.zero)
            {
                if(customAnimNum == 0)
                {
                    PV.RPC("isWalk", RpcTarget.All, false);
                }
                else if(customAnimNum == 1)
                {
                    PV.RPC("isWidePantsWalk", RpcTarget.All, false);
                }
                else if(customAnimNum == 2)
                {
                    PV.RPC("isSuitWalk", RpcTarget.All, false);
                }
                else if(customAnimNum == 3)
                {
                    PV.RPC("isJeanWalk", RpcTarget.All, false);
                }

            } 
            else
            {
                if(customAnimNum == 0)
                {
                    PV.RPC("isWalk", RpcTarget.All, true);
                }
                else if(customAnimNum == 1)
                {
                    PV.RPC("isWidePantsWalk", RpcTarget.All, true);
                }
                else if(customAnimNum == 2)
                {
                    PV.RPC("isSuitWalk", RpcTarget.All, true);
                }
                else if(customAnimNum == 3)
                {
                    PV.RPC("isJeanWalk", RpcTarget.All, true);
                }
                //PV.RPC("isWalk", RpcTarget.AllBuffered, true);
                
                if(stickDir.x < 0)
                {
                    PV.RPC("isRight", RpcTarget.All, false);
                } 
                else
                {
                    PV.RPC("isRight", RpcTarget.All, true);
                } 
            }
        }
        else if(((Vector2)transform.position - curPos).sqrMagnitude >= 1f) transform.position = curPos;
        else transform.position = Vector2.Lerp((Vector2)transform.position, curPos, Time.deltaTime * 10f);
        // else transform.position = curPos;
        
        // other player visibility
        #region other player visible manage
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] deads = GameObject.FindGameObjectsWithTag("Dead");
        GameObject player = gameObject;
        if(!player.GetComponent<PhotonView>().IsMine) return;
        // player
        for(int i = 0; i < players.Length; i++)
        {
            if(!players[i].GetComponent<PhotonView>().IsMine)
            {
                if((players[i].GetComponent<CharacterInfoSetting>().isAlive || !(player.GetComponent<CharacterInfoSetting>().isAlive)) &&
                   (players[i].GetComponent<CharacterInfoSetting>().currentSceneName == player.GetComponent<CharacterInfoSetting>().currentSceneName))
        	    {
                    players[i].transform.Find("Image").gameObject.SetActive(true);
                    players[i].transform.Find("Canvas").gameObject.SetActive(true);
                    /*print(players[i].GetComponent<CharacterInfoSetting>().currentSceneName + " = " + player.GetComponent<CharacterInfoSetting>().currentSceneName);
                    print(players[i].GetComponent<PhotonView>().name + " True");*/
	            }
	            else
	            {
	        	    players[i].transform.Find("Image").gameObject.SetActive(false);
                    players[i].transform.Find("Canvas").gameObject.SetActive(false);
                    //print(players[i].GetComponent<PhotonView>().name + " false");
	            }
            }
        }
        // dead
        for(int i = 0; i < deads.Length; i++)
        {
            if(deads[i].GetComponent<CharacterInfoSetting>().currentSceneName == player.GetComponent<CharacterInfoSetting>().currentSceneName)
            {
                deads[i].transform.Find("Image").gameObject.SetActive(true);
                deads[i].transform.Find("Canvas").gameObject.SetActive(true);
            }
            else
            {
                deads[i].transform.Find("Image").gameObject.SetActive(false);
                deads[i].transform.Find("Canvas").gameObject.SetActive(false);
            }
        }
        #endregion
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext((Vector2)gameObject.transform.position);
        }
        else
        {
            curPos = (Vector2)stream.ReceiveNext();
        }
    }
}
