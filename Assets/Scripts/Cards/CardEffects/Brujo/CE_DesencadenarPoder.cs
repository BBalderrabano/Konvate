using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Brujo/Desencadenar Poder")]
public class CE_DesencadenarPoder : CardEffect
{
    public int amount;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        parentAction.PushActions(GM.DrawCard(card.owner, amount, card.instanceId, effectId, parentAction));
        parentAction.PushActions(GM.DrawCard(opponent, amount, card.instanceId, effectId, parentAction));
    }
}
