using UnityEngine;
using UnityEngine.UI;

public class GameDiceController : MonoBehaviour
{

    public Sprite[] diceValueSprites;
    public GameObject arrowObject;
    public GameObject diceValueObject;
    public GameObject diceAnim;
    public GameObject ligths;

    // Use this for initialization
    public bool isMyDice = false;
    public GameObject LudoController;
    public LudoGameController controller;
    public int player = 1;
    private Button button;
    public GameObject notInteractable;

    private int steps = 0;
  
    void Start()
    {
        button = GetComponent<Button>();
        controller = LudoController.GetComponent<LudoGameController>();

        button.interactable = false;
    }

    public void SetDiceValue()
    {
        Debug.Log("Set dice value called");
        diceValueObject.GetComponent<Image>().sprite = diceValueSprites[steps - 1];
        diceValueObject.SetActive(true);
        diceAnim.SetActive(false);
        controller.gUIController.restartTimer();
        if (isMyDice)
            controller.HighlightPawnsToMove(player, steps);
        if (GameManager.Instance.currentPlayer.isBot)
        {
            controller.HighlightPawnsToMove(player, steps);
        }

    }

    public void EnableShot()
    {
        if (GameManager.Instance.currentPlayer.isBot)
        {
            GameManager.Instance.miniGame.BotTurn(false);
            notInteractable.SetActive(false);
        }
        else
        {
            if (PlayerPrefs.GetInt(StaticString.VibrationsKey, 0) == 0)
            {
                Debug.Log("Vibrate");
#if UNITY_ANDROID || UNITY_IOS
                Handheld.Vibrate();
#endif
            }
            else
            {
                Debug.Log("Vibrations OFF");
            }
            diceValueObject.SetActive(true);
            controller.gUIController.myTurnSource.Play();
            notInteractable.SetActive(false);
            button.interactable = true;
            arrowObject.SetActive(true);
            
        }
    }

    public void DisableShot()
    {
        diceValueObject.SetActive(false);
        notInteractable.SetActive(true);
        button.interactable = false;
        arrowObject.SetActive(false);
    }

    public void EnableDiceShadow()
    {
        ligths.SetActive(false);
        notInteractable.SetActive(true);
        diceValueObject.SetActive(false);
    }

    public void DisableDiceShadow()
    {
        ligths.SetActive(true);
        notInteractable.SetActive(false);
    }
    int aa = 0;
    int bb = 0;
    
    public void RollDice()
    {
        if (isMyDice)
        {

            controller.nextShotPossible = false;
            controller.gUIController.PauseTimers();
            button.interactable = false;
            Debug.Log("Roll Dice");
            arrowObject.SetActive(false);
            steps = Random.Range(1, 7);
            RollDiceStart(steps);
            string data = steps + ";" + controller.gUIController.GetCurrentPlayerIndex();
           
            PhotonNetwork.RaiseEvent((int)EnumGame.DiceRoll, data, true, null);
            
            Debug.Log(data);
            Debug.Log("Value: " + steps);
        }
    }

    public void RollDiceBot(int value)
    {       

        controller.nextShotPossible = false;
        controller.gUIController.PauseTimers();

        Debug.Log("Roll Dice bot");

        // if (bb % 2 == 0) steps = 6;
        // else steps = 2;
        // bb++;

        steps = value;

        RollDiceStart(steps);


    }

    public void RollDiceStart(int steps)
    {
        GetComponent<AudioSource>().Play();
        this.steps = steps;
        diceValueObject.SetActive(false);
        diceAnim.SetActive(true);
        diceAnim.GetComponent<Animator>().Play("RollDiceAnimation");
    }
}
