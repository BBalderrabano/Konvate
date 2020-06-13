using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/Asesino/Esquivar")]
public class Ase_CD_Esquivar : Condition
{
    public override bool isTemporary()
    {
        return false;
    }

    public override bool isValid(int photonId, int cardId)
    {
        PlayerHolder player = GameManager.singleton.getPlayerHolder(photonId);

        for (int i = 0; i < player.playedCards.Count; i++)
        {
            Debug.Log(player.playedCards[i].cardName + " " + player.playedCards[i].HasTags(CardTag.ATAQUE_BASICO));

            if (player.playedCards[i].HasTags(CardTag.ATAQUE_BASICO))
            {
                WarningPanel.singleton.ShowWarning("No puedes jugar esta carta con Ataques Basicos en juego");
                return false;
            }
        }

        return true;
    }
}
