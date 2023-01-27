using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MyPlayerData
{

    public static string AvatarIndexKey = "AvatarIndex";
    public static string PlayerName = "PlayerName";
    public static string FortuneWheelLastFreeKey = "FortuneWheelLastFreeTime";
    public static string TitleFirstLoginKey = "TitleFirstLogin";
    public static string TotalPlayerWinsKey = "TotalPlayerWinsKey";
    public static string GamesPlayedKey = "GamesPlayed";
    public static string TwoPlayerWinsKey = "TwoPlayerWins";
    public static string FourPlayerWinsKey = "FourPlayerWins";


    public Dictionary<string, UserDataRecord> data;

   

    public int GetTotalPlayerWinsKey()
    {
        return int.Parse(this.data[TotalPlayerWinsKey].Value);
    }

    public int GetTwoPlayerWins()
    {
        return int.Parse(this.data[TwoPlayerWinsKey].Value);
    }

    public int GetFourPlayerWins()
    {
        return int.Parse(this.data[FourPlayerWinsKey].Value);
    }

    public int GetPlayedGamesCount()
    {
        if (this.data != null)
            return int.Parse(this.data[GamesPlayedKey].Value);
        return -1;
    }

    public string GetAvatarIndex()
    {
        return this.data[AvatarIndexKey].Value;
    }
  
    public string GetPlayerName()
    {
        if (this.data.ContainsKey(PlayerName))
            return this.data[PlayerName].Value;
        else return "Error";
    }

    public string GetLastFortuneTime()
    {
        if (this.data.ContainsKey(FortuneWheelLastFreeKey))
        {
            return this.data[FortuneWheelLastFreeKey].Value;

        }
        else
        {
            string date = DateTime.Now.Ticks.ToString();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(FortuneWheelLastFreeKey, date);
            UpdateUserData(data);
            return date;
        }
    }
    public MyPlayerData() { }
    public MyPlayerData(Dictionary<string, UserDataRecord> data, bool myData)
    {
        this.data = data;

        GameManager.Instance.nameMy = GetPlayerName();
        
        GameManager.Instance.avatarMy = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().avatars[PlayerPrefs.GetInt("ImageIndex")];
        Debug.Log("MY DATA LOADED");
    }
    public void UpdateUserData(Dictionary<string, string> data)
    {

        if (this.data != null)
            foreach (var item in data)
            {
                Debug.Log("SAVE: " + item.Key);
                if (this.data.ContainsKey(item.Key))
                {
                    Debug.Log("AA");
                    this.data[item.Key].Value = item.Value;
                }
            }

        UpdateUserDataRequest userDataRequest = new UpdateUserDataRequest()
        {
            Data = data,
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(userDataRequest, (result1) =>
        {
            Debug.Log("Data updated successfull ");

        }, (error1) =>
        {
            Debug.Log("Data updated error " + error1.ErrorMessage);
        }, null);

    }

    public static Dictionary<string, string> InitialUserData()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(AvatarIndexKey, PlayerPrefs.GetInt("ImageIndex").ToString());
        data.Add(TotalPlayerWinsKey, "0"); 
        data.Add(GamesPlayedKey, "0");
        data.Add(TwoPlayerWinsKey, "0");
        data.Add(FourPlayerWinsKey, "0");
        data.Add(TitleFirstLoginKey, "1");
        data.Add(FortuneWheelLastFreeKey, DateTime.Now.Ticks.ToString());
        return data;
    }


}