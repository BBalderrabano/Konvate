using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Logics/Opponent Hand Card")]
public class OpponentHandCard : CardLogic
{
    public override void OnClick(CardInstance inst)
    {
    }

    public override void OnHighlight(CardInstance inst)
    {
    }

    public override void OnSetLogic(CardInstance inst)
    {
        inst.viz.cardBackImage.gameObject.SetActive(true);
    }
}
