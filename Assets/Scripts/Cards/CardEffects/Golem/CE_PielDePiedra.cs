using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Golem/Piel de Piedra")]
public class CE_PielDePiedra : CardEffect
{
    public CardEffect prevail;

    [System.NonSerialized]
    public CE_PielDePiedraRemove remove = null;

    public override void Execute()
    {
        base.Execute();

        if(remove == null)
        {
            remove = card.cardEffects.OfType<CE_PielDePiedraRemove>().First();
        }

        parentAction.MakeActiveOnComplete(false);
        skipsEffectPreviewTime = true;

        if (!remove.removed && card.owner.isFloatingDefend(card.instanceId))
        {
            CardEffect clone = prevail.Clone(card);

            card.cardEffects.Add(clone);

            card.MakeBorderInactive();
        }
    }
}
