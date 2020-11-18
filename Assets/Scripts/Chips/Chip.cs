using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Chip : MonoBehaviour
{
    public ChipType type;
    public PlayerHolder owner;
    public UnityEngine.UI.Image backSide;
}

public enum ChipType
{
    COMBAT,
    POISON,
    BLEED,
    OFFENSIVE,
    COMBAT_OFFENSIVE
}
