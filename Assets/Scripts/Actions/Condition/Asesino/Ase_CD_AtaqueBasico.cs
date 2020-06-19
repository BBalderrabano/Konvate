using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/Asesino/Esquivar_AtaqueBasico")]
public class Ase_CD_AtaqueBasico : Condition
{
    public override bool isTemporary()
    {
        return true;
    }

    public override bool isValid(int photonId, int cardId)
    {
        PlayerHolder player = GameManager.singleton.GetPlayerHolder(photonId);

        for (int i = 0; i < player.playedCards.Count; i++)
        {
            if (player.playedCards[i].cardName == "Esquivar")
            {
                WarningPanel.singleton.ShowWarning("No puedes jugar Ataques Basicos con Esquivar en juego");
                return false;
            }
        }

        return true;
    }
}
