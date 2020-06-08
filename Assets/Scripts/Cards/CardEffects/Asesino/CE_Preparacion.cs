using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Card Effects/Asesino/Preparacion")]
public class CE_Preparacion : CardEffect
{
    public int amount;
    public StatModification comboDiscount;

    public override void Execute()
    {
        base.Execute();

        for (int i = 0; i < card.owner.all_cards.Count; i++)
        {
            if (card.owner.all_cards[i].cardName == card.cardName
                && card.owner.all_cards[i].instanceId != card.instanceId
                && !card.cardEffects.Contains(comboDiscount))
            {

                card.owner.all_cards[i].cardEffects.Add(comboDiscount);
                card.owner.all_cards[i].cardViz.RefreshStats();
            }
        }

        parentAction.PushActions(GM.DrawCard(card.owner, amount, card.instanceId, effectId, parentAction));
    }
}
