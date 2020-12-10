using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Chaman/Encantar Arma")]
public class CE_EncantarArma : CardEffect
{
    public CardEffect tempPlaceCombatChip;

    public override void Execute()
    {
        base.Execute();

        List<Card> playedStrikes = card.owner.playedCards.FindAll(a => a.HasTags(CardTag.ATAQUE_BASICO));

        for (int i = 0; i < playedStrikes.Count; i++)
        {
            CardEffect clone = (CardEffect)tempPlaceCombatChip.Clone();

            clone.effectId = int.Parse((i + 1).ToString() + effectId.ToString());
            clone.card = playedStrikes[i];

            playedStrikes[i].cardEffects.Add(clone);
        }
    }
}
