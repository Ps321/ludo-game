using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;

public class LudoGameController : PunBehaviour, IMiniGame
{

    public GameObject[] dice;
    public GameObject GameGui;
    public GameGUIController gUIController;
    public GameObject[] Pawns1;
    public GameObject[] Pawns2;
    public GameObject[] Pawns3;
    public GameObject[] Pawns4;

    public GameObject gameBoard;
    public GameObject gameBoardScaler;

    [HideInInspector]
    public int steps = 5;

    public bool nextShotPossible;
    private int SixStepsCount = 0;
    public int finishedPawns = 0;
    private int botCounter = 0;
    private List<GameObject> botPawns;
    private static object instance;

    private LudoGameController() { }

   public static LudoGameController Instance
    {
        get
        {
            if ( instance==null)
            {
                instance = new LudoGameController();

            }
            return (LudoGameController)instance;
        }
    }

    public void HighlightPawnsToMove(int player, int steps)
    {

        botPawns = new List<GameObject>();

        gUIController.restartTimer();


        GameObject[] pawns = GameManager.Instance.currentPlayer.pawns;

        this.steps = steps;

        if (steps == 6)
        {

            nextShotPossible = true;
            SixStepsCount++;
            if (SixStepsCount == 3)
            {
                nextShotPossible = false;
                if (GameGui != null)
                {
                    //gUIController.SendFinishTurn();
                    Invoke("sendFinishTurnWithDelay", 1.0f);
                }

                return;
            }
        }
       else
        {
            SixStepsCount = 0;
            nextShotPossible = false;
        }

        bool movePossible = false;

        int possiblePawns = 0;
        GameObject lastPawn = null;
        for (int i = 0; i < pawns.Length; i++)
        {
            bool possible = pawns[i].GetComponent<LudoPawnController>().CheckIfCanMove(steps);
            if (possible)
            {
                lastPawn = pawns[i];
                movePossible = true;
                possiblePawns++;
                botPawns.Add(pawns[i]);
            }
        }



        if (possiblePawns == 1)
        {
            if (GameManager.Instance.currentPlayer.isBot)
            {
                StartCoroutine(movePawn(lastPawn, false));
            }
            else
            {
                lastPawn.GetComponent<LudoPawnController>().MakeMove();

            }
        }
        else
        {
            if (possiblePawns == 2 && lastPawn.GetComponent<LudoPawnController>().pawnInJoint != null)
            {
                if (GameManager.Instance.currentPlayer.isBot)
                {
                    if (!lastPawn.GetComponent<LudoPawnController>().mainInJoint)
                    {
                        StartCoroutine(movePawn(lastPawn, false));
                        Debug.Log("AAA");
                    }
                    else
                    {
                        StartCoroutine(movePawn(lastPawn.GetComponent<LudoPawnController>().pawnInJoint, false));
                        Debug.Log("BBB");
                    }

                }
                else
                {
                    if (!lastPawn.GetComponent<LudoPawnController>().mainInJoint)
                    {
                        lastPawn.GetComponent<LudoPawnController>().MakeMove();
                    }
                    else
                    {
                        lastPawn.GetComponent<LudoPawnController>().pawnInJoint.GetComponent<LudoPawnController>().MakeMove();
                    }
                }
            }
            else
            {
                if (possiblePawns > 0 && GameManager.Instance.currentPlayer.isBot)
                {
                    int bestScoreIndex = 0;
                    int bestScore = int.MinValue;
                    // Make bot move
                    for (int i = 0; i < botPawns.Count; i++)
                    {
                        int score = botPawns[i].GetComponent<LudoPawnController>().GetMoveScore(steps);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestScoreIndex = i;
                        }
                    }
                    StartCoroutine(movePawn(botPawns[bestScoreIndex], true));
                }
            }
        }

