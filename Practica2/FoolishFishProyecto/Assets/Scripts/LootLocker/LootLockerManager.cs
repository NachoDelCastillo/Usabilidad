using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using LootLocker;

public class LootLockerManager : MonoBehaviour{
    public Leaderboard leaderboard;
    public TMP_InputField playerNameInputField;

    
    private void Start(){
        //LootLockerSettingsOverrider.OverrideSettings();
        StartCoroutine(SetUpRoutine());      
    }
    IEnumerator SetUpRoutine(){
        yield return LoginRoutine();
        yield return leaderboard.FetchTopHighscoresRoutine();
        yield return SetName();
    }
    private IEnumerator SetName(){
        bool done = false;
        LootLockerSDKManager.GetPlayerName((response) =>{
            if (response.success){
                Debug.Log("NAME WAS CORRECTLY PUT");
                playerNameInputField.text = response.name;               
                done = true;
            }
            else{
                Debug.LogError("Could not set Name in inputfiled");
            }
        });
       yield return new WaitWhile(() => done == false);
    }
    IEnumerator LoginRoutine(){
        bool done = false;
        //PlayerPrefs.DeleteKey("LootLockerGuestPlayerID");
        LootLockerSDKManager.StartGuestSession((response) =>{
            if (response.success){
                Debug.Log("Player was logged in");
                
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else{
                Debug.LogError("Could not start session");
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    public void setPlayerName(){      
        LootLockerSDKManager.SetPlayerName(playerNameInputField.text, (response) =>{
            if (response.success){
                Debug.Log("Succesfully set player name");
                StartCoroutine(leaderboard.FetchTopHighscoresRoutine());
            }
            else{
                Debug.LogError("Could not set player name" +response.Error);
            }
        });
    }
}
