using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    protected GameManager GM
    {
        get { return GameManager.singleton; }
    }

    public ResourcesManager RM;

    public GameObject victoryContainer;
    public GameObject defeatContainer;

    public TextMeshProUGUI description;

    [System.NonSerialized]
    public bool isWinner = false;

    [System.NonSerialized]
    public static EndGameScreen singleton;

    public Button menuButton;
    public Button rematchButton;
    public Button changeDeckButton;
    public Button keepDeckButton;
    public Button acceptButton;
    public TMP_Dropdown deckSelector;

    [System.NonSerialized]
    public int localWins = 0;

    [System.NonSerialized]
    public int opponentWins = 0;

    readonly List<SyncSignal> rematchSync = new List<SyncSignal>();

    bool PlayersAreSync(List<SyncSignal> signal)
    {
        if (signal.Count <= 0)
            return false;

        for (int i = 0; i < signal.Count; i++)
        {
            if (signal[i].isReady == false)
            {
                return false;
            }
        }

        return true;
    }

    void ChangePlayerSync(List<SyncSignal> signal, int photonId, bool value)
    {
        for (int i = 0; i < signal.Count; i++)
        {
            if (signal[i].photonId == photonId)
            {
                signal[i].isReady = value;
                return;
            }
        }
    }

    public void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }

        deckSelector.ClearOptions();

        for (int i = 0; i < RM.allDecks.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(RM.allDecks[i].deckName);

            deckSelector.options.Add(option);
        }

        deckSelector.value = 0;
        deckSelector.captionText.text = RM.allDecks[0].deckName;

        localWins = 0;
        opponentWins = 0;

        this.gameObject.SetActive(false);
    }

    bool gameReset = false;
    bool rematchIsOn = false;
    bool rematchVoted = false;
    bool playerDisconnected = false;

    private void Update()
    {
        if(this.gameObject.activeInHierarchy && !playerDisconnected)
        {
            if (PlayersAreSync(rematchSync) && !rematchIsOn)
            {
                description.text = "Elige el modo de la revancha";

                SetActiveButtons(false);

                SetActiveButton(changeDeckButton, true);
                SetActiveButton(keepDeckButton, true);

                rematchIsOn = true;
            }

            if (PlayersAreSync(keepDeckVote) && !rematchVoted)
            {
                keepDeckVote.Clear();
                changeDeckVote.Clear();

                description.text = "Comenzando...";

                LeanTween.value(0f, 1f, 1f).setOnComplete(()=> {
                    RematchIsReady();
                });

                rematchVoted = true;
            }

            if(PlayersAreSync(changeDeckVote) && !rematchVoted)
            {
                description.text = "Elige tu nuevo mazo";

                SetActiveButtons(false);

                deckSelector.interactable = true;
                deckSelector.gameObject.SetActive(true);

                SetActiveButton(acceptButton, true);

                rematchVoted = true;
            }

            if (ArePlayersReady() && !gameReset)
            {
                int deck_id = -1;

                if (PlayersAreSync(changeDeckVote))
                {
                    deck_id = deckSelector.value;

                    description.text = "Comenzando...";

                    LeanTween.value(0f, 1f, 1f).setOnComplete(() => {
                        ResetMatch(deck_id);
                    });
                }
                else
                {
                    ResetMatch(deck_id);
                }

                gameReset = true;
            }
        }
    }

    void ResetMatch(int deck_id)
    {
        GM.ResetMatch(deck_id);

        RematchNotReady();

        this.gameObject.SetActive(false);
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

            SetActiveButtons(false);
            SetActiveButton(rematchButton, true);
            SetActiveButton(menuButton, true);

            RematchNotReady();

            rematchIsOn = false;
            rematchVoted = false;
            gameReset = false;
            playerDisconnected = false;

            this.gameObject.SetActive(true);
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
        SetActiveButton(rematchButton, enable);
        SetActiveButton(changeDeckButton, enable);
        SetActiveButton(keepDeckButton, enable);
        SetActiveButton(acceptButton, enable);

        deckSelector.interactable = enable;
        deckSelector.gameObject.SetActive(enable);
    }

    void SetActiveButton(Button button, bool enable)
    {
        button.interactable = enable;
        button.gameObject.SetActive(enable);
    }

    private void OnEnable()
    {
        rematchSync.Clear();
        keepDeckVote.Clear();
        changeDeckVote.Clear();
        changeDeckSynced.Clear();

        foreach (PlayerHolder p in GM.allPlayers)
        {
            rematchSync.Add(new SyncSignal(p.photonId));
            keepDeckVote.Add(new SyncSignal(p.photonId));
            changeDeckVote.Add(new SyncSignal(p.photonId));
            changeDeckSynced.Add(new SyncSignal(p.photonId));
        }

        if (isWinner)
        {
            AudioManager.singleton.PlayVictorySfx();

            localWins++;

            defeatContainer.transform.parent.gameObject.SetActive(false);
            victoryContainer.transform.parent.gameObject.SetActive(true);

            victoryContainer.transform.localScale = Vector3.one;

            LeanTween.scale(victoryContainer, new Vector3(2, 2, 2), 1f).setEaseOutElastic();
        }
        else
        {
            AudioManager.singleton.PlayDefeatySfx();

            opponentWins++;

            victoryContainer.transform.parent.gameObject.SetActive(false);
            defeatContainer.transform.parent.gameObject.SetActive(true);

            defeatContainer.transform.localScale = Vector3.one;

            LeanTween.scale(defeatContainer, new Vector3(2, 2, 2), 1f).setEaseOutQuad();
        }
    }

    readonly List<SyncSignal> keepDeckVote = new List<SyncSignal>();
    readonly List<SyncSignal> changeDeckVote = new List<SyncSignal>();
    readonly List<SyncSignal> changeDeckSynced = new List<SyncSignal>();

    public void RematchMode(bool sameDeck)
    {
        AudioManager.singleton.Play(SoundEffectType.BUTTON_CLICK);

        this.description.text = "Esperando a tu oponente";

        if (sameDeck)
        {
            ChangePlayerSync(changeDeckVote, GM.localPlayer.photonId, false);
            ChangePlayerSync(keepDeckVote, GM.localPlayer.photonId, true);

            keepDeckButton.interactable = false;
            changeDeckButton.interactable = true;
        }
        else
        {
            ChangePlayerSync(changeDeckVote, GM.localPlayer.photonId, true);
            ChangePlayerSync(keepDeckVote, GM.localPlayer.photonId, false);

            keepDeckButton.interactable = true;
            changeDeckButton.interactable = false;
        }

        MultiplayerManager.singleton.SetRematchMode(sameDeck);

    }

    public void OpponentPickedMode(bool sameDeck)
    {
        if (sameDeck)
        {
            ChangePlayerSync(keepDeckVote, GM.clientPlayer.photonId, true);
            ChangePlayerSync(changeDeckVote, GM.clientPlayer.photonId, false);

            this.description.text = "Tu oponente quiere usar los <b>mismos</b> mazos";
        }
        else
        {
            ChangePlayerSync(changeDeckVote, GM.clientPlayer.photonId, true);
            ChangePlayerSync(keepDeckVote, GM.clientPlayer.photonId, false);

            this.description.text = "Tu oponente quiere <b>cambiar</b> los mazos";
        }
    }

    public void DeckSynced(int photonId)
    {
        ChangePlayerSync(changeDeckSynced, photonId, true);

        if (PlayersAreSync(changeDeckSynced))
        {
            RematchIsReady();
        }
    }

    public void ResetMatch()
    {
        AudioManager.singleton.Play(SoundEffectType.BUTTON_CLICK);

        description.text = "Esperando a tu oponente";
        rematchButton.interactable = false;

        ChangePlayerSync(rematchSync, GM.localPlayer.photonId, true);

        MultiplayerManager.singleton.AskForRematch();
    }

    public void OpponentWantsRematch()
    {
        ChangePlayerSync(rematchSync, GM.clientPlayer.photonId, true);
        description.text = "Tu oponente quiere una revancha";
    }

    public void FinishDeckSelection()
    {
        AudioManager.singleton.Play(SoundEffectType.BUTTON_CLICK);

        deckSelector.interactable = false;
        acceptButton.interactable = false;
        description.text = "Esperando a tu oponente";

        MultiplayerManager.singleton.SendNewDeckData(GM.localPlayer.photonId, RM.allDecks[deckSelector.value].cardIds);
    }

    public void ReturnToMainMenu()
    {
        AudioManager.singleton.Play(SoundEffectType.BUTTON_CLICK);

        description.text = "Desconectandose";

        deckSelector.interactable = false;
        menuButton.interactable = false;
        acceptButton.interactable = false;
        changeDeckButton.interactable = false;
        keepDeckButton.interactable = false;
        rematchButton.interactable = false;

        playerDisconnected = true;
        SessionManager.singleton.LoadMenu();
    }
}