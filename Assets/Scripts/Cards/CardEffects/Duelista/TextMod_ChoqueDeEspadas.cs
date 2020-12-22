using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Duelista/TextMod Choque de Espadas")]
public class TextMod_ChoqueDeEspadas : CardTextMod
{
    [TextArea(15, 20)]
    public string text_mod = "\n<color=#036bfc>Remueve </color>1 <sprite=0>";

    public override void Init(Card c)
    {
        base.Init(c);
    }

    public override void UpdateText()
    {
        card.cardViz.cardText.SetText(original_text + text_mod);
        card.cardViz.cardText.ForceMeshUpdate(true);
    }
}
