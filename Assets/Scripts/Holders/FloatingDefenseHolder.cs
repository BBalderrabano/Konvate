using UnityEngine;
using System.Collections;

public class FloatingDefenseHolder
{
    public CardEffect effect;
    public ChipType type;

    public FloatingDefenseHolder(CardEffect effect, ChipType type)
    {
        this.effect = effect;
        this.type = type;
    }

}
