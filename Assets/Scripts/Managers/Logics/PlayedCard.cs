using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Logics/Played Card")]
public class PlayedCard : CardLogic
{
    public override void OnClick(CardInstance inst)
    {
    }

    public override void OnHighlight(CardInstance inst)
    {
    }

    public override void OnSetLogic(CardInstance inst)
    {
        inst.viz.cardBackImage.gameObject.SetActive(false);
    }
}
