using UnityEngine;
using System.Collections;

public class CardTag {
    public static CardTags[] ATAQUE_BASICO =
    {
        CardTags.ATTACK,
        CardTags.BASIC_CARD
    };

    public static CardTags[] DEFENSA =
    {
        CardTags.DEFENSE,
        CardTags.BASIC_CARD
    };

    public static CardTags[] DEFENSA_SUPERIOR =
    {
        CardTags.DEFENSE,
        CardTags.SUPERIOR_DEFENSE
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

    BREAKES_CARDS,
    MANTAIN,
    COMBO,

    MAKES_FREE_AND_LIGHTNING,

    PES_EXPLO_VENENO_TARGET,

    REMOVES_CARD_FROM_PLAY,

    ADDS_START_TURN_CARD_DRAW,
    ADDS_ENERGY,
    PLAYS_FROM_HAND,

    RESTORES_HP,
    SUPERIOR_DEFENSE,

    REMOVES_ENERGY,
    SENDS_CARD_FROM_DISCARD_TO_DECK,
    SENDS_CARD_FROM_HAND_TO_DECK,
    SENDS_CARD_FROM_DECK_TO_DISCARD,

    TARGETS_PLAYER,

    PREVAILS,
    DISCARDS_CARDS,
    CURSE,
    CURSE_BRUJO,
    SENDS_DECKCARD_TO_BOTTOM_DECK,
    CURSE_BRUJO_COST_TARGET,
    SHAPE,
    TUTORS,

    SHAPE_ENT,
    SHAPE_WOLF,
    SHAPE_BEAR
}
