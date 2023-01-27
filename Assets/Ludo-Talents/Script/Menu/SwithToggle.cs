using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SwithToggle : MonoBehaviour
{
    [SerializeField] RectTransform uiHandleRectTransform;
    [SerializeField] Color backgroundActiveColor;
    [SerializeField] Color handleActiveColor;
    Image backgroundImage, handleImage;

    Color backgroundDefaultColor, handleDefaultColor;

    Toggle toggle;
    Vector2 handlePosition;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
       
       
        // toggle .onValueChanged.RemoveAllListeners();

        handlePosition = uiHandleRectTransform.anchoredPosition;

        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;

        if (PlayerPrefs.GetInt(StaticString.SoundsKey, 0) == 1)
        {
            toggle.isOn = false;

        }
        else
        {
            toggle.isOn = true;
            uiHandleRectTransform.DOAnchorPos(true ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack);
            handleImage.DOColor(true ? handleActiveColor : handleDefaultColor, .4f);
        }
    }

    private void Start()
    {
        toggle.onValueChanged.AddListener((OnSwitch) =>
        {
            PlayerPrefs.SetInt(StaticString.SoundsKey, OnSwitch ? 0 : 1);

            if (OnSwitch)
            {              
                AudioListener.volume = 1;
                uiHandleRectTransform.DOAnchorPos(OnSwitch ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack);
                handleImage.DOColor(OnSwitch ? handleActiveColor : handleDefaultColor, .4f);
            }
            else
            {
                AudioListener.volume = 0;
                uiHandleRectTransform.DOAnchorPos(OnSwitch ? handlePosition * 1 : handlePosition, .4f).SetEase(Ease.InOutBack);
                handleImage.DOColor(OnSwitch ? handleActiveColor : handleDefaultColor, .4f);
            }
        }
     );
    }
}
