using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Logics/Deck Card")]
public class DeckLogic : CardLogic
{
    public override void OnClick(CardInstance inst)
    {
        PlayerHolder player = inst.viz.card.owner;

        if (player.isLocal)
        {
            List<Card> deckCards = player.deck.ToList();

            deckCards.Shuffle();
            deckCards.Shuffle();
            deckCards.Shuffle();

            ScrollSelectionManager.singleton.SelectCards(deckCards, "Cartas en tu mazo", true);
        }
        else
        {
            WarningPanel.singleton.ShowWarning(player.playerName + " tiene " + player.deck.Count + " cartas en su mazo");
        }
    }

    public override void OnHighlight(CardInstance inst)
    {
    }

    public override void OnSetLogic(CardInstance inst)
    {
        inst.viz.cardBackImage.gameObject.SetActive(true);
    }
}
