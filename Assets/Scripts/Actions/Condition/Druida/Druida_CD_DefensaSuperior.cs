using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/Druida/Defensa Superior")]
public class Druida_CD_DefensaSuperior : Condition
{
    public override bool isTemporary()
    {
        return false;
    }

    public override bool isValid(int photonId, int cardId)
    {
        PlayerHolder player = GameManager.singleton.GetPlayerHolder(photonId);

        for (int i = 0; i < player.playedCards.Count; i++)
        {
            if (player.playedCards[i].HasTags(new CardTags[] { CardTags.SHAPE_WOLF }))
            {
                WarningPanel.singleton.ShowWarning("No puede usarse si Forma de Lobo esta en juego");
                return false;
            }
        }

        return true;
    }
}
