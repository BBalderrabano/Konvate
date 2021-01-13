using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Gladiador/No estan entretenidos Place")]
public class CE_NoEstanEntretenidosPlace : CardEffect
{
    [System.NonSerialized]
    public int start_turn = 0;

    public override void Execute()
    {
        base.Execute();

        int amount = Mathf.Abs(start_turn - GM.turn.turnCount);

        skipsEffectPreviewTime = (amount > 0);

        if(amount > 0)
        {
            parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, amount));
        }
    }
}
