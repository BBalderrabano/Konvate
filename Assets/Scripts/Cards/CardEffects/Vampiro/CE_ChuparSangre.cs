using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vampiro/Chupar Sangre Heal")]
public class CE_ChuparSangre : CardEffect
{
    public int amount = 1;
    public Card formaMurcielago;

    public override void Execute()
    {
        skipsCardPreview = false;
        skipsEffectPreviewTime = false;
        
        base.Execute();

        if(card.owner.playedCards.Exists(a => a.cardName == formaMurcielago.cardName))
        {
            Finish();
        }
        else
        {
            skipsCardPreview = true;
            skipsEffectPreviewTime = true;

            card.owner.ModifyHitPoints(amount);
        }
    }
}
