using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Pestilente/Explosion Venenosa")]
public class CE_ExplosionVenenosa : CardEffect
{
    public SMOD_ZeroCost poisonDiscount;
    public SMOD_QuickPlay poisonQuickPlay;
    public CardEffect placePoison;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder player = GameManager.singleton.getPlayerHolder(card.owner.photonId);

        for (int i = 0; i < player.all_cards.Count; i++)
        {
            if (player.all_cards[i].HasTags(new CardTags[] { CardTags.PES_EXPLO_VENENO_TARGET })
                && !player.all_cards[i].cardEffects.Contains(poisonDiscount))
            {
                CardEffect clone = (CardEffect)placePoison.Clone();

                clone.effectId = int.Parse((i+1).ToString() + effectId.ToString());
                clone.card = player.all_cards[i];

                player.all_cards[i].cardEffects.Add(poisonDiscount);
                player.all_cards[i].cardEffects.Add(poisonQuickPlay);
                player.all_cards[i].cardEffects.Add(clone);

                player.all_cards[i].cardViz.RefreshStats();
            }
        }

        Finish();
    }
}
