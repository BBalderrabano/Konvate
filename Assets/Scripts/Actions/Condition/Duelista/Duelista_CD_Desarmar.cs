using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/Duelista/Desarmar")]
public class Duelista_CD_Desarmar : Condition
{
    public override bool isTemporary()
    {
        return true;
    }

    public override bool isValid(int photonId, int cardId)
    {
        if (GameManager.singleton.GetCard(cardId).GetCardType() is QuickPlay)
        {
            WarningPanel.singleton.ShowWarning("No puedes jugar cartas <sprite=3> este turno\n(tu oponente jugó <b>Desarmar</b>)");
            return false;
        }

        return true;
    }
}
