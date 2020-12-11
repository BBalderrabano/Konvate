using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Arquero/Disparo Multiple")]
public class CE_DisparoMultiple : CardEffect
{
    public Card discountTarget;

    SMOD_ZeroCost arrowDiscount = new SMOD_ZeroCost();
    SMOD_QuickPlay arrowQuickPlay = new SMOD_QuickPlay();

    public override void Execute()
    {
        base.Execute();

        PlayerHolder player = card.owner;

        for (int i = 0; i < player.all_cards.Count; i++)
        {
            if (player.all_cards[i].cardName == discountTarget.cardName
                && !player.all_cards[i].statMods.Contains(arrowDiscount))
            {
                player.all_cards[i].statMods.Add(arrowDiscount);
                player.all_cards[i].statMods.Add(arrowQuickPlay);

                player.all_cards[i].cardViz.RefreshStats();
            }
        }

        Finish();
    }
}
