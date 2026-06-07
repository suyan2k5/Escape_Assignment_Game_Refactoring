using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SettingRole : MonoBehaviourPunCallbacks
{
    //Setting roles of players in game after change scence to Spawn

    private GameObject[] players;
    private List<CharacterInfoSetting> ps  = new List<CharacterInfoSetting>();
    private int playersNum;
    
    public void setRoles()
    {   
        players = GameObject.FindGameObjectsWithTag("Player");

        for(int i = 0; i < players.Length; i++)
        {
            ps.Add(players[i].GetComponent<CharacterInfoSetting>());
        }
        playersNum = ps.Count;
        
        if(playersNum > 0)
        {
            List<string> roles = new List<string>();
            if(playersNum == 5)
            {
                roles = shuffleRoles(3,1,1);
            }
            else if(playersNum == 6)
            {
                roles = shuffleRoles(4,1,1);
                
            }
            else if(playersNum == 7)
            {
                roles = shuffleRoles(4,1,2);
            }
            else if(playersNum == 8)
            {
                roles = shuffleRoles(5,1,2);
            }
            //for test 3 players or 2 or 1.
            //2,2,2�� �� ������ 1���� ������� �ϴ��� shuffleroles�� ����� ������ �������� ������ ���� ���ؼ�
            else if(playersNum == 2 || playersNum == 3)
            {
                roles = shuffleRoles(1,1,1);
            }
            else
            {
                roles = shuffleRoles(2,2,2);
            }
            
            for(int i = 0; i < playersNum; i++)
            {
                // ps[i].GetComponent<PhotonView>().RPC("SetRole", RpcTarget.AllBuffered, roles[i]);
                GameManagerScript.instance.setRoles(ps[i].nickName, roles[i]);
                //else break;
            }
        }
    }       

    private List<string> shuffleRoles(int stu, int grad, int prof)
    {
        List <string> roleArray = new List <string>();
        List <string> playersRoleArray = new List<string>();
        int total = stu + grad + prof;
        
        for(int i = 0; i < stu; i++)
        {
            roleArray.Add("student");
        }

        for(int i = 0; i < grad; i++)
        {
            roleArray.Add("graduate");
        }

        for(int i = 0; i < prof; i++)
        {
            roleArray.Add("professor");
        }

        
        for(int player = 0; player < total ; player ++)
        {
            int ran = Random.Range(0, roleArray.Count);
            playersRoleArray.Add(roleArray[ran]);
            roleArray.RemoveAt(ran);
        }
        
        return playersRoleArray;  //for method1
        

    }
    
}
