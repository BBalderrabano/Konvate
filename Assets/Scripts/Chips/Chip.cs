using UnityEngine;
using UnityEngine.UI;

public class Chip : MonoBehaviour
{
    public ChipType type;
    public PlayerHolder owner;
    public Image backSide;
}

public enum ChipType
{
    COMBAT,
    POISON,
    BLEED,
    OFFENSIVE,
    COMBAT_OFFENSIVE
}
