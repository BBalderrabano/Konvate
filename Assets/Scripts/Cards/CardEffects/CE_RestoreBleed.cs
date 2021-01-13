using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Restore HP")]
public class CE_RestoreBleed : CardEffect
{
    public int amount = 1;

    public bool skipsPreview = false;

    public override void Execute()
    {
        base.Execute();

        skipsEffectPreviewTime = skipsPreview;

        card.owner.ModifyHitPoints(amount);

        Finish();
    }
}
