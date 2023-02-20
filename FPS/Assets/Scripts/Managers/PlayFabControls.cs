using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFabControls : MonoBehaviour
{
    [SerializeField] GameObject signUpTab, logInTab, startPanel, HUD;
    public TMP_InputField username, userEmail, userPassword, userEmailLogIn, userPasswordLogIn;
    public TMP_Text ErrorSignUp, ErrorLogIn;
    public GameObject rowPrefab;
    public Transform rowsParent;

    public void SignUpTab(){
        signUpTab.SetActive(true);
        logInTab.SetActive(false);
        ErrorSignUp.text = "";
        ErrorLogIn.text = "";
    }

    public void LogInTab(){
        signUpTab.SetActive(false);
        logInTab.SetActive(true);
        ErrorSignUp.text = "";
        ErrorLogIn.text = "";
    }

    public void SignUp(){
        var registerRequest = new RegisterPlayFabUserRequest{
            Email = userEmail.text,
            Password = userPassword.text,
            Username = username.text,            
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterError);
    }

    public void RegisterSuccess(RegisterPlayFabUserResult result){
        ErrorSignUp.text = "";
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
        Data = new Dictionary<string, string>() {
            {"Kills", "0"},
            {"Deaths", "0"}
        }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data");
            Debug.Log(error.GenerateErrorReport());
        });
        StartGame();
    }

    public void RegisterError(PlayFabError error){
        ErrorSignUp.text = error.GenerateErrorReport();
        
    }

    public void LogIn(){
        var request = new LoginWithEmailAddressRequest{
            Email = userEmailLogIn.text,
            Password = userPasswordLogIn.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, LogInSuccess, LogInError);
    }

    public void LogInSuccess(LoginResult result){
        ErrorSignUp.text = "";
        StartGame();
    }

    public void LogInError(PlayFabError error){
        ErrorLogIn.text = error.GenerateErrorReport();
        
    }

    void StartGame(){
        startPanel.SetActive(false);
        HUD.SetActive(true);
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

    public void onError(PlayFabError error){
        Debug.Log(error.GenerateErrorReport());
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result){
        Debug.Log("Successful leaderboard update");
    }

    public void GetLeaderboard(){
        var request = new GetLeaderboardRequest{
            StatisticName = "Kills",
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, onError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result){
        Debug.Log("Successful Get");
        // foreach(Transform item in rowsParent){
        //     Destroy(item.gameObject);
        // }
        foreach(var item in result.Leaderboard){
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            TMP_Text[] texts = newGo.GetComponentsInChildren<TMP_Text>();
            texts[0].text = (item.Position +1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
            Debug.Log("Good");
        }
    }
}

