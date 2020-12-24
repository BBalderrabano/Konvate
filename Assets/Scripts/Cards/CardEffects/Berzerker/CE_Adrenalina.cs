using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Berzerker/Adrenalina")]
public class CE_Adrenalina : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        card.owner.ChangeMana(1);
    }
}
