using System.Collections.Generic;
using UnityEngine;
using SO;
using System.Linq;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager singleton;

    public ActionManager actionManager;
    public AnimationManager animationManager;
    public ResourcesManager resourcesManager;

    public bool isMultiplayer;

    public PlayerHolder[] allPlayers;

    public PlayerHolder currentPlayer;

    public CardHolders playerOneHolder;
    public CardHolders playerTwoHolder;

    public List<Card> all_cards = new List<Card>();

    [System.NonSerialized]
    public List<CardTextMod> text_mods = new List<CardTextMod>();

    public Card GetCard(int instanceId)
    {
        for (int i = 0; i < all_cards.Count; i++)
        {
            if (all_cards[i].instanceId == instanceId)
                return all_cards[i];
        }

        return null;
    }

    public PlayerHolder localPlayer;
    public PlayerHolder clientPlayer;

    public PlayerHolder GetPlayerHolder(int photonId)
    {
        if (localPlayer.photonId == photonId)
            return localPlayer;

        if (clientPlayer.photonId == photonId)
            return clientPlayer;

        return null;
    }

    public PlayerHolder GetOpponentHolder(int photonId)
    {
        if (clientPlayer.photonId == photonId)
            return localPlayer;
        else
            return clientPlayer;
    }

    public GameState currentState;

    public Turn turn;

    public bool isInit = false;

    public GameEvent onTurnChange;
    public GameEvent onPhaseControllerChange;
    public GameEvent onPhaseChange;

    public GameObject currentPreviewCard;
    Vector3 originalPreviewScale;

    public void PreviewCard(Card c, bool autoHide = true)
    {
        if (c == null)
            return;

        iTween.Stop(currentPreviewCard);

        currentPreviewCard.transform.localScale = originalPreviewScale;
        currentPreviewCard.GetComponent<CardViz>().LoadCardViz(c);
        currentPreviewCard.SetActive(true);

        if (autoHide)
        {
            iTween.ScaleTo(currentPreviewCard, iTween.Hash(
                    "scale", Vector3.zero,
                    "time", Settings.CARD_EFFECT_PREVIEW_ANIM_DURATION,
                    "delay", Settings.CARD_EFFECT_MIN_PREVIEW,
                    "easetype", "easeInElastic",
                    "oncomplete", "HidePreviewCard",
                    "oncompletetarget", this.gameObject
                    ));
        }
    }

    public void AnimateHidePrivewCard()
    {
        iTween.Stop(currentPreviewCard);

        iTween.ScaleTo(currentPreviewCard, iTween.Hash(
                    "scale", Vector3.zero,
                    "time", Settings.CARD_EFFECT_PREVIEW_ANIM_DURATION,
                    "easetype", "easeInElastic",
                    "oncomplete", "HidePreviewCard",
                    "oncompletetarget", this.gameObject
                    ));
    }

    public void HidePreviewCard()
    {
        currentPreviewCard.SetActive(false);
    }
    #endregion

    #region Initialization
    public void Awake()
    {
        if (singleton == null)
        {
            if (resourcesManager == null)
            {
                resourcesManager = Settings.GetResourcesManager();
            }

            singleton = this;
            animationManager = GetComponent<AnimationManager>();

            currentPreviewCard.SetActive(false);
            originalPreviewScale = currentPreviewCard.transform.localScale;
        }
    }

    public void InitGame(int startingPlayerID)
    {
        checkTextMods = true;

        if (!isMultiplayer)
        {
            allPlayers = new PlayerHolder[2];

            allPlayers[0] = localPlayer;
            allPlayers[0].isLocal = true;

            allPlayers[1] = clientPlayer;
            allPlayers[1].isLocal = false;

            currentPlayer = allPlayers[0];
        }
        else
        {
            if (localPlayer.photonId == startingPlayerID)
            {
                turn.offensivePlayer = allPlayers[0];
            }
            else
            {
                turn.offensivePlayer = allPlayers[1];
            }

            currentPlayer = null;
        }

        allPlayers[0].currentHolder = playerOneHolder;
        allPlayers[1].currentHolder = playerTwoHolder;

        SetPlayers();

        GameObject offensiveChip = Instantiate(resourcesManager.dataHolder.offensiveChip, Vector3.zero, Quaternion.identity, gameObject.transform);

        offensiveChip.transform.localPosition = new Vector3(offensiveChip.transform.localPosition.x + 1000000, offensiveChip.transform.localPosition.y);
        offensiveChip.transform.localScale = Vector3.one;
        offensiveChip.GetComponent<Chip>().owner = turn.offensivePlayer;

        turn.offensiveChip = offensiveChip;

        turn.turnCount = 0;

        for (int i = 0; i < turn.phases.Length; i++)
        {
            turn.phases[i].phaseIndex = i;
            turn.phases[i].InitPlayerSync();
        }

        onTurnChange.Raise();

        actionManager = new ActionManager();

        if (!isMultiplayer)
        {
            isInit = true;
        }
    }

    void SetPlayers()
    {
        for (int i = 0; i < allPlayers.Length; i++)
        {
            allPlayers[i].Init();

            if (!isMultiplayer) 
            {
                PlayerHolder playerHolder = allPlayers[i];

                for (int d = 0; d < playerHolder.starting_deck.Count; d++)
                {
                    Card card = resourcesManager.GetCardInstance(playerHolder.starting_deck[d]);
                    GameObject go = Instantiate(resourcesManager.dataHolder.cardPrefab) as GameObject;
                    
                    CardViz v = go.GetComponent<CardViz>();
                    v.LoadCard(card);
                    Settings.SetParent(go.transform, playerHolder.currentHolder.deckGrid.value);

                    card.photonId = playerHolder.photonId;
                    card.owner = playerHolder;

                    card.instanceId = int.Parse(card.photonId.ToString() + card.instanceId.ToString());
                    card.cardPhysicalInst = go.GetComponent<CardInstance>();
                    card.cardPhysicalInst.setCurrentLogic(resourcesManager.dataHolder.deckLogic);

                    for (int f = 0; f < card.cardEffects.Count; f++)
                    {
                        card.cardEffects[f] = (CardEffect)card.cardEffects[f].Clone();
                        card.cardEffects[f].card = card;
                    }

                    playerHolder.deck.Add(card);
                    playerHolder.all_cards.Add(card);
                }
            }
        }

        if (isMultiplayer)
        {
            MultiplayerManager.singleton.CreateCardForPlayers();
        }
    }
    #endregion

    #region Card Actions

    public void ChangeCardOwner(int cardId, int oldOwnerPhotonId, int newOwnerPhotonId)
    {
        Card card = GetCard(cardId);
        PlayerHolder newOwner = GetPlayerHolder(newOwnerPhotonId);
        PlayerHolder oldOwner = GetPlayerHolder(oldOwnerPhotonId);

        card.owner = newOwner;
        newOwner.all_cards.Add(card);
        oldOwner.all_cards.Remove(card);
    }

    public List<Action> DrawCard(PlayerHolder player, int amount, int cardId = 0, int effectId = 0, Action action = null)
    {
        List<Action> actions = new List<Action>();

        if (player.isLocal)
        {
            for (int i = 0; i < amount; i++)
            {
                Action draw = new A_Draw(player.photonId, -1, cardId, effectId);

                if(action == null)
                {
                    actionManager.AddAction(draw);
                }

                actions.Add(draw);
            }
        }

        return actions;
    }

    #endregion

    #region Turn Loop

    public bool endingTurn = false;

    bool checkTextMods = true;

    private void Update()
    {
        if (!isInit)
            return;

        bool isComplete = turn.Execute();

        if (!isMultiplayer)
        {
            if (isComplete)
            {
                turn.turnCount += 1;
                onTurnChange.Raise();
            }
        }
        else
        {
            if (endingTurn)
            {
                endingTurn = false;
                turn.turnCount += 1;

                onTurnChange.Raise();
                return;
            }

            if (isComplete && !endingTurn)
            {
                endingTurn = true;
                return;
            }
        }

        if (checkTextMods)
        {
            if (text_mods.Count > 0)
            {
                foreach (CardTextMod mod in text_mods)
                {
                    mod.UpdateText();
                }
            }
            else
            {
                checkTextMods = false;
            }
        }

        if (currentState != null)
        {
            currentState.Tick(Time.deltaTime);
        }

        actionManager.Tick(Time.deltaTime);
    }

    public void ChangeTurnController(int photonID, bool isVisual = false)
    {
        PlayerHolder p = GetPlayerHolder(photonID);

        if (!isVisual)
        {
            p.playedQuickCard = false;
            turn.currentPhase.value.OnPhaseControllerChange(photonID);
        }

        currentPlayer = p;
        onPhaseControllerChange.Raise();
    }

    public void SetState(GameState state)
    {
        currentState = state;
    }

    #endregion

    #region Chips
    public List<Transform> GetChips(ChipType type, PlayerHolder player, bool played = false)
    {
        List<Transform> pick = new List<Transform>();
        Transform chipsHolder;

        if(type == ChipType.POISON)
        {
            if (played)
            {
                chipsHolder = player.currentHolder.playedPoisonChipHolder.value;
            }
            else
            {
                chipsHolder = player.currentHolder.poisonChipHolder.value;
            }

        }
        else if(type == ChipType.BLEED)
        {
            chipsHolder = player.currentHolder.bleedChipHolder.value;
        }
        else
        {
            if (played)
            {
                chipsHolder = player.currentHolder.playedCombatChipHolder.value;
            }
            else
            {
                chipsHolder = player.currentHolder.combatChipHolder.value;
            }
        }

        for (int i = 0; i < chipsHolder.childCount; i++)
        {
            Transform _chip = chipsHolder.GetChild(i);

            if(type == ChipType.COMBAT_OFFENSIVE)
            {
                if (_chip.GetComponent<Chip>().type == ChipType.OFFENSIVE || _chip.GetComponent<Chip>().type == ChipType.COMBAT)
                {
                    pick.Add(_chip);
                }
            }
            else
            {
                if (_chip.GetComponent<Chip>().type == type)
                {
                    pick.Add(_chip);
                }
            }
        }

        return pick;
    }
    #endregion
}
