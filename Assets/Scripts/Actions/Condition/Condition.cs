using UnityEngine;

public abstract class Condition : ScriptableObject
{
    public abstract bool isTemporary();

    public abstract bool isValid(int photonId, int cardId = -1);
}
