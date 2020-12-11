using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Enano/Descuento y Relampago a Defs")]
public class CE_QuickAndCheapDefends : CardEffect
{
    SMOD_ZeroCost energyDiscount = new SMOD_ZeroCost();
    SMOD_QuickPlay makeQuickPlay = new SMOD_QuickPlay();

    public override void Execute()
    {
        base.Execute();

        PlayerHolder player = GM.GetPlayerHolder(card.owner.photonId);

        for (int i = 0; i < player.all_cards.Count; i++)
        {
            if ((player.all_cards[i].HasTags(CardTag.DEFENSA) ||
                    player.all_cards[i].HasTags(CardTag.DEFENSA_SUPERIOR))
                        && !player.all_cards[i].statMods.Contains(energyDiscount))
            {
                player.all_cards[i].statMods.Add(energyDiscount);
                player.all_cards[i].statMods.Add(makeQuickPlay);

                player.all_cards[i].cardViz.RefreshStats();
            }
        }

        Finish();
    }
}
