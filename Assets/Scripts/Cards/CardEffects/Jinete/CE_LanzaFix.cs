using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Card Effects/Jinete/Lanza Fix")]
public class CE_LanzaFix : CardEffect
{
    public override void Execute()
    {
        card.MakeBorderActive();
    }
}
