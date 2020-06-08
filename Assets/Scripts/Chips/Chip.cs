using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    public ChipType type;
    public PlayerHolder owner;
}

public enum ChipType
{
    COMBAT,
    POISON,
    BLEED,
    OFFENSIVE,
    COMBAT_OFFENSIVE
}
