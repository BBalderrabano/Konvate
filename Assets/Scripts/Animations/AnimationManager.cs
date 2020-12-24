using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

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

                if(chip_component.state == ChipSate.STASHED)
                {
                    animationPointer.OnComplete();
                    continue;
                }

                Vector3 travelTo = enemy.currentHolder.bleedChipHolder.value.position;
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

                FloatingDefenseHolder floatingDefense = enemy.GetFloatingDefend(ChipType.BLEED);

                if (floatingDefense != null)
                {
                    travelTo = Settings.WorldToCanvasPosition(floatingDefense.effect.card.cardPhysicalInst.gameObject.transform.position);

                    animationPointer = MoveChip(chip, actionId, player.photonId, travelTo, parent);

                    chip_component.state = ChipSate.STASHED;
                    enemy.RemoveFloatingDefend(floatingDefense);
                }
                else
                {
                    Hashtable hash = new Hashtable
                    {
                        { "action", actionId },
                        { "new_parent", parent},
                        { "object", chip},
                        { "photonId", enemy.photonId},
                        { "animationId", animationPointer.animId},
                        { "reset_position", true}
                    };

                    LeanTween.move(chip, travelTo, Settings.CHIP_ANIMATION_TIME)
                        .setEaseInOutQuad()
                        .setDelay(delay)
                        .setOnStart(() => { AudioManager.singleton.Play(SoundEffectType.MOVE_CHIP); })
                        .setOnComplete(BleedChipDamage)
                        .setOnCompleteParam(hash as object);
                }
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
                chip.transform.position = Settings.WorldToCanvasPosition(card.cardPhysicalInst.transform.position);
            }

            float delay = Settings.ANIMATION_DELAY + (Settings.ANIMATION_INTERVAL * i);
            chip_component.state = ChipSate.STASHED;

            Hashtable hash = new Hashtable
                    {
                        { "action", actionId },
                        { "new_parent", chip_component.owner.currentHolder.bleedChipHolder.value.gameObject},
                        { "object", chip},
                        { "photonId", target_photonId},
                        { "animationId", animationPointer.animId},
                        { "reset_position", true}
                    };

            LeanTween.move(chip, target.currentHolder.bleedChipHolder.value.position, Settings.CHIP_ANIMATION_TIME)
                .setEaseInOutQuad()
                .setDelay(delay)
                .setOnStart(() => { AudioManager.singleton.Play(SoundEffectType.MOVE_CHIP); })
                .setOnComplete(BleedChipDamage)
                .setOnCompleteParam(hash as object);
        }

        return animationPointer;
    }

    public void BleedChipDamage(object animParams)
    {
        AM_FinishAnimation(animParams);

        Hashtable hstbl = (Hashtable)animParams;

        int photonId = ((int)hstbl["photonId"]);
        GameObject chip = (GameObject)hstbl["object"];
        Chip chip_component = chip.GetComponent<Chip>();

        PlayerHolder player = GM.GetPlayerHolder(photonId);

        player.ModifyHitPoints(-1);

        chip_component.backSide.gameObject.SetActive(false);
        chip_component.state = ChipSate.STASHED;

        if (chip_component.owner.isLocal)
        {
            GM.turn.localInflictedBleed = true;
        }
        else
        {
            GM.turn.opponentInflictedBleed = true;
        }
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

            int chips_to_animate = 0;

            for (int i = 0; i < playedChips.childCount; i++)
            {
                GameObject chip = playedChips.GetChild(i).gameObject;
                Chip chip_component = chip.GetComponent<Chip>();

                if (chip_component.type == ChipType.BLEED || chip_component.state == ChipSate.STASHED)
                {
                    animationPointer.OnComplete();
                    continue;
                }

                float delay = Settings.ANIMATION_DELAY + (Settings.ANIMATION_INTERVAL * chips_to_animate);

                Hashtable hash = new Hashtable
                    {
                        { "action", actionId },
                        { "object", chip},
                        { "photonId", player.photonId},
                        { "animationId", animationPointer.animId},
                    };

                LeanTween.rotate(chip, rotateTo, Settings.CHIP_ANIMATION_TIME * 0.5f)
                    .setEaseInOutQuad()
                    .setDelay(delay)
                    .setOnComplete(RotateBleedChips)
                    .setOnCompleteParam(hash);

                chips_to_animate++;
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

        Vector3 rotateAdd = new Vector3(0, 180, 0);

        chip.GetComponent<Chip>().backSide.gameObject.SetActive(true);

        Hashtable hash = new Hashtable
                    {
                        { "action", action_id },
                        { "new_parent", null},
                        { "object", chip},
                        { "photonId", -1},
                        { "animationId", animationId},
                        { "reset_position", true}
                    };

        LeanTween.rotate(chip, rotateAdd, Settings.CHIP_ANIMATION_TIME * 0.5f)
            .setEaseInOutQuad()
            .setDelay(Settings.CHIP_ANIMATION_DELAY)
            .setOnStart(() => { AudioManager.singleton.Play(SoundEffectType.PLACE_CHIP); })
            .setOnComplete(AM_FinishAnimation)
            .setOnCompleteParam(hash as object);
    }
    #endregion

    #region Chip Animations
    public Animation RemoveChip(int actionId, CardEffect effect, int photonId, int cardId, ChipType type, int amount = 1, bool floatsDefend = true)
    {
        int temp_amount = amount;

        PlayerHolder e = GM.GetOpponentHolder(photonId);

        List<Transform> chips = GM.GetChips(type, e, true);

        Card card = GM.GetCard(cardId);

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

                chip_component.state = ChipSate.STASHED;

                Hashtable hash = new Hashtable
                    {
                        { "action", actionId },
                        { "new_parent", parentTo},
                        { "object", chip},
                        { "photonId", photonId},
                        { "animationId", animationPointer.animId},
                        { "reset_position", true},
                        { "play_sound", SoundEffectType.PLACE_CHIP }
                    };

                LeanTween.move(chip, Settings.WorldToCanvasPosition(card.cardPhysicalInst.transform.position), Settings.CHIP_ANIMATION_TIME)
                    .setEaseInOutQuad()
                    .setDelay(delay)
                    .setOnStart(() => { AudioManager.singleton.Play(SoundEffectType.MOVE_CHIP); })
                    .setOnComplete(AM_FinishAnimation)
                    .setOnCompleteParam(hash as object);

                /*iTween.MoveTo(chip,
                    iTween.Hash(
                        "position", Settings.WorldToCanvasPosition(card.cardPhysicalInst.transform.position),
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
                                                        "reset_position", true,
                                                        "play_sound", SoundEffectType.PLACE_CHIP),
                        "onCompleteTarget", this.gameObject
                ));*/
            }
            else if (floatsDefend)
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
                chip.transform.position = Settings.WorldToCanvasPosition(card.cardPhysicalInst.transform.position);
            }

            Vector3 travelTo;
            GameObject parentTo;

            FloatingDefenseHolder floatingDefense = e.GetFloatingDefend(type);

            if (floatingDefense != null)
            {
                travelTo = Settings.WorldToCanvasPosition(floatingDefense.effect.card.cardPhysicalInst.gameObject.transform.position);

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

                e.RemoveFloatingDefend(floatingDefense);

                chip_component.state = ChipSate.STASHED;
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
                    Transform combatChipHolder = p.currentHolder.playedCombatChipHolder.value;

                    float x_right_most_pos = combatChipHolder.position.x;

                    //Move the chips to the position of the right most currently placed chip
                    if(combatChipHolder.childCount > 0)
                    {
                        x_right_most_pos = combatChipHolder.GetChild(combatChipHolder.childCount - 1).position.x;
                    }

                    travelTo = new Vector3(x_right_most_pos, p.currentHolder.playedCombatChipHolder.value.position.y);
                    parentTo = p.currentHolder.playedCombatChipHolder.value.gameObject;
                }

                chip_component.state = ChipSate.PLAYED;
            }

            float delay = Settings.ANIMATION_DELAY + (Settings.ANIMATION_INTERVAL * i);

            Hashtable hash = new Hashtable
                    {
                        { "action", actionId },
                        { "new_parent", parentTo},
                        { "object", chip.gameObject},
                        { "photonId", photonId},
                        { "animationId", animationPointer.animId},
                        { "reset_position", true},
                        { "play_sound", SoundEffectType.PLACE_CHIP }
                    };

            LeanTween.move(chip.gameObject, travelTo, Settings.CHIP_ANIMATION_TIME)
                .setEaseInOutQuad()
                .setDelay(delay)
                .setOnStart(() => { AudioManager.singleton.Play(SoundEffectType.MOVE_CHIP); })
                .setOnComplete(AM_FinishAnimation)
                .setOnCompleteParam(hash as object);
        }

        return animationPointer;
    }

    public Animation MoveChip(GameObject chip, int actionId, int photonId, Vector3 position, GameObject new_parent)
    {
        Animation animationPointer = new Animation();

        Hashtable hash = new Hashtable
                    {
                        { "action", actionId },
                        { "new_parent", new_parent},
                        { "object", chip},
                        { "photonId", photonId},
                        { "animationId", animationPointer.animId},
                        { "reset_position", true},
                        { "play_sound", SoundEffectType.PLACE_CHIP }
                    };

        LeanTween.move(chip.gameObject, position, Settings.CHIP_ANIMATION_TIME)
            .setEaseInOutQuad()
            .setDelay(Settings.CHIP_ANIMATION_DELAY)
            .setOnStart(() => { AudioManager.singleton.Play(SoundEffectType.MOVE_CHIP); })
            .setOnComplete(AM_FinishAnimation)
            .setOnCompleteParam(hash as object);

        return animationPointer;
    }

    #endregion

    #region Card Animations
    public Animation MoveCard(int actionId, int photonId, int cardId, Vector3 position, GameObject new_parent, SoundEffectType playSound = SoundEffectType.NONE)
    {
        Animation animationPointer = new Animation();

        PlayerHolder p = GM.GetPlayerHolder(photonId);
        Card c = p.GetCard(cardId);

        Hashtable hash = new Hashtable
        {
            { "action", actionId },
            { "new_parent", new_parent},
            { "object", c.cardPhysicalInst.gameObject},
            { "photonId", photonId},
            { "animationId", animationPointer.animId},
            { "reset_position", true},
            { "play_sound", playSound}
        };

        LeanTween.move(c.cardPhysicalInst.gameObject, position, Settings.ANIMATION_TIME)
            .setEaseInOutQuad()
            .setDelay(Settings.ANIMATION_DELAY)
            .setOnComplete(AM_FinishAnimation).setOnCompleteParam(hash as object);

        LeanTween.rotate(c.cardPhysicalInst.gameObject, new_parent.transform.root.eulerAngles, Settings.ANIMATION_TIME);

        return animationPointer;
    }
    #endregion
    
    void AM_FinishAnimation(object animParams)
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

        KAction action = GM.actionManager.GetAction(action_id);

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

    void PlaySound(object animParams)
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
}
