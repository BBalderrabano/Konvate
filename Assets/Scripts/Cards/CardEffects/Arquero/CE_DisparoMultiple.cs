using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Arquero/Disparo Multiple")]
public class CE_DisparoMultiple : CardEffect
{
    public SMOD_ZeroCost arrowDiscount;
    public SMOD_QuickPlay arrowQuickPlay;
    public Card discountTarget;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder player = card.owner;

        for (int i = 0; i < player.all_cards.Count; i++)
        {
            if (player.all_cards[i].cardName == discountTarget.cardName
                && !player.all_cards[i].cardEffects.Contains(arrowDiscount))
            {
                player.all_cards[i].cardEffects.Add(arrowDiscount);
                player.all_cards[i].cardEffects.Add(arrowQuickPlay);

                player.all_cards[i].cardViz.RefreshStats();
            }
        }

        Finish();
    }
}
