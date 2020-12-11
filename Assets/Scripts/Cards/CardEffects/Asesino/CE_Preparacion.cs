using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Card Effects/Asesino/Preparacion")]
public class CE_Preparacion : CardEffect
{
    public int amount;

    public override void Execute()
    {
        base.Execute();

        SMOD_ZeroCost zeroCost = new SMOD_ZeroCost();

        for (int i = 0; i < card.owner.all_cards.Count; i++)
        {
            if (card.owner.all_cards[i].cardName == card.cardName
                && card.owner.all_cards[i].instanceId != card.instanceId
                && !card.statMods.Contains(zeroCost))
            {
                card.owner.all_cards[i].statMods.Add(zeroCost);
                card.owner.all_cards[i].cardViz.RefreshStats();
            }
        }

        parentAction.PushActions(GM.DrawCard(card.owner, amount, card.instanceId, effectId, parentAction));
    }
}
