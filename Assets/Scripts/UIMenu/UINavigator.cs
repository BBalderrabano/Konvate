using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINavigator : MonoBehaviour
{
    [System.NonSerialized]
    public MENU currentMenu = MENU.MAIN_MENU;

    float screenHeight;

    public GameObject mainMenu;
    public GameObject multiplayerMenu;

    public LeanTweenType type = LeanTweenType.easeInOutBack;
    public float time = 1f;

    List<Button> allButtons;
    List<TMP_InputField> allInputFields;

    [SerializeField]
    AudioClip chainUp;
    [SerializeField]
    AudioClip chainDown;
    [SerializeField]
    AudioClip buttonClick;

    void Start()
    {
        screenHeight = GetComponent<RectTransform>().rect.height;
        allButtons = GetComponentsInChildren<Button>().ToList();
        allInputFields = GetComponentsInChildren<TMP_InputField>().ToList();

        Settings.SCREEN_HEIGHT = screenHeight;
    }

    public void NavigateToMultiplayer() 
    {
        DisableAllButtons();

        LeanTween.moveLocalY(mainMenu, mainMenu.transform.localPosition.y + screenHeight, time).setEase(type)
        .setOnComplete(() => {
            currentMenu = MENU.MULTIPLAYER_MENU;
        }); ;

        LeanTween.moveLocalY(multiplayerMenu, multiplayerMenu.transform.localPosition.y + screenHeight, time).setEase(type).setOnComplete(EnableAllButtons);
        LeanAudio.play(chainUp, 0.7f);
    }

    public void NavigateToMain()
    {
        DisableAllButtons();

        LeanTween.moveLocalY(mainMenu, mainMenu.transform.localPosition.y - screenHeight, time).setEase(type);

        currentMenu = MENU.MAIN_MENU;

        LeanTween.moveLocalY(multiplayerMenu, multiplayerMenu.transform.localPosition.y - screenHeight, time).setEase(type).setOnComplete(EnableAllButtons);

        LeanAudio.play(chainDown, 0.7f);
    }

    void DisableAllButtons()
    {
        foreach(Button button in allButtons)
        {
            button.interactable = false;
        }

        foreach (TMP_InputField input_field in allInputFields)
        {
            input_field.interactable = false;
        }
    }

    void EnableAllButtons()
    {
        foreach (Button button in allButtons)
        {
            button.interactable = true;
        }

        foreach (TMP_InputField input_field in allInputFields)
        {
            input_field.interactable = true;
        }
    }
    
    public void PlayButtonSfx()
    {
        LeanAudio.play(buttonClick, 1f);
    }

    public enum MENU
    {
        MAIN_MENU,
        MULTIPLAYER_MENU
    }
}
