using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ReadyInstantiateTest : MonoBehaviour, IPunInstantiateMagicCallback
{
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        GameManagerScript.instance.readyObj = gameObject;
        Debug.LogError("ReadyObj Instantiated");
    }

    void Start()
    {
        Debug.LogError("ReadyObj Called Start Method");
    }

    private void OnDestroy() {
        Debug.LogError("ReadyObj Destroied");
        GameManagerScript.instance.isReadyObjDestroyed = true;
    }
}
