using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Turns/Phases/Place Phase")]
public class PlacePhase : Phase
{
    List<CardEffect> place = new List<CardEffect>();
    List<CardEffect> special = new List<CardEffect>();

    void LoadCardEffects()
    {
        int offensivePhotonId = GM.turn.offensivePlayer.photonId;

        foreach (PlayerHolder player in GM.allPlayers)
        {
            foreach (Card c in player.playedCards)
            {
                c.RevealCard();

                if (c.isBroken) { continue; }

                for (int i = 0; i < c.cardEffects.Count; i++)
                {
                    if (c.cardEffects[i].isDone)
                        continue;

                    if (c.cardEffects[i].type == EffectType.PLACE)
                    {
                        place.Add(c.cardEffects[i]);
                    }
                    if (c.cardEffects[i].type == EffectType.SPECIAL)
                    {
                        special.Add(c.cardEffects[i]);
                    }
                }
            }

            foreach (Card c in player.handCards)
            {
                foreach (CE_HandCheck hand_effect in c.cardEffects.OfType<CE_HandCheck>())
                {
                    if (hand_effect.linkedCardEffect.type == EffectType.PLACE)
                    {
                        place.Add(hand_effect.linkedCardEffect);
                    }

                    if (hand_effect.linkedCardEffect.type == EffectType.SPECIAL)
                    {
                        special.Add(hand_effect.linkedCardEffect);
                    }
                }
            }
        }

        place = place.OrderBy(a => a.priority).ThenBy(a => (a.card.owner.photonId != offensivePhotonId)).ToList();
        special = special.OrderBy(a => a.priority).ThenBy(a => (a.card.owner.photonId != offensivePhotonId)).ToList();
    }

    void ExecuteEffects()
    {
        foreach (CardEffect eff in special)
        {
            if (eff.isDone) { continue; }

            KAction special_effect = new A_ExecuteEffect(eff.card.instanceId, eff.effectId, eff.card.owner.photonId);
            GM.actionManager.AddAction(special_effect);
        }

        foreach (CardEffect eff in place)
        {
            if (eff.isDone) { continue; }

            KAction place_effect = new A_ExecuteEffect(eff.card.instanceId, eff.effectId, eff.card.owner.photonId);
            GM.actionManager.AddAction(place_effect);
        }
    }

    public override bool IsComplete()
    {
        return isInit && GM.actionManager.IsDone();
    }


    public override void OnStartPhase()
    {
        if (!isInit)
        {
            MultiplayerManager.singleton.PlayerIsNotReady();

            GM.currentPlayer = null;
            GM.SetState(null);
            GM.onPhaseChange.Raise();
            GM.onPhaseControllerChange.Raise();
            GM.clientPlayer.playerUI.UpdateAll();           //Removes '?'

            place.Clear();
            special.Clear();

            LoadCardEffects();

            ExecuteEffects();

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
        return false;
    }

    public override void OnTurnButtonPress(Button button)
    {
    }
    public override void OnTurnButtonHold(Button button)
    {
    }

    public override void OnPhaseControllerChange(int photonId)
    {
    }

    public override void OnPlayCard(Card c)
    {
        int offensivePhotonId = GM.turn.offensivePlayer.photonId;

        for (int i = 0; i < c.cardEffects.Count; i++)
        {
            if (c.cardEffects[i].isDone)
                continue;

            if (c.cardEffects[i].type == EffectType.PLACE)
            {
                place.Add(c.cardEffects[i]);
            }
            if (c.cardEffects[i].type == EffectType.SPECIAL)
            {
                special.Add(c.cardEffects[i]);
            }
        }

        place = place.OrderBy(a => a.priority).ThenBy(a => (a.card.owner.photonId != offensivePhotonId)).ToList();
        special = special.OrderBy(a => a.priority).ThenBy(a => (a.card.owner.photonId != offensivePhotonId)).ToList();

        for (int i = 0; i < c.cardEffects.Count; i++)
        {
            if (c.cardEffects[i].isDone)
                continue;

            int place_index = place.FindIndex(a => a.effectId == c.cardEffects[i].effectId);
            int special_index = special.FindIndex(a => a.effectId == c.cardEffects[i].effectId);

            if(place_index > 0)
            {
                KAction previousAction = GM.actionManager.GetActionByEffect(place[Mathf.Max(0, place_index - 1)].effectId);

                KAction place_action = new A_ExecuteEffect(c.cardEffects[i].card.instanceId, c.cardEffects[i].effectId, c.cardEffects[i].card.owner.photonId);

                if (previousAction != null)
                {
                    GM.actionManager.PushAction(previousAction.actionId, place_action);
                }
                else
                {
                    GM.actionManager.PushActionToStart(place_action);
                }
            }

            if(special_index > 0)
            {
                KAction previousAction = GM.actionManager.GetActionByEffect(special[Mathf.Max(0, special_index - 1)].effectId);

                KAction special_action = new A_ExecuteEffect(c.cardEffects[i].card.instanceId, c.cardEffects[i].effectId, c.cardEffects[i].card.owner.photonId);

                if (previousAction != null)
                {
                    GM.actionManager.PushAction(previousAction.actionId, special_action);
                }
                else
                {
                    GM.actionManager.PushActionToStart(special_action);
                }
            }
        }
    }
}
