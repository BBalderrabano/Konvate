using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Asesino/Ataque Premeditado")]
public class CE_AtaquePremeditado : SelectionCardEffect
{
    public CardEffect maintainEffect;

    public override void Execute()
    {
        base.Execute();

        if (card.owner.isLocal)
        {
            List<Card> handCards = card.owner.handCards;

            handCards.RemoveAll(a => a.cardEffects.Exists(b => b.type == EffectType.MAINTAIN));

            parentAction.PushAction(new A_CardSelection("Puedes <b>mantener</b> una carta", handCards, card.owner.photonId, this, card.instanceId));
        }
        else
        {
            GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            for (int i = 0; i < cardIds.Length; i++)
            {
                Card selected = GM.GetPlayerHolder(card.owner.photonId).GetCard(cardIds[i]);
                selected.cardEffects.Add(maintainEffect);
            }
        }

        Finish();
    }
}
