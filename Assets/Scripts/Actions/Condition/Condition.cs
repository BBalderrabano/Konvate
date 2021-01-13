using System;
using UnityEngine;

public abstract class Condition : ScriptableObject, ICloneable
{
    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public abstract bool isTemporary();

    public abstract bool isValid(int photonId, int cardId = -1);
}
