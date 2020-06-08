using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Logics/Discard Card")]
public class DiscardCard : CardLogic
{
    public override void OnClick(CardInstance inst)
    {
        Debug.Log("Carta descartada");
    }

    public override void OnHighlight(CardInstance inst)
    {
    }

    public override void OnSetLogic(CardInstance inst)
    {
        inst.viz.cardBackImage.gameObject.SetActive(false);
    }
}
