using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.ObjectModel;

public class CollectionCardViz : MonoBehaviour, IPointerClickHandler
{
    public CardViz viz;

    public TMP_Text amount_counter;

    public CollectionNavigator collectionNavigator;

    public void OnPointerClick(PointerEventData eventData)
    {
        collectionNavigator.PreviewCard(viz.card, transform.position);
    }

    private void Start()
    {
        viz = GetComponent<CardViz>();
    }
}
