using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Comodin")]
public class CE_Joker : CardEffect
{
    public bool skipsPreview = true;

    public override void Execute()
    {
        skipsEffectPreview = skipsPreview;
    }
}
