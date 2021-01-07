using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Sacerdote/Wololo")]
public class CE_Wololo : SelectionCardEffect
{
    public CardEffect returnToOwner;

    PlayerHolder opponent = null;
    Transform playedHolder = null;

    Card top_deck_card;

    bool isInit = false;

    public override void Execute()
    {
        base.Execute();

        if (opponent == null)
        {
            opponent = GM.GetOpponentHolder(card.owner.photonId);
        }

        if(playedHolder == null)
        {
            playedHolder = card.owner.currentHolder.playedGrid.value;
        }

        if (!isInit)
        {
            if (opponent.deck.Count > 0)
            {
                top_deck_card = opponent.deck[0];
                top_deck_card.owner = card.owner;

                top_deck_card.RevealCard();

                GM.PreviewCard(top_deck_card);

                parentAction.LinkAnimation(GM.animationManager.MoveCard(parentAction.actionId, opponent.photonId, top_deck_card.instanceId, playedHolder.position, playedHolder.gameObject, SoundEffectType.PLACE_CARD));

                opponent.deck.Remove(top_deck_card);
                opponent.all_cards.Remove(top_deck_card);

                card.owner.playedCards.Add(top_deck_card);
                card.owner.all_cards.Add(top_deck_card);

                if (card.owner.isLocal)
                {
                    GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId, opponent.photonId));
                }
                else
                {
                    parentAction.PushAction(new A_CardSelection("¿<color=#880014>Inflingirte</color> 1 <sprite=2> para enviar tu \"" + top_deck_card.cardName + "\" al descarte?", top_deck_card, opponent.photonId, this, card.instanceId).ModifyParameters(true, false, 0, 0));
                }
            }
            else
            {
                top_deck_card = null;

                Finish();
            }

            isInit = true;
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if(cardIds != null)
        {
            Card c = GM.GetCard(cardIds.First());

            opponent.all_cards.Add(c);

            card.owner.playedCards.Remove(c);
            card.owner.all_cards.Remove(c);
            c.owner = GM.GetPlayerHolder(c.photonId);

            parentAction.LinkAnimation(GM.animationManager.DirectDamageBleedChip(parentAction.actionId, card.owner.photonId, card.instanceId, opponent.photonId));
            parentAction.PushAction(new A_Discard(c.instanceId, c.owner.photonId));
        }
        else
        {
            if (top_deck_card != null)
            {
                CardEffect clone = (CardEffect) returnToOwner.Clone();

                clone.effectId = int.Parse((9).ToString() + effectId.ToString());
                clone.card = top_deck_card;

                top_deck_card.cardEffects.Add(clone);

                Finish();

                if(top_deck_card.GetCardType() is QuickPlay)
                {
                    GM.turn.currentPhase.value.OnPlayCard(top_deck_card);
                }
            }
        }
    }

    public override void Finish()
    {
        base.Finish();
        isInit = false;
    }
}
