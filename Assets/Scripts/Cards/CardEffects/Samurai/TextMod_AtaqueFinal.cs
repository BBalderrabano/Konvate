using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Samurai/TextMod Ataque Final")]
public class TextMod_AtaqueFinal : CardTextMod
{
    public string text = "\n<b><i>(<color=#FF0000>Coloca </color>%% <sprite=0>)</i><b>";

    int hp_difference = -99;

    public override void Init(Card c)
    {
        base.Init(c);

        hp_difference = -99;
    }

    public override void UpdateText()
    {
        int current_amount = Mathf.Abs(card.owner.maxHealth - card.owner.bleedCount);

        if(current_amount <= 0)
        {
            card.cardText = original_text;
            card.cardViz.cardText.SetText(original_text);
            card.cardViz.cardText.ForceMeshUpdate(true);
        }
        else if (current_amount != hp_difference)
        {
            string original_text_copy = original_text + "" + text;
            string modified_text = original_text_copy.Replace(text_target, current_amount.ToString());

            card.cardText = modified_text;
            card.cardViz.cardText.SetText(modified_text);
            card.cardViz.cardText.ForceMeshUpdate(true);

            hp_difference = current_amount;
        }
    }
}
