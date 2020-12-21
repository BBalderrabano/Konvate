using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Golem/Defensa Superior")]
public class CE_GolemDefSup : CardEffect
{
    public int amount = 2;

    public override void Execute()
    {
        base.Execute();

        parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, card.owner.photonId, card.instanceId, ChipType.COMBAT_OFFENSIVE, amount));

        if (IsCombo())
        {
            parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, card.owner.photonId, card.instanceId, ChipType.POISON, 20));
        }
    }
}
