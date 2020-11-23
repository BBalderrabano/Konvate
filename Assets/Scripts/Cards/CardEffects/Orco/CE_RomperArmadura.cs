using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Card Effects/Orco/Romper Armadura")]
public class CE_RomperArmadura : CardEffect
{
    public override void Execute()
    {
        List<Card> played_cards = GM.GetOpponentHolder(card.owner.photonId).playedCards;

        List<Card> defense = new List<Card>();

        defense.AddRange(played_cards.Where(a => a.HasTags(CardTag.DEFENSA)).ToList());
        defense.AddRange(played_cards.Where(a => a.HasTags(CardTag.DEFENSA_SUPERIOR)).ToList());

        foreach(Card selected in defense)
        {
            parentAction.PushAction(new A_Discard(selected.instanceId, selected.owner.photonId));
        }
    }
}
