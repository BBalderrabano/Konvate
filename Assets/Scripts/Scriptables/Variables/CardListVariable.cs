using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Card List Variable")]
public class CardListVariable : ScriptableObject
{
    public List<Card> values;

    public void Set(List<Card> v) {
        values = v;
    }

    public void Add(Card v)
    {
        values.Add(v);
    }
}
