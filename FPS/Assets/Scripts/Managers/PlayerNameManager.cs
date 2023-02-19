using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;

    void Start()
    {
        if(PlayerPrefs.HasKey("username"))
        {
            usernameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        {
            usernameInput.text = "Player " + Random.Range(0, 10000).ToString("0000");
            OnUsernameInputValueChanged();
        }
    }

    public void OnUsernameInputValueChanged()
    {
        PhotonNetwork.NickName = usernameInput.text;
        PlayerPrefs.SetString("username", usernameInput.text);
        var request = new UpdateUserTitleDisplayNameRequest{
            DisplayName = usernameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, onDisplayNameUpdate, onError);
    }

    void onDisplayNameUpdate(UpdateUserTitleDisplayNameResult result){
        Debug.Log("Dispaly name updated");
    }

    void onError(PlayFabError error){
        Debug.Log(error.GenerateErrorReport());

    }
}
