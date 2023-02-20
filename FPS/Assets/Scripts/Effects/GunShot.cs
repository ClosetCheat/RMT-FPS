using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunShot :  MonoBehaviourPunCallbacks
{
    public AudioClip footesteps;
    private float lasttime = -50.0f;
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && (Time.time - lasttime > 1.0f)){
            photonView.RPC("Gunshot", RpcTarget.All);
            lasttime = Time.time;
        }
        
    }

[PunRPC]
public void Gunshot(){
    AudioSource audioRPC = gameObject.AddComponent<AudioSource> ();
    audioRPC.spatialBlend = 1;
    audioRPC.minDistance = 25;
    audioRPC.maxDistance = 100;
    audioRPC.PlayOneShot(footesteps);
}

}
