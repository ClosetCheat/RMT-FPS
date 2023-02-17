using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleScene : MonoBehaviour
{
    public AudioSource BackgroundAudio;
    void OnEnable()
    {
        MenuManager.Instance.OpenMenu("title");
        BackgroundAudio.GetComponent<AudioSource>();
        BackgroundAudio.mute = !BackgroundAudio.mute;
        
    }
}
