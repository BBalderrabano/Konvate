using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInstance : MonoBehaviour, IClickable
{
    public CardViz viz;
    private CardLogic currentLogic;

    public int instanceId
    {
        get { return viz.card.instanceId; }
    }

    public CardLogic GetCurrentLogic() { return currentLogic; }

    public void setCurrentLogic(CardLogic newLogic)
    {
        currentLogic = newLogic;
        currentLogic.OnSetLogic(this);
    }

    void Start()
    {
        viz = GetComponent<CardViz>();
    }

    public void OnClick()
    {
        if (currentLogic == null)
            return;

        currentLogic.OnClick(this);
    }

    public void OnHighlight()
    {
        if (currentLogic == null)
            return;

        currentLogic.OnHighlight(this);
    }

}