        if (!movePossible)
        {
            if (GameGui != null)
            {
                Debug.Log("game controller call finish turn");
                gUIController.PauseTimers();
                Invoke("sendFinishTurnWithDelay", 1.0f);
            }
        }
    }

    private IEnumerator MovePawnWithDelay(GameObject lastPawn)
    {
        yield return new WaitForSeconds(1.0f);

        lastPawn.GetComponent<LudoPawnController>().MakeMove();
    }

    public void sendFinishTurnWithDelay()
    {
        gUIController.SendFinishTurn();
    }

    public void Unhighlight()
    {
        for (int i = 0; i < Pawns1.Length; i++)
        {
            Pawns1[i].GetComponent<LudoPawnController>().Highlight(false);
        }

        for (int i = 0; i < Pawns2.Length; i++)
        {
            Pawns2[i].GetComponent<LudoPawnController>().Highlight(false);
        }

        for (int i = 0; i < Pawns3.Length; i++)
        {
            Pawns3[i].GetComponent<LudoPawnController>().Highlight(false);
        }

        for (int i = 0; i < Pawns4.Length; i++)
        {
            Pawns4[i].GetComponent<LudoPawnController>().Highlight(false);
        }

    }

    void IMiniGame.BotTurn(bool first)
    {
        if (first)
        {
            SixStepsCount = 0;
        }
        //   Invoke("RollDiceWithDelay", GameManager.Instance.botDelays[(botCounter + 1) % GameManager.Instance.botDelays.Count]);
        Invoke("ForBotRollDice", 2f);
        botCounter++;
        //throw new System.NotImplementedException();
    }


    public IEnumerator movePawn(GameObject pawn, bool delay)
    {
        if (delay)
        {
            //  yield return new WaitForSeconds(GameManager.Instance.botDelays[(botCounter + 1) % GameManager.Instance.botDelays.Count]);
            yield return new WaitForSeconds(1f);
            botCounter++;
        }
        pawn.GetComponent<LudoPawnController>().MakeMovePC();
    }

    public void RollDiceWithDelay()
    {
        GameManager.Instance.currentPlayer.dice.GetComponent<GameDiceController>().RollDiceBot(GameManager.Instance.botDiceValues[(botCounter + 1) % GameManager.Instance.botDelays.Count]);
    }

    public void RollDiceForPlayer()
    {
        GameManager.Instance.currentPlayer.dice.GetComponent<GameDiceController>().RollDiceBot(Random.Range(1, 5));
        Debug.Log("Auto Dice roll for Online mode");

    }
    public void ForBotRollDice()
    {
        GameManager.Instance.currentPlayer.dice.GetComponent<GameDiceController>().RollDiceBot(Random.Range(1, 7));

        Debug.Log("In loop ForBotRollDice");
    }


    void IMiniGame.CheckShot()
    {
        throw new System.NotImplementedException();
    }

    void IMiniGame.setMyTurn()
    {
        SixStepsCount = 0;
        GameManager.Instance.diceShot = false;
        dice[0].GetComponent<GameDiceController>().EnableShot();
    }

    void IMiniGame.setOpponentTurn()
    {
        SixStepsCount = 0;
        GameManager.Instance.diceShot = false;
        dice[0].GetComponent<GameDiceController>().DisableShot();
        Unhighlight();
    }


    void Awake()
    {
        GameManager.Instance.miniGame = this;
        PhotonNetwork.OnEventCall += this.OnEvent;
    }

    // Use this for initialization
    void Start()
    {
        // Scale gameboard


        float scalerWidth = gameBoardScaler.GetComponent<RectTransform>().rect.size.x;
        float boardWidth = gameBoard.GetComponent<RectTransform>().rect.size.x;

        gameBoard.GetComponent<RectTransform>().localScale = new Vector2(scalerWidth / boardWidth, scalerWidth / boardWidth);

        gUIController = GameGui.GetComponent<GameGUIController>();


    }

    void OnDestroy()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    private void OnEvent(byte eventcode, object content, int senderid)
    {
        Debug.Log("Received event Ludo: " + eventcode);

        if (eventcode == (int)EnumGame.DiceRoll)
        {

            gUIController.PauseTimers();
            string[] data = ((string)content).Split(';');
            steps = int.Parse(data[0]);
            Debug.Log("---------dicerollsteop"+steps);
            int pl = int.Parse(data[1]);
            Debug.Log("----------plworkdiceroll"+pl);
            GameManager.Instance.playerObjects[pl].dice.GetComponent<GameDiceController>().RollDiceStart(steps);
        }
        else if (eventcode == (int)EnumGame.PawnMove)
        {
            string[] data = ((string)content).Split(';');
            int index = int.Parse(data[0]);
            int pl = int.Parse(data[1]);
            Debug.Log("----------plpawnmove" + pl);
            steps = int.Parse(data[2]);
            Debug.Log("---------pawnmovesstep" + steps);

            GameManager.Instance.playerObjects[pl].pawns[index].GetComponent<LudoPawnController>().MakeMovePC();
        }
        else if (eventcode == (int)EnumGame.PawnRemove)
        {
            string data = (string)content;
            string[] messages = data.Split(';');
            int index = int.Parse(messages[1]);
            int playerIndex = int.Parse(messages[0]);
            GameManager.Instance.playerObjects[playerIndex].pawns[index].GetComponent<LudoPawnController>().GoToInitPosition(false);
        }

    }
}
