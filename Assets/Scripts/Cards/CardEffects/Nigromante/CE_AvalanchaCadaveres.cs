using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Nigromante/Avalancha de Cadaveres")]
public class CE_AvalanchaCadaveres : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        int amount = card.owner.handCards.Count;

        List<KAction> discards = new List<KAction>();

        for (int i = 0; i < card.owner.handCards.Count; i++)
        {
            Card to_discard = card.owner.handCards[i];

            discards.Add(new A_Discard(to_discard.instanceId, to_discard.owner.photonId, true));
        }

        parentAction.PushActions(discards);
        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.POISON, amount));
    }
}
