using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CardLogic : ScriptableObject
{
    public abstract void OnClick(CardInstance inst);

    public abstract void OnHighlight(CardInstance inst);

    public abstract void OnSetLogic(CardInstance inst);
}
