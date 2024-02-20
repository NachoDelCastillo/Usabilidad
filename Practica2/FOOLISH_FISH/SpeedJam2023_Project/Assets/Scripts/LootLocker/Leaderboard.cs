using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.SocialPlatforms;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

public class Leaderboard : MonoBehaviour{
   [SerializeField]string nameLoaderBoard = "fishfulishsimulator";
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerScores;
    List<int> ranking= new List<int>();
    void Start(){
        
    }
    /// <summary>
    /// ACTUALIZAR LEADERBOARD CUANDO SE APSA EL NIVEL
    /// </summary>
    /// <param name="scoreToUpload"></param>
    /// <returns></returns>
    public IEnumerator SubmitScoreRoutine(int scoreToUpload){
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, nameLoaderBoard, (response) =>
        {
            if (response.success){
                Debug.Log("Successfully uploaded score");
                //FetchTopHighscoresRoutine();
                done = true;
            }
            else
            {
                Debug.LogWarning("Failed to submit score to " + nameLoaderBoard + ": " + response.Error);
                done = true;
            }
        });
        yield return FetchTopHighscoresRoutine();
    }

    public IEnumerator FetchTopHighscoresRoutine(){
        bool done = false;
        LootLockerSDKManager.GetScoreList(nameLoaderBoard, 10, (response) =>
        {
            int x = 0;
            if (response.success){
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";
                string aux="";
                LootLockerLeaderboardMember[] members = response.items;
                for (int i = 0; i < members.Length; i++){
                    tempPlayerNames += members[i].rank + ".  ";
                    if (i < 10)
                    {
                        if (members[i].player.name != "")
                        {
                            if (members[i].player.name.Length > 10) { aux = "\n"; x++; }
                            tempPlayerNames += members[i].player.name;

                        }
                        else
                        {
                            tempPlayerNames += members[i].player.id;
                        }

                        tempPlayerScores += Timer.GetTimeString(members[i].score / 1000f) + "\n" + aux;
                        tempPlayerNames += "\n";
                    }
                    ranking.Add(members[i].score);
                }
                done = true;string espacios = "";
                for (int i = 0; i < x*2; i++) espacios += "\n";
                playerName.text=tempPlayerNames;
                playerScores.text=espacios+tempPlayerScores;
                Debug.Log("Fetched score list from board: " + nameLoaderBoard + ": " + response.statusCode);
            }
            else{
                Debug.LogWarning("Failed to get score list from board: " + nameLoaderBoard + ": " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

   public int getYourPosition(int score){
        int myScore=1;

        foreach(var num in ranking){
            if (num > score) break;           
            else myScore++;
        }
        return myScore;
    }
}
