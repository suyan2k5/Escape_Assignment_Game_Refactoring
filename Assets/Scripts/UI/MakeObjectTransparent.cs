using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObjectTransparent : MonoBehaviour
{

    //오브젝트 투명하게 만드는 스크립트.
    public GameObject Object;
    new private Renderer renderer;
    private Color color;
    
    private void Start() 
    {
        if(Object != null)
        {
            renderer = Object.transform.GetChild(0).GetComponent<Renderer>();
            Debug.Log(Object.transform.GetChild(0).name);
        }
    }

    //어디에 쓰이는거지
    public void MakeTransparent()
    {
        if(renderer != null)
        {
            color = renderer.material.color;
            renderer.material.color = Color.clear;
        }
    }

    public void MakeNotTransparent()
    {
        if(renderer != null)
        {
            renderer.material.color = color;
        }
    }
}
