using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class footsteps :  MonoBehaviourPunCallbacks
{
    public AudioClip footesteps;
    private float lasttime = -50.0f;
    void Update()
    {
        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && (Time.time - lasttime > 1.0f) ){
            photonView.RPC("Footsteps", RpcTarget.All);
            lasttime = Time.time;
        }
    }

[PunRPC]
public void Footsteps(){
    AudioSource audioRPC = gameObject.AddComponent<AudioSource> ();
    audioRPC.spatialBlend = 1;
    audioRPC.minDistance = 25;
    audioRPC.maxDistance = 100;
    audioRPC.PlayOneShot(footesteps);
}

}
