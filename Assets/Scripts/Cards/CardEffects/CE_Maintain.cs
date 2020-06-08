using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Maintain")]
public class CE_Maintain : CardEffect
{
    public override void Execute()
    {
        base.Execute();
        isDone = true;
    }
}
