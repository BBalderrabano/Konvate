using UnityEngine;
using System.Collections;

public class CardTag {
    public static CardTags[] ATAQUE_BASICO =
    {
        CardTags.ATTACK,
        CardTags.BASIC_CARD
    };
}

public enum CardTags
{
    BASIC_CARD,
    CLASS_CARD,

    ATTACK,
    DEFENSE,
    SPECIAL,

    PLACES_COMBAT_CHIP,
    PLACES_POISON_CHIP,
    PLACES_BLEED_CHIP,

    REMOVES_COMBAT_CHIP,
    REMOVES_POISON_CHIP,
    PREVENTS_BLEED_CHIP,
    REMOVES_BLEED_CHIP,
    
    DRAWS_CARD,

    PREVENTS_USE_OF_CARDS,
    GIVES_MANTAIN,
    COMBO,

    MAKES_FREE_AND_LIGHTNING,

    PES_EXPLO_VENENO_TARGET,

    REMOVES_CARD_FROM_PLAY,

    ADDS_START_TURN_CARD_DRAW,
    ADDS_ENERGY,
    PLAYS_FROM_HAND
}
