using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Brujo/Maldicion text mod")]
public class TextMod_Maldicion : CardTextMod
{
    [TextArea(15, 20)]
    public string curse_active_text = "<color=#BA009C>Maldición</color>\nSi juegas esta carta tu oponente <color=#1E9D44>coloca </color>1 <sprite=1>\nSi descartas esta carta te <color=#880014>inflijes</color> 1<sprite=2>";
    [TextArea(15, 20)]
    public string curse_text = "<size=25>Al final del turno baraja esta carta en el mazo de un oponente\nSi otro jugador juega esta carta <color=#1E9D44>colocas </color>1 <sprite=1>\nSi otro jugador descarta esta carta le <color=#880014>inflijes</color> 1<sprite=2></size>";

    private bool text_changed = false;

    public override void Init(Card c)
    {
        base.Init(c);
        text_changed = false;

        original_text = curse_text;
        card.cardText = curse_text;
        card.cardViz.cardText.SetText(curse_text);
        card.cardViz.cardText.ForceMeshUpdate(true);
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
            card.cardText = curse_text;
            card.cardViz.cardText.SetText(curse_text);
            card.cardViz.cardText.ForceMeshUpdate(true);

            text_changed = false;
        }
    }
}
