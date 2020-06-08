using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Remove Combat Chips")]
public class CE_RemoveCombatChips : CardEffect
{
    public int amount;

    private PlayerHolder player;

    public override void Execute()
    {
        base.Execute();

        player = card.owner;

        parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, player.photonId, card.instanceId, ChipType.COMBAT_OFFENSIVE, amount));
    }
}
