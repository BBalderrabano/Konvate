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
        PlayerHolder player = GameManager.singleton.GetPlayerHolder(photonId);

        for (int i = 0; i < player.playedCards.Count; i++)
        {
            if (player.playedCards[i].HasTags(CardTag.ATAQUE_BASICO))
            {
                WarningPanel.singleton.ShowWarning("No puedes jugar esta carta con Ataques Basicos en juego");
                return false;
            }
        }

        return true;
    }
}
