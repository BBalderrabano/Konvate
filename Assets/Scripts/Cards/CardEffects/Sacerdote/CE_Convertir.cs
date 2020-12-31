using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Sacerdote/Convertir")]
public class CE_Convertir : SelectionCardEffect
{
    bool shuffled = false;

    PlayerHolder opponent;

    Transform deckGrid = null;

    public override void Execute()
    {
        base.Execute();

        shuffled = false;

        if(deckGrid == null)
        {
            deckGrid = card.owner.currentHolder.deckGrid.value;
        }

        opponent = GM.GetOpponentHolder(card.owner.photonId);

        List<Card> availableCards = opponent.discardCards.FindAll(a => a.ModifiedCardCost() == 1);

        if (availableCards.Count > 0)
        {
            shuffled = true;

            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("<b>Baraja</b> una carta en tu mazo", availableCards, card.owner.photonId, this, card.instanceId));
            }
            else
            {
                GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
            }
        }
        else
        {
            Finish();
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            Card selected = opponent.GetCard(cardIds.First());

            GM.PreviewCard(selected);

            parentAction.LinkAnimation(GM.animationManager.MoveCard(parentAction.actionId, opponent.photonId, selected.instanceId, deckGrid.position,
                                                                                                                                     deckGrid.gameObject, 
                                                                                                                                     SoundEffectType.PICK_CARD_UP));

            opponent.deck.Remove(selected);
            opponent.discardCards.Remove(selected);
            opponent.handCards.Remove(selected);
            opponent.playedCards.Remove(selected);
            opponent.all_cards.Remove(selected);

            card.owner.deck.Remove(card);
            card.owner.discardCards.Remove(card);
            card.owner.handCards.Remove(card);
            card.owner.playedCards.Remove(card);
            card.owner.all_cards.Remove(card);

            card.owner.deck.Add(selected);
            card.owner.all_cards.Add(selected);

            selected.owner = card.owner;
            selected.cardPhysicalInst.setCurrentLogic(GM.resourcesManager.dataHolder.deckLogic);

            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_Shuffle(card.owner.photonId, false));
            }
        }

        Finish();
    }

    public override void Finish()
    {
        base.Finish();

        if (shuffled)
        {
            LeanTween.scale(card.cardPhysicalInst.gameObject, Vector3.zero, Settings.ANIMATION_TIME).setOnComplete(() =>
            {
                card.cardPhysicalInst.gameObject.SetActive(false);
            });
        }
    }
}
