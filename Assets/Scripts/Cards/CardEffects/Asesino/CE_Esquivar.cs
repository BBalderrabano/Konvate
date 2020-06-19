using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Asesino/Esquivar")]
public class CE_Esquivar : CardEffect
{
    public CardEffect removeCards;
    public Condition basicAttackCondition;

    public override void Execute()
    {
        PlayerHolder player = card.owner;

        for (int i = 0; i < player.all_cards.Count; i++)
        {
            if (player.all_cards[i].HasTags(CardTag.ATAQUE_BASICO))
            {
                player.all_cards[i].conditions.Add(basicAttackCondition);
            }
        }

        Finish();
    }
}
