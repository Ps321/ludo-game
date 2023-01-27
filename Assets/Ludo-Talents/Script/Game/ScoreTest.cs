using UnityEngine;
[System.Serializable]
public struct ScoreTest
{

    public int playerScoreText;
    public GameObject playername;
    public string playFabID;

    public ScoreTest(int playerScoreText ,GameObject  playerNames, string playFabID)
    {
        this.playerScoreText = playerScoreText;
        this.playername = playerNames;
        this.playFabID = playFabID;
    }

}
