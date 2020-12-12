using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Druida/Luz Lunar")]
public class CE_LuzLunar : CardEffect
{
    PlayerHolder player;

    public int amount = 1;

    public override void Execute()
    {
        base.Execute();

        player = card.owner;

        if (player.playedCards.Find(a => a.HasTags(new CardTags[] { CardTags.SHAPE_ENT })))
        {
            card.owner.ModifyHitPoints(amount);
        }

        Finish();
    }
}
