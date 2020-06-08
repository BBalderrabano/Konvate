using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Normal Play")]
public class NormalPlay : CardType
{
    public override void onSetType(CardViz viz)
    {
        viz.quickPlayIcon.gameObject.SetActive(false);
    }
}
