﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{
    GameManager GM
    {
        get { return GameManager.singleton; }
    }

    public static float ANIMATION_TIME = 0.3f;
    public static string ANIMATION_STYLE = "easeInOutQuad";
    public static float ANIMATION_DELAY = 0f;
    public static float ANIMATION_INTERVAL = 0.2f;

    public static float CHIP_ANIMATION_TIME = 0.5f;
    public static float CHIP_ANIMATION_DELAY = 0.5f;


    #region Move bleed chips to damage players
    public Animation MoveBleedChipsToDamagePlayers(int actionId)
    {
        int animationAmount = 0;

        foreach (PlayerHolder player in GM.allPlayers)
        {
            Transform playedChips = player.currentHolder.playedCombatChipHolder.value;

            animationAmount += playedChips.childCount;
        }

        Animation animationPointer = new Animation(animationAmount);

        foreach (PlayerHolder player in GM.allPlayers)
        {
            Transform playedChips = player.currentHolder.playedCombatChipHolder.value;
            PlayerHolder enemy = GM.getOpponentHolder(player.photonId);

            for (int i = 0; i < playedChips.childCount; i++)
            {
                float delay = CHIP_ANIMATION_DELAY + (ANIMATION_INTERVAL * i);

                GameObject chip = playedChips.GetChild(i).gameObject;
                GameObject parent = chip.GetComponent<Chip>().owner.currentHolder.bleedChipHolder.value.gameObject;

                iTween.MoveTo(chip,
                    iTween.Hash(
                        "position", enemy.currentHolder.bleedChipHolder.value.position,
                        "time", CHIP_ANIMATION_TIME,
                        "oncomplete", "BleedChipDamage",
                        "easetype", ANIMATION_STYLE,
                        "delay", delay,
                        "oncompleteparams", iTween.Hash("action", actionId,
                                                        "new_parent", parent,
                                                        "object", chip,
                                                        "photonId", enemy.photonId,
                                                        "animationId", animationPointer.animId,
                                                        "reset_position", true),
                        "onCompleteTarget", this.gameObject
                ));
            }
        }

        return animationPointer;
    }

    public void BleedChipDamage(object animParams)
    {
        Hashtable hstbl = (Hashtable)animParams;

        int photonId = ((int)hstbl["photonId"]);

        PlayerHolder player = GM.getPlayerHolder(photonId);

        player.ModifyBloodChip(-1);

        AM_FinishAnimation(animParams);
    }

    #endregion

    #region Replace combat chips with bleed chips

    public Animation ReplaceChipsWithBleed(int actionId)
    {
        Vector3 rotateTo = new Vector3(0, 90, 0);

        int animationAmount = 0;

        foreach (PlayerHolder player in GM.allPlayers)
        {
            Transform playedChips = player.currentHolder.playedCombatChipHolder.value;

            if (player.currentHolder.playedPoisonChipHolder.value.childCount > 0)
            {
                Settings.SetParent(player.currentHolder.playedPoisonChipHolder.value.GetChild(0), playedChips);
            }

            animationAmount += playedChips.childCount;
        }

        Animation animationPointer = new Animation(animationAmount);

        foreach (PlayerHolder player in GM.allPlayers)
        {
            Transform playedChips = player.currentHolder.playedCombatChipHolder.value;

            for (int i = 0; i < playedChips.childCount; i++)
            {
                float delay = ANIMATION_DELAY + (ANIMATION_INTERVAL * i);

                GameObject chip = playedChips.GetChild(i).gameObject;

                iTween.RotateTo(chip,
                    iTween.Hash(
                        "rotation", rotateTo,
                        "time", CHIP_ANIMATION_TIME,
                        "oncomplete", "RotateBleedChips",
                        "easetype", ANIMATION_STYLE,
                        "delay", delay,
                        "oncompleteparams", iTween.Hash("object", chip,
                                                        "action", actionId,
                                                        "animationId", animationPointer.animId),
                        "onCompleteTarget", this.gameObject
                ));
            }
        }

        return animationPointer;
    }

    public void RotateBleedChips(object animParams)
    {
        Hashtable hstbl = (Hashtable)animParams;
        GameObject chip = (GameObject)hstbl["object"];

        int action_id = (int)hstbl["action"];
        int animationId = (int)hstbl["animationId"];

        PlayerHolder chipOwner = chip.GetComponent<Chip>().owner;

        Vector3 rotateAdd = new Vector3(0, 90, 0);

        GameObject bleedChip = chipOwner.currentHolder.bleedChipHolder.value.GetChild(0).gameObject;

        Settings.SetParent(bleedChip.transform, chipOwner.currentHolder.playedCombatChipHolder.value);

        bleedChip.transform.Rotate(rotateAdd);

        iTween.RotateAdd(bleedChip,
            iTween.Hash(
                    "amount", rotateAdd,
                    "time", CHIP_ANIMATION_TIME,
                    "delay", CHIP_ANIMATION_DELAY,
                    "oncomplete", "AM_FinishAnimation",
                    "oncompleteparams", iTween.Hash("action", action_id,
                                                    "new_parent", null,
                                                    "object", bleedChip,
                                                    "photonId", -1,
                                                    "animationId", animationId,
                                                    "reset_position", true),
                    "onCompleteTarget", this.gameObject
        ));

        if (chip.GetComponent<Chip>().type == ChipType.POISON)
        {
            Settings.SetParent(chip.transform, chipOwner.currentHolder.poisonChipHolder.value.transform);
        }
        else if (chip.GetComponent<Chip>().type == ChipType.COMBAT || chip.GetComponent<Chip>().type == ChipType.OFFENSIVE)
        {
            Settings.SetParent(chip.transform, chipOwner.currentHolder.combatChipHolder.value.transform);
        }
    }
    #endregion

    #region Chip Animations
    public Animation RemoveChip(int actionId, CardEffect effect, int photonId, int cardId, ChipType type, int amount = 1, bool floatsDefend = true)
    {
        int temp_amount = amount;

        PlayerHolder p = GM.getPlayerHolder(photonId);
        PlayerHolder e = GM.getOpponentHolder(photonId);

        List<Transform> chips = GM.GetChips(type, e, true);

        Card card = p.GetCard(cardId);

        if (chips.Count == 0)
        {
            return null;
        }

        if (temp_amount > chips.Count)
        {
            temp_amount = chips.Count;
        }

        Animation animationPointer = new Animation(temp_amount);

        for (int i = 0; i < amount; i++)
        { 
            if(i < temp_amount)
            {
                float delay = ANIMATION_DELAY + (ANIMATION_INTERVAL * i);

                GameObject chip = chips[i].gameObject;
                GameObject parentTo;

                if (type == ChipType.BLEED)
                {
                    parentTo = p.currentHolder.bleedChipHolder.value.gameObject;
                }
                else if (type == ChipType.POISON)
                {
                    parentTo = p.currentHolder.poisonChipHolder.value.gameObject;
                }
                else 
                {
                    parentTo = p.currentHolder.combatChipHolder.value.gameObject;
                }

                iTween.MoveTo(chip,
                    iTween.Hash(
                        "position", WorldToCanvasPosition(card.cardPhysicalInst.transform.position),
                        "time", CHIP_ANIMATION_TIME,
                        "oncomplete", "AM_FinishAnimation",
                        "easetype", ANIMATION_STYLE,
                        "delay", delay,
                        "oncompleteparams", iTween.Hash("action", actionId,
                                                        "new_parent", parentTo,
                                                        "object", chip,
                                                        "photonId", photonId,
                                                        "animationId", animationPointer.animId,
                                                        "reset_position", false),
                        "onCompleteTarget", this.gameObject
                ));
            }
            else if(floatsDefend)
            {
                card.owner.addFloatingDefend(new FloatingDefenseHolder(effect, type));
                animationPointer.OnComplete();
            }
        }

        return animationPointer;
    }

    public Animation PlaceChip(int actionId, int photonId, int cardId, ChipType type, int amount = 1)
    {
        int temp_amount = amount;

        PlayerHolder p = GM.getPlayerHolder(photonId);
        PlayerHolder e = GM.getOpponentHolder(photonId);

        List<Transform> chips = GM.GetChips(type, p);

        Card card = p.GetCard(cardId);

        if (chips.Count == 0)
        {
            return null;
        }

        if (temp_amount > chips.Count)
        {
            temp_amount = chips.Count;
        }

        Animation animationPointer = new Animation(temp_amount);

        for (int i = 0; i < temp_amount; i++)
        {
            Transform chip = chips[i];

            if (card != null)
            {
                chip.transform.position = WorldToCanvasPosition(card.cardPhysicalInst.transform.position);
            }

            Vector3 travelTo;
            GameObject parentTo;

            FloatingDefenseHolder floatingDefense = e.getFloatingDefend(type);

            if (floatingDefense != null)
            {
                travelTo = WorldToCanvasPosition(floatingDefense.effect.card.cardPhysicalInst.gameObject.transform.position);

                if (type == ChipType.POISON)
                {
                    parentTo = p.currentHolder.poisonChipHolder.value.gameObject;
                }
                else
                {
                    parentTo = p.currentHolder.combatChipHolder.value.gameObject;
                }

                e.removeFloatingDefend(floatingDefense);
            }
            else
            {
                if (type == ChipType.POISON)
                {
                    travelTo = p.currentHolder.playedPoisonChipHolder.value.position;
                    parentTo = p.currentHolder.playedPoisonChipHolder.value.gameObject;
                }
                else
                {
                    travelTo = p.currentHolder.playedCombatChipHolder.value.position;
                    parentTo = p.currentHolder.playedCombatChipHolder.value.gameObject;
                }
            }

            float delay = ANIMATION_DELAY + (ANIMATION_INTERVAL * i);

            iTween.MoveTo(chip.gameObject,
                iTween.Hash(
                    "position", travelTo,
                    "time", CHIP_ANIMATION_TIME,
                    "oncomplete", "AM_FinishAnimation",
                    "easetype", ANIMATION_STYLE,
                    "delay", delay,
                    "oncompleteparams", iTween.Hash("action", actionId,
                                                    "new_parent", parentTo,
                                                    "object", chip.gameObject,
                                                    "photonId", photonId,
                                                    "animationId", animationPointer.animId,
                                                    "reset_position", true),
                    "onCompleteTarget", this.gameObject
            ));
        }

        return animationPointer;
    }

    public Animation MoveChip(GameObject chip, int actionId, int photonId, Vector3 position, GameObject new_parent)
    {
        Animation animationPointer = new Animation();

        iTween.MoveTo(chip,
            iTween.Hash(
                "position", position,
                "time", CHIP_ANIMATION_TIME,
                "oncomplete", "AM_FinishAnimation",
                "easetype", ANIMATION_STYLE,
                "delay", CHIP_ANIMATION_DELAY,
                "oncompleteparams", iTween.Hash("action", actionId,
                                                "new_parent", new_parent,
                                                "object", chip,
                                                "photonId", photonId,
                                                "animationId", animationPointer.animId,
                                                "reset_position", true),
                "onCompleteTarget", this.gameObject
        ));

        return animationPointer;
    }

    #endregion

    #region Card Animations
    public Animation MoveCard(int actionId, int photonId, int cardId, Vector3 position, GameObject new_parent)
    {
        Animation animationPointer = new Animation();

        PlayerHolder p = GM.getPlayerHolder(photonId);
        Card c = p.GetCard(cardId);

        iTween.MoveTo(c.cardPhysicalInst.gameObject, 
            iTween.Hash(
                "position", position,
                "time", ANIMATION_TIME,
                "oncomplete", "AM_FinishAnimation",
                "easetype", ANIMATION_STYLE,
                "delay", ANIMATION_DELAY,
                "oncompleteparams", iTween.Hash("action", actionId,
                                                "new_parent", new_parent,
                                                "object", c.cardPhysicalInst.gameObject,
                                                "photonId", photonId,
                                                "animationId", animationPointer.animId,
                                                "reset_position", true),
                "onCompleteTarget", this.gameObject
        ));

        iTween.RotateTo(c.cardPhysicalInst.gameObject,
            iTween.Hash(
                "rotation", new_parent.transform.rotation.eulerAngles,
                "time", ANIMATION_TIME));

        return animationPointer;
    }
    #endregion
    
    public void AM_FinishAnimation(object animParams)
    {
        Hashtable hstbl = (Hashtable)animParams;

        GameObject target = (GameObject)hstbl["object"];
        GameObject new_parent = (GameObject)hstbl["new_parent"];
        bool reset_position = (bool)hstbl["reset_position"];

        int action_id = (int)hstbl["action"];
        int photonId = (int)hstbl["photonId"];
        int animationId = (int)hstbl["animationId"];

        Action action = GM.actionManager.GetAction(action_id);

        if (new_parent != null)
        {
            if (reset_position)
            {
                Settings.SetParent(target.transform, new_parent.transform);
            }
            else
            {
                target.transform.SetParent(new_parent.transform);
            }
        }

        if(action != null && action.GetAnimation(animationId) != null)
        {
            action.CompleteAnimation(animationId, photonId);
        }
    }

    Vector3 WorldToCanvasPosition(Vector3 worldPosition)
    {
        return Camera.main.WorldToScreenPoint(worldPosition);
    }
}
