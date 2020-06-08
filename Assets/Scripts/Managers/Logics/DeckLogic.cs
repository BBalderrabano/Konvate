using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Logics/Deck Card")]
public class DeckLogic : CardLogic
{
    public override void OnClick(CardInstance inst)
    {
        Debug.Log("Carta en mazo");
    }

    public override void OnHighlight(CardInstance inst)
    {
    }

    public override void OnSetLogic(CardInstance inst)
    {
        inst.viz.cardBackImage.gameObject.SetActive(true);
    }
}
