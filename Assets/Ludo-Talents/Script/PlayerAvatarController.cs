using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatarController : MonoBehaviour
{

    public GameObject Name;
    public GameObject Avatar;
    public GameObject Timer;
    public GameObject leftRoomObject;
    public GameObject MainObject;
    public GameObject Crown;
    public GameObject Position;
    public Sprite[] PositionSprites;
    public Text scoreTexts;
    public string playIds;
    public Text moverPositions;
    [HideInInspector]
    public bool Active = true;
    [HideInInspector]
    public bool finished = false;
    public AudioSource PlayerLeftRoomAudio;
    

    public void PlayerLeftRoom()
    {
        if (!finished)
        {
            PlayerLeftRoomAudio.Play();
            Active = false;
            Name.GetComponent<Text>().text = "";
            MainObject.transform.localScale = new Vector2(0.8f, 0.8f);
            leftRoomObject.SetActive(true);
        }
    }

    public void setPositionSprite(int index)
    {
        Position.SetActive(true);
        Position.GetComponent<Image>().sprite = PositionSprites[index];
    }
}
