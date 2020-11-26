using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Jinete/Justa combat chips")]
public class CE_Justa_CombatChips : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder player = card.owner;

        int amount = Mathf.Abs(card.owner.discardCards.Count - GM.GetOpponentHolder(card.owner.photonId).discardCards.Count);

        if(amount > 0)
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, player.photonId, card.instanceId, ChipType.COMBAT, amount));
        }
    }
}
