using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectionCardViz : MonoBehaviour
{
    public CardViz viz;

    [SerializeField]
    TMP_Text amount_counter;

    private void Start()
    {
        viz = GetComponent<CardViz>();
    }
}
