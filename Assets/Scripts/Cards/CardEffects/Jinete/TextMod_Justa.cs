using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Jinete/Justa text mod")]
public class TextMod_Justa : CardTextMod
{
    int discard_pile_amount = -99;

    public override void Init(Card c)
    {
        base.Init(c);

        discard_pile_amount = -99;
    }

    public override void UpdateText()
    {
        int current_amount = Mathf.Abs(card.owner.discardCards.Count - GameManager.singleton.GetOpponentHolder(card.owner.photonId).discardCards.Count);

        if(current_amount != discard_pile_amount)
        {
            string original_text_copy = original_text;
            string modified_text = original_text_copy.Replace(text_target, current_amount.ToString());

            card.cardText = modified_text;
            card.cardViz.cardText.SetText(modified_text);
            card.cardViz.cardText.ForceMeshUpdate(true);

            discard_pile_amount = current_amount;
        }
    }
}
