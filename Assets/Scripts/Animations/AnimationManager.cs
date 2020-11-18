﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{
    GameManager GM
    {
        get { return GameManager.singleton; }
    }

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
            PlayerHolder enemy = GM.GetOpponentHolder(player.photonId);

            for (int i = 0; i < playedChips.childCount; i++)
            {
                float delay = Settings.CHIP_ANIMATION_DELAY + (Settings.ANIMATION_INTERVAL * i);

                GameObject chip = playedChips.GetChild(i).gameObject;
                Chip chip_component = chip.GetComponent<Chip>();

                GameObject parent;

                if (chip_component.type == ChipType.POISON)
                {
                    parent = chip_component.owner.currentHolder.poisonChipHolder.value.gameObject;
                }
                else if (chip_component.type == ChipType.BLEED)
                {
                    parent = chip_component.owner.currentHolder.bleedChipHolder.value.gameObject;
                }
                else
                {
                    parent = chip_component.owner.currentHolder.combatChipHolder.value.gameObject;
                }
                
                iTween.MoveTo(chip,
                    iTween.Hash(
                        "position", enemy.currentHolder.bleedChipHolder.value.position,
                        "time", Settings.CHIP_ANIMATION_TIME,
                        "onstart", "PlaySound",
                        "onstarttarget", this.gameObject,
                        "onstartparams", iTween.Hash("play_sound", SoundEffectType.MOVE_CHIP),
                        "oncomplete", "BleedChipDamage",
                        "easetype", Settings.ANIMATION_STYLE,
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

    public Animation DirectDamageBleedChip(int actionId, int photonId, int cardId, int target_photonId, int amount = 1)
    {
        int temp_amount = amount;

        PlayerHolder player = GM.GetPlayerHolder(photonId);
        PlayerHolder target = GM.GetPlayerHolder(target_photonId);

        List<Transform> chips = GM.GetChips(ChipType.BLEED, player);

        Card card = player.GetCard(cardId);

        if (chips.Count == 0)
        {
            return null;
        }

        if (temp_amount > chips.Count)
        {
            temp_amount = chips.Count;
        }

        Animation animationPointer = new Animation(amount);

        for (int i = 0; i < temp_amount; i++)
        {
            GameObject chip = chips[i].gameObject;
            Chip chip_component = chip.GetComponent<Chip>();

            if (card != null)
            {
                chip.transform.position = WorldToCanvasPosition(card.cardPhysicalInst.transform.position);
            }

            float delay = Settings.ANIMATION_DELAY + (Settings.ANIMATION_INTERVAL * i);

            iTween.MoveTo(chip,
                    iTween.Hash(
                        "position", target.currentHolder.bleedChipHolder.value.position,
                        "time", Settings.CHIP_ANIMATION_TIME,
                        "onstart", "PlaySound",
                        "onstarttarget", this.gameObject,
                        "onstartparams", iTween.Hash("play_sound", SoundEffectType.MOVE_CHIP),
                        "oncomplete", "BleedChipDamage",
                        "easetype", Settings.ANIMATION_STYLE,
                        "delay", delay,
                        "oncompleteparams", iTween.Hash("action", actionId,
                                                        "new_parent", chip_component.owner.currentHolder.bleedChipHolder.value.gameObject,
                                                        "object", chip,
                                                        "photonId", target_photonId,
                                                        "animationId", animationPointer.animId,
                                                        "reset_position", true),
                        "onCompleteTarget", this.gameObject
            ));
        }

        return animationPointer;
    }

    public void BleedChipDamage(object animParams)
    {
        Hashtable hstbl = (Hashtable)animParams;

        int photonId = ((int)hstbl["photonId"]);
        GameObject chip = (GameObject)hstbl["object"];

        PlayerHolder player = GM.GetPlayerHolder(photonId);

        player.ModifyHitPoints(-1);

        AM_FinishAnimation(animParams);

        chip.GetComponent<Chip>().backSide.gameObject.SetActive(false);
    }

    #endregion

    #region Replace combat chips with bleed chips

    public Animation ReplaceChipsWithBleed(int actionId)
    {
        Vector3 rotateTo;
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
                float delay = Settings.ANIMATION_DELAY + (Settings.ANIMATION_INTERVAL * i);

                GameObject chip = playedChips.GetChild(i).gameObject;

                if(chip.GetComponent<Chip>().type != ChipType.BLEED)
                {
                    rotateTo = new Vector3(0, 90, 0);
                }
                else
                {
                    rotateTo = Vector3.zero;
                }

                iTween.RotateTo(chip,
                    iTween.Hash(
                        "rotation", rotateTo,
                        "time", (Settings.CHIP_ANIMATION_TIME * 0.5),
                        "oncomplete", "RotateBleedChips",
                        "easetype", Settings.ANIMATION_STYLE,
                        "delay", delay,
                        "oncompleteparams", iTween.Hash("object", chip,
                                                        "action", actionId,
                                                        "photonId", player.photonId,
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

        Vector3 rotateAdd;

        chip.GetComponent<Chip>().backSide.gameObject.SetActive(true);

        if (chip.GetComponent<Chip>().type != ChipType.BLEED)
        {
            rotateAdd = new Vector3(0, 90, 0);
        }
        else
        {
            rotateAdd = Vector3.zero;
        }

        iTween.RotateAdd(chip,
            iTween.Hash(
                    "amount", rotateAdd,
                    "time", (Settings.CHIP_ANIMATION_TIME * 0.5),
                    "delay", Settings.CHIP_ANIMATION_DELAY,
                    "onstart", "PlaySound",
                    "onstarttarget", this.gameObject,
                    "onstartparams", iTween.Hash("play_sound", SoundEffectType.PLACE_CHIP),
                    "oncomplete", "AM_FinishAnimation",
                    "oncompleteparams", iTween.Hash("action", action_id,
                                                    "new_parent", null,
                                                    "object", chip,
                                                    "photonId", -1,
                                                    "animationId", animationId,
                                                    "reset_position", true),
                    "onCompleteTarget", this.gameObject
        ));
    }
    #endregion

    #region Chip Animations
    public Animation RemoveChip(int actionId, CardEffect effect, int photonId, int cardId, ChipType type, int amount = 1, bool floatsDefend = true)
    {
        int temp_amount = amount;

        PlayerHolder e = GM.GetOpponentHolder(photonId);

        List<Transform> chips = GM.GetChips(type, e, true);

        Card card = GM.GetCard(cardId);

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
                float delay = Settings.ANIMATION_DELAY + (Settings.ANIMATION_INTERVAL * i);

                GameObject chip = chips[i].gameObject;
                Chip chip_component = chip.GetComponent<Chip>();

                GameObject parentTo;

                if (type == ChipType.BLEED)
                {
                    parentTo = chip_component.owner.currentHolder.bleedChipHolder.value.gameObject;
                }
                else if (type == ChipType.POISON)
                {
                    parentTo = chip_component.owner.currentHolder.poisonChipHolder.value.gameObject;
                }
                else 
                {
                    parentTo = chip_component.owner.currentHolder.combatChipHolder.value.gameObject;
                }

                iTween.MoveTo(chip,
                    iTween.Hash(
                        "position", WorldToCanvasPosition(card.cardPhysicalInst.transform.position),
                        "time", Settings.CHIP_ANIMATION_TIME,
                        "onstart", "PlaySound",
                        "onstarttarget", this.gameObject,
                        "onstartparams", iTween.Hash("play_sound", SoundEffectType.MOVE_CHIP),
                        "oncomplete", "AM_FinishAnimation",
                        "easetype", Settings.ANIMATION_STYLE,
                        "delay", delay,
                        "oncompleteparams", iTween.Hash("action", actionId,
                                                        "new_parent", parentTo,
                                                        "object", chip,
                                                        "photonId", photonId,
                                                        "animationId", animationPointer.animId,
                                                        "reset_position", false,
                                                        "play_sound", SoundEffectType.PLACE_CHIP),
                        "onCompleteTarget", this.gameObject
                ));
            }
            else if(floatsDefend)
            {
                card.owner.AddFloatingDefend(new FloatingDefenseHolder(effect, type));
                animationPointer.OnComplete();
            }
        }

        return animationPointer;
    }

    public Animation PlaceChip(int actionId, int photonId, int cardId, ChipType type, int amount = 1)
    {
        int temp_amount = amount;

        PlayerHolder p = GM.GetPlayerHolder(photonId);
        PlayerHolder e = GM.GetOpponentHolder(photonId);

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
            Chip chip_component = chip.GetComponent<Chip>();

            if (card != null)
            {
                chip.transform.position = WorldToCanvasPosition(card.cardPhysicalInst.transform.position);
            }

            Vector3 travelTo;
            GameObject parentTo;

            FloatingDefenseHolder floatingDefense = e.GetFloatingDefend(type);

            if (floatingDefense != null)
            {
                travelTo = WorldToCanvasPosition(floatingDefense.effect.card.cardPhysicalInst.gameObject.transform.position);

                if (type == ChipType.POISON)
                {
                    parentTo = chip_component.owner.currentHolder.poisonChipHolder.value.gameObject;
                }
                else
                {
                    parentTo = chip_component.owner.currentHolder.combatChipHolder.value.gameObject;
                }

                e.RemoveFloatingDefend(floatingDefense);
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

            float delay = Settings.ANIMATION_DELAY + (Settings.ANIMATION_INTERVAL * i);

            iTween.MoveTo(chip.gameObject,
                iTween.Hash(
                    "position", travelTo,
                    "time", Settings.CHIP_ANIMATION_TIME,
                    "onstart", "PlaySound",
                    "onstarttarget", this.gameObject,
                    "onstartparams", iTween.Hash("play_sound", SoundEffectType.MOVE_CHIP),
                    "oncomplete", "AM_FinishAnimation",
                    "easetype", Settings.ANIMATION_STYLE,
                    "delay", delay,
                    "oncompleteparams", iTween.Hash("action", actionId,
                                                    "new_parent", parentTo,
                                                    "object", chip.gameObject,
                                                    "photonId", photonId,
                                                    "animationId", animationPointer.animId,
                                                    "reset_position", true,
                                                    "play_sound", SoundEffectType.PLACE_CHIP),
                    "onCompleteTarget", this.gameObject
            ));
        }

        return animationPointer;
    }

    public Animation MoveChip(GameObject chip, int actionId, int photonId, Vector3 position, GameObject new_parent)
    {
        Animation animationPointer = new Animation();

        AudioManager.singleton.Play(SoundEffectType.MOVE_CHIP);

        iTween.MoveTo(chip,
            iTween.Hash(
                "position", position,
                "time", Settings.CHIP_ANIMATION_TIME,
                "easetype", Settings.ANIMATION_STYLE,
                "delay", Settings.CHIP_ANIMATION_DELAY,
                "oncomplete", "AM_FinishAnimation",
                "oncompleteparams", iTween.Hash("action", actionId,
                                                "new_parent", new_parent,
                                                "object", chip,
                                                "photonId", photonId,
                                                "animationId", animationPointer.animId,
                                                "reset_position", true,
                                                "play_sound", SoundEffectType.PLACE_CHIP),
                "onCompleteTarget", this.gameObject
        ));

        return animationPointer;
    }

    #endregion

    #region Card Animations
    public Animation MoveCard(int actionId, int photonId, int cardId, Vector3 position, GameObject new_parent, SoundEffectType playSound = SoundEffectType.NONE)
    {
        Animation animationPointer = new Animation();

        PlayerHolder p = GM.GetPlayerHolder(photonId);
        Card c = p.GetCard(cardId);

        iTween.MoveTo(c.cardPhysicalInst.gameObject, 
            iTween.Hash(
                "position", position,
                "time", Settings.ANIMATION_TIME,
                "oncomplete", "AM_FinishAnimation",
                "easetype", Settings.ANIMATION_STYLE,
                "delay", Settings.ANIMATION_DELAY,
                "oncompleteparams", iTween.Hash("action", actionId,
                                                "new_parent", new_parent,
                                                "object", c.cardPhysicalInst.gameObject,
                                                "photonId", photonId,
                                                "animationId", animationPointer.animId,
                                                "reset_position", true,
                                                "play_sound", playSound),
                "onCompleteTarget", this.gameObject
        ));

        iTween.RotateTo(c.cardPhysicalInst.gameObject,
            iTween.Hash(
                "rotation", new_parent.transform.rotation.eulerAngles,
                "time", Settings.ANIMATION_TIME));

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

        if (hstbl.ContainsKey("play_sound"))
        {
            SoundEffectType soundEffect = (SoundEffectType)hstbl["play_sound"];

            if(soundEffect != SoundEffectType.NONE)
            {
                AudioManager.singleton.Play(soundEffect);
            }
        }

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

    public void PlaySound(object animParams)
    {
        Hashtable hstbl = (Hashtable)animParams;

        if (hstbl.ContainsKey("play_sound"))
        {
            SoundEffectType soundEffect = (SoundEffectType)hstbl["play_sound"];

            if (soundEffect != SoundEffectType.NONE)
            {
                AudioManager.singleton.Play(soundEffect);
            }
        }
    }

    Vector3 WorldToCanvasPosition(Vector3 worldPosition)
    {
        return Camera.main.WorldToScreenPoint(worldPosition);
    }
}
