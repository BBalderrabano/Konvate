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
    MENU lastMenu = MENU.MAIN_MENU;

    float screenHeight;

    public GameObject mainMenu;
    public GameObject multiplayerMenu;
    public GameObject collectionMenu;

    public LeanTweenType type = LeanTweenType.easeInOutBack;
    public float time = 1f;

    List<Button> allButtons;
    List<TMP_InputField> allInputFields;

    public AudioClip chainUp;
    public AudioClip chainDown;
    public AudioClip buttonClick;

    CanvasGroup mainMenuCanvasGroup;
    CanvasGroup collectionMenuCanvasGroup;

    ScrollDeckSelector scrollDeckSelector;
    CollectionNavigator collectionNavigator;

    void Start()
    {
        screenHeight = GetComponent<RectTransform>().rect.height;
        allButtons = GetComponentsInChildren<Button>().ToList();
        allInputFields = GetComponentsInChildren<TMP_InputField>().ToList();
        scrollDeckSelector = multiplayerMenu.GetComponentInChildren<ScrollDeckSelector>();

        mainMenuCanvasGroup = GetComponent<CanvasGroup>();
        collectionMenuCanvasGroup = collectionMenu.GetComponent<CanvasGroup>();
        collectionNavigator = collectionMenu.GetComponent<CollectionNavigator>();

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
    
    public void NavigateToCollection()
    {
        DisableAllButtons();

        lastMenu = currentMenu;
        currentMenu = MENU.COLLECTION_MENU;

        collectionNavigator.LoadCollectionMenu();
        collectionMenu.transform.GetChild(0).transform.localScale = new Vector3(2, 2);

        LeanTween.scale(collectionMenu.transform.GetChild(0).gameObject, Vector3.one, time);

        LeanTween.value(1f, 0f, time * 0.5f)
            .setOnUpdate((float a)=> {
                mainMenuCanvasGroup.alpha = a;
                collectionMenuCanvasGroup.alpha = (1f - a);
            })
            .setOnComplete(()=> {
                mainMenuCanvasGroup.interactable = false;
                mainMenuCanvasGroup.blocksRaycasts = false;

                collectionMenuCanvasGroup.interactable = true;
                collectionMenuCanvasGroup.blocksRaycasts = true;

                EnableAllButtons();
            });

        LeanAudio.play(chainUp, 0.7f);
    }

    public void ReturnFromCollection()
    {
        DisableAllButtons();

        collectionMenuCanvasGroup.interactable = false;
        collectionMenuCanvasGroup.blocksRaycasts = false;

        LeanTween.scale(collectionMenu.transform.GetChild(0).gameObject, new Vector3(2, 2), time);

        LeanTween.value(1f, 0f, time * 0.5f)
            .setOnUpdate((float a) => {
                mainMenuCanvasGroup.alpha = (1f - a);
                collectionMenuCanvasGroup.alpha = a;
            })
            .setOnComplete(() => {
                mainMenuCanvasGroup.interactable = true;
                mainMenuCanvasGroup.blocksRaycasts = true;

                EnableAllButtons();
                scrollDeckSelector.ContinueSelection();

                currentMenu = lastMenu;
            });

        LeanAudio.play(chainDown, 0.7f);
    }

    void DisableAllButtons()
    {
        foreach (Button button in allButtons)
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
        MULTIPLAYER_MENU,
        COLLECTION_MENU
    }
}
