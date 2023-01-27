using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticString : MonoBehaviour
{
    public static string baseurl = "https://ludotalents.in/admin/api/";
    public static string localaddress = "https://ludotalents.in/admin/";
    public static string shareLink = "https://ludotalents.in/";


    //firebase Storage key 
    public static string storageKey = "gs://ludotalents-33998.appspot.com";


    public static string appName = "Ludo Talents";
    public static string ruppeSymbol = "₹";


    public static string bid = baseurl+ "bids"; 
    public static string balance = baseurl + "balance";
    public static string gameEnd = baseurl + "game?id=";
    public static string loginurl = baseurl + "getUser";
    public static string checkUser = baseurl + "checkuser";
    public static string gameEvent = baseurl+ "show_events";
    public static string addUpiDetails = baseurl + "addupi";
    public static string checkProfiles = baseurl + "profile";
    public static string winnerprizeurl = baseurl + "winuser";
    public static string bankOrUpi = baseurl + "getBankOrUpi";
    public static string prizePools = baseurl + "show_prize_pool";
    public static string addBankDetails = baseurl + "addbankdetails";
    public static string withdrawRequest = localaddress + "withdraw.php";
    public static string transationHistory = baseurl + "transaction_history";
    public static string paymentgateway = localaddress + "game/Payments/checkout.php";
 
    public static string ReferRedem = baseurl + "reffer";
    public static string version = baseurl + "version";
    public static string ContactUs = "";
    public static string Whatsapphelp;

    // Services configration IDS
    public static string PlayFabTitleID = "DAFD2";
    public static string PhotonAppID = "a87483f8-25b3-4723-9f73-fb88ef9a799e";

    //share code
    public static string SharePrivateLinkMessage = $"Hi, I am using {appName} gaming APP and earning ₹1000 rupees daily by playing online {appName} game. \n\nJoin {appName} using my Referral Code: ";
    public static string SharePrivateLinkMessage2 = $"Download {appName}  from:";
    public static string ShareScreenShotText = $"I finished game in {appName}. It's my score :-) Join me and download {appName}:";
    // Game configuration

    public static float WaitTimeUntilStartWithBots = 1.0f;
    // Time in seconds. If after that time new player doesnt join room game will start with bots
    public static float photonDisconnectTimeout = 900.0f;
    // In game scene - its better to don't change it. Player that loose focus on app will be immediately disconnected
    public static float photonDisconnectTimeoutLong = 900.0f;
    // Time in seconds. If after that time new player doesnt join room game will start with bots

    public static bool isFourPlayerModeEnabled = true;
    public static string SoundsKey = "EnableSounds";
    public static string VibrationsKey = "EnableVibrations";
    public static string waitingForOpponent = "Waiting for your opponent";

    public static string[] BotNames = new string[] {
        "Deep",
        "Sai",
        "Raj",
        "Naman",
        "Nandu",
        "Mahaveer",
        "Mandeep",
        "Madhu",
        "Manas",
        "Riyan",
        "Kanha",
        "Hiralal",
        "Hariprasad",
        "Tanvir",
        "Bhagat",
        "Om",
        "Rishi",
        "Vishnu",
        "Loknath",
        "Haricharan",
        "Anjan",
        "Ekta",
        "Niraj",
        "Pooja",
        "Tanvi",
        "Priya",
        "Panini",
        "Ganga",
        "Patil",
        "Sharma",
        "Shaikh",
        "Pathan",
        "Mahi",
        "Yadav",
        "Charan",
        "Ram",
        "Laxman",
        "Allah",
        "Dev",
        "Smith",
        "Kumar",
        "Nandini",
        "Brijesh",
        "Nakul",
        "Tilak",
    };
}
