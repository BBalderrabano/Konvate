﻿using UnityEngine;

public class A_CheckFloatingDefense : KAction
{
    public A_CheckFloatingDefense(int photonId, int actionId = -1) : base(photonId, actionId) {}

    public override bool Continue()
    {
        return false;
    }

    public override void Execute(float t)
    {
        if (!isInit)
        {
            foreach (PlayerHolder p in GM.allPlayers)
            {
                PlayerHolder enemy = GM.GetOpponentHolder(p.photonId);

                for (int i = 0; i < p.currentHolder.playedPoisonChipHolder.value.childCount; i++)
                {
                    GameObject chip = p.currentHolder.playedPoisonChipHolder.value.GetChild(i).gameObject;
                    Chip chip_component = chip.GetComponent<Chip>();

                    FloatingDefenseHolder floatingDefense = enemy.GetFloatingDefend(ChipType.POISON);

                    if (floatingDefense != null)
                    {
                        if (chip_component.type == ChipType.POISON)
                        {
                            GameObject parentTo = p.currentHolder.poisonChipHolder.value.gameObject;
                            Vector3 targetPosition = Settings.WorldToCanvasPosition(floatingDefense.effect.card.cardPhysicalInst.gameObject.transform.position);

                            LinkAnimation(GM.animationManager.MoveChip(chip, actionId, -1, targetPosition, parentTo));
                            chip_component.state = ChipSate.STASHED;
                        }
                        enemy.RemoveFloatingDefend(floatingDefense);
                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = 0; i < p.currentHolder.playedCombatChipHolder.value.childCount; i++)
                {
                    GameObject chip = p.currentHolder.playedCombatChipHolder.value.GetChild(i).gameObject;
                    Chip chip_component = chip.GetComponent<Chip>();

                    FloatingDefenseHolder floatCombatDefense = enemy.GetFloatingDefend(ChipType.COMBAT_OFFENSIVE);
                    FloatingDefenseHolder floatPoisonDefense = enemy.GetFloatingDefend(ChipType.POISON);

                    if (floatCombatDefense == null && floatPoisonDefense == null)
                        break;

                    if (floatCombatDefense != null)
                    {
                        if (chip_component.type == ChipType.COMBAT || chip_component.type == ChipType.OFFENSIVE)
                        {
                            GameObject parentTo = p.currentHolder.combatChipHolder.value.gameObject;
                            Vector3 targetPosition = Settings.WorldToCanvasPosition(floatCombatDefense.effect.card.cardPhysicalInst.gameObject.transform.position);

                            LinkAnimation(GM.animationManager.MoveChip(chip, actionId, -1, targetPosition, parentTo));
                            chip_component.state = ChipSate.STASHED;
                        }
                        enemy.RemoveFloatingDefend(floatCombatDefense);
                    }

                    if(floatPoisonDefense != null)
                    {
                        if(chip.GetComponent<Chip>().type == ChipType.POISON)
                        {
                            GameObject parentTo = p.currentHolder.poisonChipHolder.value.gameObject;
                            Vector3 targetPosition = Settings.WorldToCanvasPosition(floatPoisonDefense.effect.card.cardPhysicalInst.gameObject.transform.position);

                            LinkAnimation(GM.animationManager.MoveChip(chip, actionId, -1, targetPosition, parentTo));
                            chip_component.state = ChipSate.STASHED;
                        }
                        enemy.RemoveFloatingDefend(floatPoisonDefense);
                    }
                }
            }

            isInit = true;
        }
    }

    public override bool IsComplete()
    {
        return isInit && AnimationsAreReady();
    }
}
