using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Berzerker/Venganza")]
public class CE_Venganza : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        int amount = 1;

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        if(card.owner.playedCards.Exists(a => a.GetCardType() is QuickPlay) || opponent.playedCards.Exists(a => a.GetCardType() is QuickPlay)) 
        {
            amount = 2;
        }

        parentAction.PushActions(GM.DrawCard(card.owner, amount, card.instanceId, effectId, parentAction));
    }
}
