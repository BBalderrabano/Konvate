using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Logics/Discard Card")]
public class DiscardCard : CardLogic
{
    public override void OnClick(CardInstance inst)
    {
        PlayerHolder player = inst.viz.card.owner;

        List<Card> discardCards = player.discardCards.ToList();

        discardCards.Reverse();

        ScrollSelectionManager.singleton.SelectCards(discardCards, "Descarte de " + player.playerName, true);
    }

    public override void OnHighlight(CardInstance inst)
    {
    }

    public override void OnSetLogic(CardInstance inst)
    {
        inst.viz.cardBackImage.gameObject.SetActive(false);
    }
}
