using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Vikingo/Pillaje")]
public class CE_Pillaje : CardEffect
{
    PlayerHolder opponent = null;

    public override void Execute()
    {
        base.Execute();

        opponent = GM.GetOpponentHolder(card.owner.photonId);

        List<Card> available = opponent.handCards.ToList();

        foreach(Card copies in card.owner.all_cards.FindAll(a => a.cardName == card.cardName))
        {
            copies.statMods.Add(new SMOD_ZeroCost());
            copies.cardViz.RefreshStats();
        }

        if(available.Count > 0)
        {
            if(available.Count == 1)
            {
                parentAction.PushAction(new A_Discard(available.First().instanceId, opponent.photonId, true));
            }
            else
            {
                int n = Random.Range(0, available.Count);
                parentAction.PushAction(new A_Discard(available[n].instanceId, opponent.photonId, true));
            }
        }
        else
        {
            Finish();
        }
    }
}
