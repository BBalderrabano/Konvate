using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Samurai/Shuriken")]
public class CE_Shuriken : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        int amount = opponent.playedCards.FindAll(a => a.GetCardType() is QuickPlay).Count;

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.COMBAT, Mathf.Max(1, amount)));
    }
}
