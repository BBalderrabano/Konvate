﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Brujo/Libro Maldito Reduccion")]
public class CE_LibroMalditoCostReduction : CardEffect
{
    public SMOD_ZeroCost curseDiscount;

    public override void Execute()
    {
        base.Execute();

        skipsEffectPreview = true;

        if (card.owner.photonId == card.photonId)
        {
            PlayerHolder player = card.owner;
            List<Card> maldicion = player.handCards.FindAll(a => a.HasTags(new CardTags[] { CardTags.CURSE_BRUJO_COST_TARGET }) && !a.cardEffects.Contains(curseDiscount));

            if (maldicion != null && maldicion.Count > 0)
            {
                maldicion.First().cardEffects.Add(curseDiscount);
                maldicion.First().cardViz.RefreshStats();
            }
        }
        
        Finish();
    }
}