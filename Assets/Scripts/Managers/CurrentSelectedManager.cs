using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSelectedManager : MonoBehaviour
{
    public CardVariable originalCard;
    public CardVariable currentCard;
    public CardViz currentCardViz;
    public SO.BoolVariable cardIsSelected;
    public int cardOffset;
    public Canvas currentCanvas;

    Transform mTransform;

    public void LoadCard()
    {
        if (currentCard.value == null) 
            return;

        //currentCard.value.gameObject.SetActive(false);
        currentCardViz.LoadCardViz(currentCard.value.viz.card);
        currentCardViz.gameObject.SetActive(true);
    }

    public void LoadHighlight()
    {
        if (currentCard.value == null)
            return;

        Vector3 cardPosition = new Vector3(currentCard.value.viz.transform.position.x, 
                                            (currentCard.value.viz.transform.position.y + (cardOffset * currentCanvas.scaleFactor)), 
                                             currentCard.value.viz.transform.position.z);

        mTransform.position = cardPosition;
        currentCardViz.LoadCardViz(currentCard.value.viz.card);
        currentCardViz.gameObject.SetActive(true);
    }

    public void CloseCardSelection() 
    {
        currentCardViz.gameObject.SetActive(false);
    }

    private void Start()
    {
        cardIsSelected.value = false;
        mTransform = this.transform;
        CloseCardSelection();
    }

    void Update()
    {
        if (cardIsSelected.value)
        {
            mTransform.position = Input.mousePosition;
        }
        else
        {
            mTransform.position = new Vector3(Input.mousePosition.x, mTransform.position.y, mTransform.position.z);
        }
    }
}
