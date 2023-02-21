using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class footsteps :  MonoBehaviourPunCallbacks
{
    public AudioClip[] sound;
    private float lasttime = -50.0f;
    void Update()
    {
        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && (Time.time - lasttime > 0.3f) ){
            photonView.RPC("Footsteps", RpcTarget.All);
            lasttime = Time.time;
        }
    }

[PunRPC]
public void Footsteps(){
    AudioSource audioRPC = gameObject.AddComponent<AudioSource> ();
    audioRPC.spatialBlend = 1;
    audioRPC.minDistance = 1;
    audioRPC.maxDistance = 25;
    audioRPC.clip = sound[Random.Range(0, sound.Length)];
    audioRPC.PlayOneShot(audioRPC.clip);
}

}
