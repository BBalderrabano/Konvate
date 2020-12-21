using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Golem/Roca text mod")]
public class TextMod_Roca : CardTextMod
{
    [TextArea(15, 20)]
    public string curse_active_text = "No puedes usar esta carta";

    private bool text_changed = false;

    public override void Init(Card c)
    {
        base.Init(c);
        text_changed = false;
    }

    public override void UpdateText()
    {
        if(card.owner.photonId != card.photonId && !text_changed)
        {
            card.cardText = curse_active_text;
            card.cardViz.cardText.SetText(curse_active_text);
            card.cardViz.cardText.ForceMeshUpdate(true);

            text_changed = true;
        }
        else if(card.owner.photonId == card.photonId && text_changed)
        {
            card.cardText = original_text;
            card.cardViz.cardText.SetText(original_text);
            card.cardViz.cardText.ForceMeshUpdate(true);

            text_changed = false;
        }
    }
}
