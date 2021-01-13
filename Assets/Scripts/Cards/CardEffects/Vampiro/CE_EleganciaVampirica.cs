using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vampiro/Elegancia Vampirica Selection")]
public class CE_EleganciaVampirica : SelectionCardEffect
{
    public CE_EleganciaVampiricaTransfer transferEffect;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        transferEffect.new_owner = opponent;

        List<Card> available = new List<Card>();

        available.AddRange(opponent.playedCards);

        available.RemoveAll(a => a.HasTags(CardTag.ATAQUE_BASICO) || a.HasTags(CardTag.DEFENSA) || a.GetCardType() is QuickPlay || a.EffectsDone() || a.isBroken);

        if(available.Count == 0)
        {
            Finish();
        }
        else
        {
            if (card.owner.isLocal)
            {
                parentAction.PushAction(new A_CardSelection("Toma <b>control</b> de una carta por el turno", available, card.owner.photonId, this, card.instanceId));
            }
            else
            {
                GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
            }
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            Card c = GM.GetCard(cardIds.First());
            CardEffect transfer = transferEffect.Clone(c, false);

            PlayerHolder opponent = c.owner;

            parentAction.LinkAnimation(GM.animationManager.MoveCard(parentAction.actionId, c.owner.photonId, cardIds.First(), 
                                                                    card.owner.currentHolder.playedGrid.value.position, 
                                                                    card.owner.currentHolder.playedGrid.value.gameObject));

            opponent.playedCards.Remove(c);
            opponent.all_cards.Remove(c);

            c.owner = card.owner;

            card.owner.playedCards.Add(c);
            card.owner.all_cards.Add(c);

            c.cardEffects.Add(transfer);
        }
    }
}
