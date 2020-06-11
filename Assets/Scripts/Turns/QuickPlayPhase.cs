using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(menuName = "Turns/Phases/Quick Play Phase")]
public class QuickPlayPhase : Phase
{
    List<CardEffect> quickplay = new List<CardEffect>();

    void LoadPlayedCardEffects(int photonId)
    {
        PlayerHolder player = GM.getPlayerHolder(photonId);

        foreach (Card c in player.playedCards)
        {
            LoadCardEffects(c);
        }

        quickplay = quickplay.OrderBy(a => a.priority).ToList();
    }

    void LoadCardEffects(Card c)
    {
        if (c.EffectsDone())
        {
            return;
        }

        for (int i = 0; i < c.cardEffects.Count; i++)
        {
            if (c.cardEffects[i].isDone ||
                (c.cardEffects[i].type != EffectType.PLACE
                && c.cardEffects[i].type != EffectType.REMOVE
                && c.cardEffects[i].type != EffectType.RESTORE
                && c.cardEffects[i].type != EffectType.SPECIAL))
                continue;

            quickplay.Add(c.cardEffects[i]);
        }
    }

    void ExecuteEffect()
    {
        foreach (CardEffect eff in quickplay)
        {
            if (eff.isDone) { continue; }

            Action execute_effect = new A_ExecuteEffect(eff.card.instanceId, eff.effectId, eff.card.owner.photonId);
            GM.actionManager.AddAction(execute_effect);
        }
    }

    bool finalCheck = false;

    public override bool IsComplete()
    {
        if(isInit && GM.actionManager.IsDone() && PlayersAreReady() && !finalCheck)
        {
            foreach (PlayerHolder p in GM.allPlayers)
            {
                LoadPlayedCardEffects(p.photonId);
            }

            ExecuteEffect();

            finalCheck = true;
        }

        return isInit && GM.actionManager.IsDone() && PlayersAreReady() && finalCheck;
    }

    public override void OnStartPhase()
    {
        base.OnStartPhase();

        if (!isInit)
        {
            GM.onPhaseChange.Raise();

            quickplay.Clear();
            GM.localPlayer.playedQuickCard = false;

            finalCheck = false;
            isInit = true;
        }
    }

    public override void OnEndPhase()
    {
        if (isInit)
        {
            GM.SetState(null);
            isInit = false;
        }
    }

    public override bool CanPlayCard(Card c)
    {
        if (c.GetCardType() is QuickPlay) {
            return true;
        }
        else
        {
            WarningPanel.singleton.ShowWarning("Solo puedes jugar cartas relámpago <sprite=3> durante esta fase", false);
            return false;
        }
    }

    public override void OnTurnButtonPress()
    {
        int localPlayerId = GM.localPlayer.photonId;
        int otherPlayerId = GM.clientPlayer.photonId;

        if ((CheckPlayerIsReady(localPlayerId) && !GM.currentPlayer.isLocal) || CheckPlayerIsReady(otherPlayerId))
            return;

        Action giveControl = new A_GiveControl(phaseIndex, localPlayerId, false, otherPlayerId);
        GM.actionManager.AddAction(giveControl);

        AudioManager.singleton.Play(SoundEffectType.BUTTON_CLICK);
    }

    public override void OnTurnButtonHold()
    {
        int localPlayerId = GM.localPlayer.photonId;
        int otherPlayerId = GM.clientPlayer.photonId;

        if (CheckPlayerIsReady(localPlayerId))
            return;

        Action giveControl = new A_GiveControl(phaseIndex, localPlayerId, true, otherPlayerId);
        GM.actionManager.AddAction(giveControl);

        AudioManager.singleton.Play(SoundEffectType.BUTTON_CLICK);
    }

    public override void OnPhaseControllerChange(int photonId)
    {
        LoadPlayedCardEffects(photonId);
        ExecuteEffect();
    }

    public override void OnPlayCard(Card c)
    {
        int currentPlayerId = GM.currentPlayer.photonId;
        int localPlayerId = GM.localPlayer.photonId;
        int otherPlayerId = GM.clientPlayer.photonId;

        if (CheckPlayerIsReady(otherPlayerId) || CheckPlayerIsReady(localPlayerId))
        {
            LoadCardEffects(c);
            ExecuteEffect();
        }
        else
        {
            if (currentPlayerId == localPlayerId)
            {
                Action giveControl = new A_GiveControl(phaseIndex, localPlayerId, false, otherPlayerId, false);
                GM.actionManager.AddAction(giveControl);
            }
        }
    }
}
