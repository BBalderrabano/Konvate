using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vikingo/Drakkar")]
public class CE_Drakkar : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        List<Card> available = card.owner.handCards.FindAll(a => a.HasTags(CardTag.ATAQUE_BASICO) || a.HasTags(CardTag.DEFENSA));

        available.Add(card.owner.discardCards.Find(a => a.HasTags(CardTag.ATAQUE_BASICO)));
        available.Add(card.owner.discardCards.Find(a => a.HasTags(CardTag.DEFENSA)));

        for (int i = 0; i < available.Count; i++)
        {
            if (available[i] == null)
                continue;

            parentAction.PushAction(new A_ForcePlayCard(card.owner.photonId, available[i].instanceId));
        }
    }

    public override void Finish()
    {
        base.Finish();
    }
}
