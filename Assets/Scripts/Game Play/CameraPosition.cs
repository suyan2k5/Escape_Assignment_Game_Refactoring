using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraPosition : MonoBehaviour
{
    /*
    update 활용?

    기본적으로는 캐릭터 위치로 카메라 이동. 그러나 게임 맵으로 이동하면,

    방 이름으로 배경 이미지 찾기
    배경의 world position을 중심으로 설정
    1920px * 1024px
    cpx = Clamp(cpx, mwpx-camerax/2, mwpx+camerax/2)
    cpy = Clamp(cpy, mwpy-cameray/2, mwpx+cameray/2)
    */

    bool cameraGo = false;
    string roomName;
    GameObject player;
    Vector3 cp;
    Vector3 mp;

    private void Update()
    {
        if(cameraGo) cameraMove();
    }

    public void cameraSetting(string _roomName)
    {
        roomName = _roomName;

        player = GameManagerScript.instance.getMyPlayer();

        cameraGo = true;

        print("Camera Set");
    }

    public void cameraMove()
    {
        cp = player.transform.position;

        GameObject bgimg = GameObject.Find(roomName + "Background/BackgroundImage");
        if(bgimg == null)
        {
            cp.z = -100;
            transform.position = cp;
            return;
        }
        // print("A Background exits.");

        GameObject bgParent = bgimg.transform.parent.gameObject;
        
        mp = bgimg.transform.position;

        Vector2 mapSize, cameraSize;

        mapSize = (Vector2)bgimg.GetComponent<SpriteRenderer>().sprite.bounds.size / 2 * bgParent.transform.localScale * bgimg.transform.localScale;
        cameraSize = (new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize));

        if(bgParent.transform.eulerAngles.z == 90 || bgParent.transform.eulerAngles.z == 270)
        {
            mapSize = new Vector2(mapSize.y, mapSize.x);
        }

        if(mapSize.x < cameraSize.x)
        {
            cp.x = bgimg.transform.position.x;
        }
        else
        {
            cp.x = Mathf.Clamp(cp.x, mp.x - mapSize.x + cameraSize.x, mp.x + mapSize.x - cameraSize.x);
        }
        if(mapSize.y < cameraSize.y)
        {
            cp.y = bgimg.transform.position.y;
        }
        else
        {
            cp.y = Mathf.Clamp(cp.y, mp.y - mapSize.y + cameraSize.y, mp.y + mapSize.y - cameraSize.y);
        }

        cp.z = -100;

        // transform.position = Vector3.Lerp(transform.position, cp, Time.deltaTime);
        transform.position = cp;
    }
}
