using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Orco/Fuerza en Numeros")]
public class CE_FuerzaEnNumeros : CardEffect
{
    public int combat_chip_amount = 1;

    public override void Execute()
    {
        base.Execute();

        List<Card> played_cards = GM.GetOpponentHolder(card.owner.photonId).playedCards;

        List<Card> defense = new List<Card>();

        defense.AddRange(played_cards.Where(a => a.HasTags(CardTag.DEFENSA)).ToList());
        defense.AddRange(played_cards.Where(a => a.HasTags(CardTag.DEFENSA_SUPERIOR)).ToList());

        if(defense.Count == 0)
        {
            PlayerHolder player = card.owner;

            GM.RevealCard(card);

            player.handCards.Remove(card);
            player.playedCards.Add(card);

            card.cardPhysicalInst.gameObject.SetActive(true);

            parentAction.PushAction(new Anim_FuerzaEnNumeros(player.photonId, card.instanceId, combat_chip_amount));
        }
        else
        {
            card.cardPhysicalInst.viz.cardBorder.color = Color.black;
            GM.HidePreviewCard();
        }
    }
}


