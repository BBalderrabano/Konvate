using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Brujo/Libro Maldito Reduccion")]
public class CE_LibroMalditoCostReduction : CardEffect
{
    public SMOD_ZeroCost curseDiscount = new SMOD_ZeroCost();

    public override void Execute()
    {
        base.Execute();

        skipsEffectPreviewTime = true;

        if (card.owner.photonId == card.photonId)
        {
            PlayerHolder player = card.owner;
            List<Card> maldicion = player.handCards.FindAll(a => a.HasTags(new CardTags[] { CardTags.CURSE_BRUJO_COST_TARGET }) && !a.statMods.Contains(curseDiscount));

            if (maldicion != null && maldicion.Count > 0)
            {
                maldicion.First().statMods.Add(curseDiscount);
                maldicion.First().cardViz.RefreshStats();
            }
        }
        
        Finish();
    }
}
