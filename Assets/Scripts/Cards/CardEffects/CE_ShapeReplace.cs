using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Reemplazar Forma")]
public class CE_ShapeReplace : CardEffect
{
    public override void Execute()
    {
        List<Card> playedShapes = card.owner.playedCards.FindAll(a => a.HasTags(new CardTags[] { CardTags.SHAPE }.ToArray()));

        skipsEffectPreview = false;

        if (playedShapes.Count > 0)
        {
            foreach(Card c in playedShapes)
            {
                GM.actionManager.AddAction(new A_Discard(c.instanceId, c.owner.photonId));
            }
        }
        else
        {
            skipsEffectPreview = true;
            Finish();
        }
    }
}
