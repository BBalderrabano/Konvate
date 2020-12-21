using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Golem/Piel de Piedra Remove")]
public class CE_PielDePiedraRemove : CardEffect
{
    public int amount = 1;

    [System.NonSerialized]
    public bool removed = false;

    public override void Execute()
    {
        base.Execute();

        removed = false;

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        if (GM.GetChips(ChipType.COMBAT_OFFENSIVE, opponent, true).Count > 0)
        {
            removed = true;
        }

        parentAction.LinkAnimation(GM.animationManager.RemoveChip(parentAction.actionId, this, card.owner.photonId, card.instanceId, ChipType.COMBAT_OFFENSIVE, amount));
    }

    public override void Finish()
    {
        base.Finish();

        if (removed)
        {
            card.MakeBorderActive();
        }
    }
}
