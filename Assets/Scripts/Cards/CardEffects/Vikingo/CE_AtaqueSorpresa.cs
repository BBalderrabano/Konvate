using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vikingo/Ataque Sorpresa")]
public class CE_AtaqueSorpresa : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        int amount = Mathf.Min(1, GM.turn.turnFlags.GetFlag(opponent.photonId, FlagDesc.DISCARD_AMOUNT_BY_EFF).amount);

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));
    }
}
