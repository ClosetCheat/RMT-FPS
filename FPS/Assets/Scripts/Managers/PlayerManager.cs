using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System.IO;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using PlayFab;
using PlayFab.ClientModels;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    GameObject controller;

    public PlayFabControls controls;

    public int kills;
    public int deaths;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if(PV.IsMine){
            CreateController();
        }   
    }

    void CreateController(){
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[]{PV.ViewID});
    }

    public void onError(PlayFabError error){
        Debug.Log(error.GenerateErrorReport());
    }


    public void Die(){
        PhotonNetwork.Destroy(controller);
        CreateController();
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), onSuccessDeath, onError);
        deaths++;

        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    void onSuccessDeath(GetUserDataResult result){
        if(result.Data != null && result.Data.ContainsKey("Kills") && result.Data.ContainsKey("Deaths")){
            string Kills = result.Data["Kills"].Value;
            string Deaths = result.Data["Deaths"].Value;
            int numOfDeaths = int.Parse(Deaths);
            numOfDeaths++;
            Deaths = numOfDeaths.ToString();
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>() {
                {"Kills", Kills},
                {"Deaths", Deaths}
            }},
            result => Debug.Log("Successfully updated user data"),
            error => {
                Debug.Log("Got error setting user data Ancestor to Arthur");
                Debug.Log(error.GenerateErrorReport());
            });
            Debug.Log("Success");
        } else {
            Debug.Log("Error");
        }
    }

    public void GetKill()
    {
        PV.RPC(nameof(RPC_GetKill), PV.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), onSuccessKill, onError);
        kills++;

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void SendLeaderboard(int score){
        var request = new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate>{
                new StatisticUpdate{
                    StatisticName = "Kills",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, onError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result){
        Debug.Log("Successful leaderboard update");
    }

    void onSuccessKill(GetUserDataResult result){
        if(result.Data != null && result.Data.ContainsKey("Kills") && result.Data.ContainsKey("Deaths")){
            string Kills = result.Data["Kills"].Value;
            string Deaths = result.Data["Deaths"].Value;
            int numOfKills = int.Parse(Kills);
            numOfKills++;
            Kills = numOfKills.ToString();
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>() {
                {"Kills", Kills},
                {"Deaths", Deaths}
            }},
            result =>{Debug.Log("Successfully updated user data");
                        SendLeaderboard(int.Parse(Kills));
                        Debug.Log("Successfully updated user data");}, 
            error => {
                Debug.Log("Got error setting user data Ancestor to Arthur");
                Debug.Log(error.GenerateErrorReport());
            });
            
            Debug.Log("Success");
        } else {
            Debug.Log("Error");
        }
    }

    



    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }
}
