using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Place Combat Chip")]
public class CE_PlaceCombatChips : CardEffect
{
    public int amount;

    private PlayerHolder player;

    public override void Execute()
    {
        base.Execute();

        player = card.owner;

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, player.photonId, card.instanceId, ChipType.COMBAT, amount));
    }
}
