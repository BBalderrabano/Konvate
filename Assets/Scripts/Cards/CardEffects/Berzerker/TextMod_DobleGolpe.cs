using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Card Effects/Berzerker/TextMod Doble Golpe")]
public class TextMod_DobleGolpe : CardTextMod
{
    [System.NonSerialized]
    public Card copied_card;

    [System.NonSerialized]
    string original_name;

    public override void Init(Card c)
    {
        base.Init(c);

        original_name = c.cardName;
    }

    public override void UpdateText()
    {
        card.cardName = copied_card.cardName;
        card.cardViz.cardName.SetText(copied_card.cardName);

        card.cardText = copied_card.cardText;
        card.cardViz.cardText.SetText(copied_card.cardText);

        card.cardViz.cardText.ForceMeshUpdate(true);
    }

    public override void OnExpire()
    {
        card.cardName = original_name;
        card.cardViz.cardName.SetText(original_name);

        card.cardText = original_text;
        card.cardViz.cardText.SetText(original_text);

        card.cardViz.cardText.ForceMeshUpdate(true);
    }
}
