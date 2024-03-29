﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Turns/Phases/Quick Play Phase")]
public class QuickPlayPhase : Phase
{
    List<CardEffect> quickplay = new List<CardEffect>();

    bool counterPlayed = false;

    void LoadPlayedCardEffects(int photonId)
    {
        PlayerHolder player = GM.GetPlayerHolder(photonId);

        foreach (Card c in player.playedCards)
        {
            if (c.isBroken || c.GetCardType() is NormalPlay)
                continue;

            LoadCardEffects(c);
        }

        quickplay = quickplay.OrderBy(a => a.priority).ToList();
    }

    void LoadCardEffects(Card c)
    {
        if (c.EffectsDone() || c.GetCardType() is NormalPlay)
        {
            return;
        }

        c.cardViz.cardBorder.color = Color.blue;

        for (int i = 0; i < c.cardEffects.Count; i++)
        {
            if (c.cardEffects[i].isDone ||
                (c.cardEffects[i].type != EffectType.PLACE
                && c.cardEffects[i].type != EffectType.REMOVE
                //&& c.cardEffects[i].type != EffectType.RESTORE
                && c.cardEffects[i].type != EffectType.QUICK_COUNTER
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

            KAction execute_effect = new A_ExecuteEffect(eff.card.instanceId, eff.effectId, eff.card.owner.photonId);
            GM.actionManager.AddAction(execute_effect);
        }
    }

    bool finalCheck = false;

    public override bool IsComplete()
    {
        if(isInit && GM.actionManager.IsDone() && counterPlayed)
        {
            counterPlayed = false;

            int currentPlayerId = GM.currentPlayer.photonId;
            int localPlayerId = GM.localPlayer.photonId;
            int otherPlayerId = GM.clientPlayer.photonId;

            if (currentPlayerId == localPlayerId)
            {
                KAction giveControl = new A_GiveControl(phaseIndex, localPlayerId, false, otherPlayerId, false);
                GM.actionManager.AddAction(giveControl);
            }
        }

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

            GM.turn.turnFlags.RemoveTemporary();

            counterPlayed = false;
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

    public override void OnTurnButtonPress(Button button)
    {
        base.OnTurnButtonPress(button);

        int localPlayerId = GM.localPlayer.photonId;
        int otherPlayerId = GM.clientPlayer.photonId;

        if ((CheckPlayerIsReady(localPlayerId) && !GM.currentPlayer.isLocal) || CheckPlayerIsReady(otherPlayerId))
            return;

        KAction giveControl = new A_GiveControl(phaseIndex, localPlayerId, false, otherPlayerId);
        GM.actionManager.AddAction(giveControl);

        AudioManager.singleton.Play(SoundEffectType.BUTTON_CLICK);
    }

    public override void OnTurnButtonHold(Button button)
    {
        base.OnTurnButtonHold(button);

        int localPlayerId = GM.localPlayer.photonId;
        int otherPlayerId = GM.clientPlayer.photonId;

        if (CheckPlayerIsReady(localPlayerId))
            return;

        if (GM.localPlayer.playedCards.Exists(a => !a.EffectsDone() && a.GetCardType() is QuickPlay))
        {
            WarningPanel.singleton.ShowWarning("Aun tienes cartas <sprite=3> en juego sin activar", true);
            return;
        }

        KAction giveControl = new A_GiveControl(phaseIndex, localPlayerId, true, otherPlayerId);
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
            if(c.cardEffects.Find(a => a.type == EffectType.QUICK_COUNTER))
            {
                if (currentPlayerId == localPlayerId)
                {
                    counterPlayed = true;
                }

                LoadCardEffects(c);
                ExecuteEffect();
            }
            else
            {
                if (currentPlayerId == localPlayerId)
                {
                    KAction giveControl = new A_GiveControl(phaseIndex, localPlayerId, false, otherPlayerId, false);
                    GM.actionManager.AddAction(giveControl);
                }
            }
        }
    }
}
