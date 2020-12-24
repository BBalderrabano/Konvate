using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Duelista/Desarmar")]
public class CE_Desarmar : CardEffect
{
    public int draw_amount = 2;
    public Condition quickPlayCondition;

    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        for (int i = 0; i < opponent.all_cards.Count; i++)
        {
            opponent.all_cards[i].conditions.Add(quickPlayCondition);
        }

        if (IsCombo())
        {
            GM.DrawCard(card.owner, draw_amount);
        }
    }
}
