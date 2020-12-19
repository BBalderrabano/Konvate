using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    public GameObject victoryContainer;
    public GameObject defeatContainer;

    public TMPro.TextMeshProUGUI description;

    [System.NonSerialized]
    public bool isWinner = false;

    [System.NonSerialized]
    public static EndGameScreen singleton;

    public Button menuButton;
    public Button rematchButton;
    public Button sameDeckButton;
    public Button keepDeckButton;
    public Button acceptButton;
    public Button declineButton;

    public void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        this.gameObject.SetActive(false);
    }

    bool gameReset = false;
    bool playerDisconnected = false;

    private void Update()
    {
        if(!playerDisconnected)
        {
            if (this.gameObject.activeInHierarchy && ArePlayersReady() && !gameReset)
            {
                GameManager.singleton.ResetMatch();
                this.gameObject.SetActive(false);

                RematchNotReady();

                gameReset = true;
            }
        }
    }

    public void RematchIsReady()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable() { { "RematchIsReady", true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    public void RematchNotReady()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable() { { "RematchIsReady", false } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    public bool ArePlayersReady()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (!(bool)player.CustomProperties["RematchIsReady"])
            {
                return false;
            }
        }
        
        return true;
    }
    
    public void EndGame(bool isWinner, string description)
    {
        if (!this.gameObject.activeInHierarchy)
        {
            this.description.text = description;
            this.isWinner = isWinner;

            this.gameObject.SetActive(true);

            rematchButton.interactable = true;
            rematchButton.gameObject.SetActive(true);

            menuButton.interactable = true;
            menuButton.gameObject.SetActive(true);

            RematchNotReady();

            gameReset = false;
            playerDisconnected = false;
        }
    }

    public void PlayerDisconnected()
    {
        playerDisconnected = true;

        if (!this.gameObject.activeInHierarchy)
        {
            EndGame(true, "Tu oponente se a desconectado");
        }
        else
        {
            this.description.text = "Tu oponente abandono la partida";
        }

        SetActiveButtons(false);
    }

    void SetActiveButtons(bool enable)
    {
        rematchButton.interactable = enable;
        rematchButton.gameObject.SetActive(enable);

        sameDeckButton.interactable = enable;
        sameDeckButton.gameObject.SetActive(enable);

        keepDeckButton.interactable = enable;
        keepDeckButton.gameObject.SetActive(enable);

        acceptButton.interactable = enable;
        acceptButton.gameObject.SetActive(enable);

        declineButton.interactable = enable;
        declineButton.gameObject.SetActive(enable);
    }

    private void OnEnable()
    {
        if (isWinner)
        {
            defeatContainer.transform.parent.gameObject.SetActive(false);
            victoryContainer.transform.parent.gameObject.SetActive(true);

            victoryContainer.transform.localScale = Vector3.one;

            LeanTween.scale(victoryContainer, new Vector3(2, 2, 2), 1f).setEaseOutElastic();
        }
        else
        {
            victoryContainer.transform.parent.gameObject.SetActive(false);
            defeatContainer.transform.parent.gameObject.SetActive(true);

            defeatContainer.transform.localScale = Vector3.one;

            LeanTween.scale(victoryContainer, new Vector3(2, 2, 2), 1f).setEaseOutQuad();
        }
    }

    public void ResetMatch()
    {
        description.text = "Esperando a tu oponente";
        rematchButton.interactable = false;
        RematchIsReady();
    }

    public void ReturnToMainMenu()
    {
        playerDisconnected = true;
        SessionManager.singleton.LoadMenu();
    }
}