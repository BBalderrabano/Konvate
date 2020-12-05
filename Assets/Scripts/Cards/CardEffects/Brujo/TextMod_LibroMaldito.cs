using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Brujo/Libro Maldito text mod")]
public class TextMod_LibroMaldito : CardTextMod
{
    [TextArea(15, 20)]
    public string curse_active_text = "<size=30><color=#BA009C>Maldición</color>\nSi juegas esta carta tu oponente <color=#1E9D44>coloca </color>1 <sprite=1>\nSi descartas esta carta te <color=#880014>inflijes</color> 1<sprite=2></size>";
    [TextArea(15, 20)]
    public string curse_text = "<size=21.2>Al final del turno baraja esta carta en el mazo de un oponente\nSi otro jugador juega esta carta <color=#1E9D44>colocas </color>1 <sprite=1>\nSi otro jugador descarta esta carta <color=#880014>inflijes</color> 1<sprite=2>\nReduce el coste de una Maldición de tu mano a <sprite=7> este turno</size>";

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
