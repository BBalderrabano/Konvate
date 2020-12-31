using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Sacerdote/Pacificar")]
public class CE_Pacificar : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        GM.GetOpponentHolder(card.owner.photonId).statMods.Add(new SMOD_FistToBleed());
        card.owner.statMods.Add(new SMOD_FistToBleed());
    }
}
