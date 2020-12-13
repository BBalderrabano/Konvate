using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Goblin/Barril Apestoso")]
public class CE_BarrilApestoso : CardEffect
{
    int base_amount = 1;

    public override void Execute()
    {
        base.Execute();

        int copies_amount = base_amount + card.owner.handCards.FindAll(a => a.cardName == card.cardName).Count;

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.POISON, copies_amount));
    }
}
