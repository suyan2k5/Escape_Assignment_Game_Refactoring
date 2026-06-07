using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerMulti : MonoBehaviourPunCallbacks, IPunObservable
{
    //PlayerMulti.cs
    #region Info Sync
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NickNameText;
    #endregion

    // PlayerMove.cs
    public Vector2 stickDir;
    public float moveSpeed;
    // public PhotonView PV;
    Vector2 curPos;
    public int customAnimNum = 0;

    // Multi.cs
    void Awake()
    {
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NickNameText.color = PV.IsMine ? Color.green : Color.white;
    }

    // Move.cs
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

    // Multi.cs
    [PunRPC]
    public void isWalk(bool _isWalk)
    {
        Animator animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        Animator animatorbtm = gameObject.transform.GetChild(2).GetComponent<Animator>();
        animator.SetBool("isWalk", _isWalk);
        animator.SetBool("isWidePantsIdle", false);
        animator.SetBool("isJeanIdle", false);
        animator.SetBool("isSuitIdle", false);
    }

    [PunRPC]
    public void isWidePantsWalk(bool _isWalk)
    {
        Animator animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        Animator animatorbtm = gameObject.transform.GetChild(2).GetComponent<Animator>();
        animator.SetBool("isWidePantsIdle", _isWalk);
        animator.SetBool("isWalk", false);
        animator.SetBool("isJeanIdle", false);
        animator.SetBool("isSuitIdle", false);
        if(_isWalk) gameObject.transform.GetChild(5).gameObject.SetActive(false);
    }

    [PunRPC]
    public void isSuitWalk(bool _isWalk)
    {
        Animator animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        //Animator animatorbtm = gameObject.transform.GetChild(2).GetComponent<Animator>();
        animator.SetBool("isSuitIdle", _isWalk);
        animator.SetBool("isWidePantsIdle", false);
        animator.SetBool("isWalk", false);
        animator.SetBool("isJeanIdle", false);
        if(_isWalk) gameObject.transform.GetChild(5).gameObject.SetActive(false);
        //if(animatorbtm.runtimeAnimatorController != null) animatorbtm.SetBool("isWalk", _isWalk);
    }

    [PunRPC]
    public void isJeanWalk(bool _isWalk)
    {
        Animator animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        //Animator animatorbtm = gameObject.transform.GetChild(2).GetComponent<Animator>();
        animator.SetBool("isJeanIdle", _isWalk);
        animator.SetBool("isWidePantsIdle", false);
        animator.SetBool("isWalk", false);
        animator.SetBool("isSuitIdle", false);
        if(_isWalk) gameObject.transform.GetChild(5).gameObject.SetActive(false);
        //if(animatorbtm.runtimeAnimatorController != null) animatorbtm.SetBool("isWalk", _isWalk);
    }
    
    [PunRPC]
    public void isRight(bool _isRight)
    {
        SpriteRenderer renderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer renderer1 = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        SpriteRenderer renderer2 = gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>();

        renderer.flipX = _isRight;
        renderer1.flipX = _isRight;
        renderer2.flipX = _isRight;
    }

    // Move.cs
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
