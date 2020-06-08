using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Quick Play")]
public class QuickPlay : CardType
{
    public override void onSetType(CardViz viz)
    {
        viz.quickPlayIcon.gameObject.SetActive(true);
    }
}
