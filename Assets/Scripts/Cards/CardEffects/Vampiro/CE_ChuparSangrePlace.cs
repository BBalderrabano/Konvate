using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vampiro/Chupar Sangre Place")]
public class CE_ChuparSangrePlace : CardEffect
{
    public int bonus = 1;
    public int amount = 1;

    public Card formaMonstruosa;

    public override void Execute()
    {
        base.Execute();

        int n = amount;

        if(card.owner.playedCards.Exists(a => a.cardName == formaMonstruosa.cardName))
        {
            n += bonus;
        }

        parentAction.LinkAnimation(GM.animationManager.PlaceChip(parentAction.actionId, card.owner.photonId, card.instanceId, ChipType.BLEED, n));
    }
}
