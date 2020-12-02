using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Brujo/Detonar Maldicion text mod")]
public class TextMod_DetonarMaldicion : CardTextMod
{
    int curse_amount = -99;

    public override void Init(Card c)
    {
        base.Init(c);
    }

    public override void UpdateText()
    {
        int current_amount = GameManager.singleton.GetOpponentHolder(card.owner.photonId).discardCards.Where(a => a.HasTags(new CardTags[] { CardTags.CURSE_BRUJO } )).ToList().Count;

        if(current_amount != curse_amount)
        {
            string original_text_copy = original_text;
            string modified_text = original_text_copy.Replace(text_target, current_amount.ToString());

            card.cardText = modified_text;
            card.cardViz.cardText.SetText(modified_text);
            card.cardViz.cardText.ForceMeshUpdate(true);

            curse_amount = current_amount;
        }
    }
}
