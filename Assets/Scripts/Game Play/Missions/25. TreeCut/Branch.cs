using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public bool isChoose = false;
    public bool isCut = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ground"))
        {
            Destroy(GetComponent<Rigidbody2D>());
        }
    }
}
