using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(menuName = "Card Effects/Asesino/Preparacion Combo")]
public class CE_PreparacionCombo : CardEffect
{
    public override void Execute()
    {
        IsCombo();
        isDone = true;
    }

}
