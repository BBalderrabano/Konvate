using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/Golem/Roca")]
public class Golem_CD_Roca : Condition
{
    public override bool isTemporary()
    {
        return false;
    }

    public override bool isValid(int photonId, int cardId = -1)
    {
        Card c = GameManager.singleton.GetCard(cardId);

        if(c.owner.photonId != c.photonId)
        {
            WarningPanel.singleton.ShowWarning("No puedes jugar esta carta");

            return false;
        }

        return true;
    }
}
