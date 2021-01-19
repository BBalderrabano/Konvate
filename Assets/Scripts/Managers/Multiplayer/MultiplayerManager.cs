using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiplayerManager : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    #region Variables
    public static MultiplayerManager singleton;
    List<NetworkPrint> playerPrints = new List<NetworkPrint>();

    public string GetVsText()
    {
        int firstPlayerWins = PhotonNetwork.PlayerList[0].IsLocal ? EndGameScreen.singleton.localWins : EndGameScreen.singleton.opponentWins;
        int secondPlayerWins = PhotonNetwork.PlayerList[1].IsLocal ? EndGameScreen.singleton.localWins : EndGameScreen.singleton.opponentWins;

        return PhotonNetwork.PlayerList[0].NickName + (firstPlayerWins > 0 || secondPlayerWins > 0 ? " (" + firstPlayerWins + ")" : "") + 
                                                                                    " vs " + 
               PhotonNetwork.PlayerList[1].NickName + (firstPlayerWins > 0 || secondPlayerWins > 0 ? " (" + secondPlayerWins + ")" : "");
    }

    NetworkPrint localPrint;

    public NetworkPrint GetPrint(int photonId)
    {
        for (int i = 0; i < playerPrints.Count; i++)
        {
            if (playerPrints[i].photonId == photonId)
                return playerPrints[i];
        }
        return null;
    }

    Transform multiplayerReferences;

    bool gameStarted;

    GameManager GM
    {
        get { return GameManager.singleton; }
    }

    #endregion

    #region Multiplayer Checks
    public void PlayerIsReady()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable() { { "PlayerIsReady", true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    public void PlayerIsNotReady()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable() { { "PlayerIsReady", false } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    public bool ArePlayersReady()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (!(bool)player.CustomProperties["PlayerIsReady"])
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Update
    private void Update()
    {
        if (!gameStarted && PhotonNetwork.LevelLoadingProgress == 1)
        {
            if (playerPrints.Count > 1 && GM != null)
            {
                gameStarted = true;
                StartMatch();
            }
        }
        else if (gameStarted)
        {
            if (ArePlayersReady() && !GM.isInit)
            {
                foreach (PlayerHolder player in GM.allPlayers)
                {
                    GM.all_cards.AddRange(player.all_cards);
                }

                GM.isInit = true;
            }
        }
    }


    #endregion

    #region Init
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        multiplayerReferences = new GameObject("MultiplayerReferences").transform;
        DontDestroyOnLoad(multiplayerReferences.gameObject);

        singleton = this;
        DontDestroyOnLoad(this.gameObject);

        SessionManager.singleton.dontDestroyOnLoadObjects.Add(multiplayerReferences.gameObject);
        SessionManager.singleton.dontDestroyOnLoadObjects.Add(this.gameObject);

        InstantiateNetworkPrint();
        NetworkManager.singleton.LoadGameScene();
    }

    void InstantiateNetworkPrint()
    {
        PhotonNetwork.Instantiate("NetworkPrint", Vector3.zero, Quaternion.identity, 0);
    }

    #endregion

    #region Starting Match
    public void StartMatch()
    {
        int localplayerid = -1;
        int clientplayerid = -1;

        GM.allPlayers = new PlayerHolder[2];

        foreach (NetworkPrint p in playerPrints)
        {
            if (p.isLocal)
            {
                GM.allPlayers[0] = GM.localPlayer;
                GM.allPlayers[0].photonId = p.photonId;
                GM.allPlayers[0].isLocal = true;
                GM.allPlayers[0].playerName = PhotonNetwork.PlayerList[0].NickName;

                p.playerHolder = GM.allPlayers[0];

                localplayerid = p.photonId;
            }
            else
            {
                GM.allPlayers[1] = GM.clientPlayer;
                GM.allPlayers[1].photonId = p.photonId;
                GM.allPlayers[1].isLocal = false;
                GM.allPlayers[1].playerName = PhotonNetwork.PlayerList[1].NickName;


                p.playerHolder = GM.allPlayers[1];

                clientplayerid = p.photonId;
            }
        }

        if (NetworkManager.isMaster)
        {
            photonView.RPC("RPC_InitGame", RpcTarget.AllBuffered, Random.Range(0, 10) < 5 ? localplayerid : clientplayerid);
        }
    }

    [PunRPC]
    public void RPC_InitGame(int startingPlayer)
    {
        GM.isMultiplayer = true;
        GM.InitGame(startingPlayer);
        GM.onPhaseControllerChange.Raise();
    }

    public void CreateCardForPlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            List<int> playersIds = new List<int>();
            List<int> cardIds = new List<int>();
            int cardIdIndex = 0;

            for (int i = 0; i < playerPrints.Count; i++)
            {
                playersIds.Add(playerPrints[i].photonId);

                for (int c = 0; c < playerPrints[i].getStartingCardIds().Length; c++)
                {
                    cardIds.Add(cardIdIndex);
                    cardIdIndex++;
                }
            }

            photonView.RPC("RPC_CreateAllCardsForPlayers", RpcTarget.AllBuffered, playersIds.ToArray(), cardIds.ToArray());
        }
    }

    [PunRPC]
    public void RPC_CreateAllCardsForPlayers(int[] playerIds, int[] cardIds)
    {
        int cardIdIndex = 0;

        for (int i = 0; i < playerIds.Length; i++)
        {
            NetworkPrint player = GetPrint(playerIds[i]);
            PlayerHolder playerHolder = GM.GetPlayerHolder(playerIds[i]);

            foreach (string cardName in player.getStartingCardIds())
            {
                Card card = GM.resourcesManager.GetCardInstance(cardName);

                GameObject go = Instantiate(GM.resourcesManager.dataHolder.cardPrefab) as GameObject;

                CardViz v = go.GetComponent<CardViz>();
                v.LoadCard(card);
                Settings.SetParent(go.transform, playerHolder.currentHolder.deckGrid.value);

                card.photonId = player.photonId;
                card.owner = playerHolder;

                card.instanceId = cardIds[cardIdIndex];
                card.instanceId = int.Parse(card.photonId.ToString() + card.instanceId.ToString());

                card.cardPhysicalInst = go.GetComponent<CardInstance>();
                card.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.deckLogic);

                for (int f = 0; f < card.cardEffects.Count; f++)
                {
                    card.cardEffects[f] = (CardEffect)card.cardEffects[f].Clone();
                    card.cardEffects[f].card = card;
                    card.cardEffects[f].effectId = int.Parse((f + 1).ToString() + card.instanceId.ToString());
                }

                int handLinkEffectAmount = 0;

                foreach (CE_HandCheck handEffect in card.cardEffects.OfType<CE_HandCheck>())
                {
                    handEffect.linkedCardEffect = (CardEffect)handEffect.linkedCardEffect.Clone();
                    handEffect.linkedCardEffect.card = card;
                    handEffect.linkedCardEffect.effectId = int.Parse(handLinkEffectAmount.ToString() + card.instanceId.ToString() + handLinkEffectAmount.ToString());
                    handLinkEffectAmount++;
                }

                if(card.textMods.Count > 0)
                {
                    foreach(CardTextMod mod in card.textMods)
                    {
                        mod.Init(card);
                        GM.text_mods.Add((CardTextMod)mod.Clone());
                    }
                }

                playerHolder.deck.Add(card);
                playerHolder.all_cards.Add(card);

                cardIdIndex++;
            }

            playerHolder.deck.Shuffle();
            playerHolder.deck.Shuffle();
            playerHolder.deck.Shuffle();

            if (playerHolder.isLocal)
            {
                int[] deckOrder = new int[playerHolder.deck.Count];

                for (int e = 0; e < deckOrder.Length; e++)
                {
                    deckOrder[e] = playerHolder.deck[e].instanceId;
                }

                SyncronizeAllCards(playerHolder.photonId, null, deckOrder, null, null);
            }
        }
    }

    #endregion

    #region Phase Controller
    public void SendActionGiveControl(int phaseIndex, int photonId, bool endMyPhase, int newControllerPhotonId, bool showMessages, int actionId)
    {
        photonView.RPC("RPC_SendActionGiveControl", RpcTarget.OthersBuffered, phaseIndex, photonId, endMyPhase, newControllerPhotonId, showMessages, actionId);
    }

    [PunRPC]
    public void RPC_SendActionGiveControl(int phaseIndex, int photonId, bool endMyPhase, int newControllerPhotonId, bool showMessages, int actionId)
    {
        KAction giveControl = new A_GiveControl(phaseIndex, photonId, endMyPhase, newControllerPhotonId, showMessages, actionId);
        GM.actionManager.AddAction(giveControl);
    }

    #endregion

    #region Card Operations
    public void SyncronizeAllCards(int photonId, int[] hand, int[] deck, int[] discard, int[] played, int actionId = -1)
    {
        if (photonId == localPrint.photonId)
        {
            photonView.RPC("RPC_SyncronizeAllCards", RpcTarget.OthersBuffered, photonId, hand, deck, discard, played, actionId);
        }
    }

    [PunRPC]
    public void RPC_SyncronizeAllCards(int photonId, int[] hand, int[] deck, int[] discard, int[] played, int actionId)
    {
        PlayerHolder player = GM.GetPlayerHolder(photonId);

        if (!player.isLocal)
        {
            if (hand != null)
            {
                player.handCards.Clear();

                for (int i = 0; i < hand.Length; i++)
                {
                    player.handCards.Add(player.GetCard(hand[i]));
                }
            }

            if (deck != null)
            {
                player.deck.Clear();

                for (int i = 0; i < deck.Length; i++)
                {
                    player.deck.Add(player.GetCard(deck[i]));
                }
            }

            if(discard != null)
            {
                player.discardCards.Clear();

                for (int i = 0; i < discard.Length; i++)
                {
                    player.discardCards.Add(player.GetCard(discard[i]));
                }
            }

            if (played != null)
            {
                player.playedCards.Clear();

                for (int i = 0; i < played.Length; i++)
                {
                    player.playedCards.Add(player.GetCard(played[i]));
                }
            }

            if(actionId > 0)
            {
                SendCompleteAction(actionId, GM.localPlayer.photonId);
            }
        }
    }

    public void ShowOpponentCards(int[] cardIds, int photonId, string description = null)
    {
        if (photonId == localPrint.photonId)
        {
            photonView.RPC("RPC_ShowOpponentCards", RpcTarget.OthersBuffered, cardIds, photonId, description);
        }
    }

    [PunRPC]
    public void RPC_ShowOpponentCards(int[] cardIds, int photonId, string description)
    {
        if (photonId != localPrint.photonId)
        {
            PlayerHolder player = GM.GetPlayerHolder(photonId);
            List<Card> cardsShown = new List<Card>();

            for (int i = 0; i < cardIds.Length; i++)
            {
                cardsShown.Add(player.GetCard(cardIds[i]));
            }

            if (cardsShown.Count > 0)
            {
                ScrollSelectionManager.singleton.SelectCards(cardsShown, description, true);
            }
        }
    }

    #endregion

    #region Carrousel Card Selection

    public void PlayerFinishCardSelection(int[] cardIds, int cardId, int effectId, int photonId, int actionId)
    {
        photonView.RPC("RPC_PlayerFinishCardSelection", RpcTarget.Others, cardIds, cardId, effectId, photonId, actionId);
    }

    [PunRPC]
    public void RPC_PlayerFinishCardSelection(int[] cardIds, int cardId, int effectId, int photonId, int actionId)
    {
        GM.actionManager.AddAction(new A_CardSelectionComplete(cardIds, photonId, cardId, effectId, actionId));
    }

    #endregion

    #region Turn Handles
    public void PhaseIsDone(int photonId, int phaseIndex)
    {
        if (photonId == localPrint.photonId)
        {
            GM.turn.GetPhaseByIndex(phaseIndex).ChangePlayerSync(photonId, true);
            photonView.RPC("RPC_PhaseIsDone", RpcTarget.OthersBuffered, photonId, phaseIndex);
        }
    }

    [PunRPC]
    public void RPC_PhaseIsDone(int photonId, int phaseIndex)
    {
        if (photonId != localPrint.photonId)
        {
            if (GM.turn.GetPhaseByIndex(phaseIndex) != null)
            {   
                GM.turn.GetPhaseByIndex(phaseIndex).ChangePlayerSync(photonId, true);
            }
        }
    }

    #endregion

    #region Actions

    public void SendDraw(int photonId, int actionId = -1, int cardId = 0, int effectId = 0)
    {
        photonView.RPC("RPC_SendDraw", RpcTarget.OthersBuffered, photonId, actionId, cardId, effectId);
    }

    [PunRPC]
    public void RPC_SendDraw(int photonId, int actionId = -1, int cardId = 0, int effectId = 0)
    {
        KAction draw = new A_Draw(photonId, actionId, cardId, effectId);

        GM.actionManager.AddAction(draw);

        draw.PlayerIsReady(GM.localPlayer.photonId);
        SendCompleteAction(actionId, GM.localPlayer.photonId);
    }

    public void SendShuffle(int actionId, int photonId, int[] deckId, bool andDraw = true)
    {
        photonView.RPC("RPC_SendShuffle", RpcTarget.OthersBuffered, actionId, photonId, deckId, andDraw);
    }

    [PunRPC]
    public void RPC_SendShuffle(int actionId, int photonId, int[] deckIds, bool andDraw)
    {
        KAction shuffle = new A_Shuffle(photonId, deckIds, andDraw, actionId);

        GM.actionManager.AddAction(shuffle);

        shuffle.PlayerIsReady(GM.localPlayer.photonId);
        SendCompleteAction(actionId, GM.localPlayer.photonId);
    }

    public void SendUseCard(int instanceId, int photonId, int actionId, ActionType type)
    {
        photonView.RPC("RPC_SendUseCard", RpcTarget.OthersBuffered, instanceId, photonId, actionId, type);
    }

    [PunRPC]
    public void RPC_SendUseCard(int instanceId, int photonId, int actionId, ActionType type)
    {
        KAction execute = null;

        if (GM.actionManager.GetAction(actionId) != null)
        {
            GM.actionManager.GetAction(actionId).PlayerIsReady(photonId);
        }
        else
        {
            switch (type)
            {
                case ActionType.PLAY_CARD:
                    execute = new A_PlayCard(instanceId, photonId, actionId);
                    break;
                case ActionType.RETURN_TO_HAND:
                    execute = new A_ReturnToHand(instanceId, photonId, actionId);
                    break;

            }

            GM.actionManager.AddAction(execute);

            execute.PlayerIsReady(GM.localPlayer.photonId);
            SendCompleteAction(actionId, GM.localPlayer.photonId);
        }
    }

    public void SendCompleteAction(int actionId, int photonId)
    {
        photonView.RPC("RPC_SendCompleteAction", RpcTarget.OthersBuffered, actionId, photonId);
    }

    [PunRPC]
    public void RPC_SendCompleteAction(int actionId, int photonId)
    {
        if (GM.actionManager.GetAction(actionId) != null)
        {
            GM.actionManager.GetAction(actionId).PlayerIsReady(photonId);
        }
    }

    public void SendConcede(int photonId)
    {
        photonView.RPC("RPC_SendConcede", RpcTarget.OthersBuffered, photonId);
    }

    [PunRPC]
    public void RPC_SendConcede(int photonId)
    {
        if (!GM.GetPlayerHolder(photonId).isLocal)
        {
            EndGameScreen.singleton.EndGame(true, "Tu oponente se a rendido");
        }
    }

    #endregion

    #region Rematch
    public void AskForRematch()
    {
        photonView.RPC("RPC_AskForRematch", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    public void RPC_AskForRematch()
    {
        EndGameScreen.singleton.OpponentWantsRematch();
    }

    public void SetRematchMode(bool keepDeck)
    {
        photonView.RPC("RPC_SetRematchMode", RpcTarget.OthersBuffered, keepDeck);
    }

    [PunRPC]
    public void RPC_SetRematchMode(bool keepDeck)
    {
        EndGameScreen.singleton.OpponentPickedMode(keepDeck);
    }

    public void SendNewDeckData(int photon, string[] new_deck)
    {
        photonView.RPC("RPC_SendNewDeckData", RpcTarget.AllBuffered, photon, new_deck);
    }

    [PunRPC]
    public void RPC_SendNewDeckData(int photon, string[] new_deck)
    {
        GetPrint(photon).changeStartingCards(new_deck);
        EndGameScreen.singleton.DeckSynced(photon);
    }

    #endregion

    #region Utilities
    public void AddPlayer(NetworkPrint n_print)
    {
        if (n_print.isLocal)
            localPrint = n_print;

        playerPrints.Add(n_print);
        n_print.transform.parent = multiplayerReferences;
    }

    #endregion
}

public enum ActionType
{
    DRAW_CARD,
    SHUFFLE_DECK,
    DISCARD_CARD,
    PLAY_CARD,
    RETURN_TO_HAND,
    NONE
}
