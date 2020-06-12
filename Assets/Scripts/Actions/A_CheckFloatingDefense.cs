using UnityEngine;

public class A_CheckFloatingDefense : Action
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
                PlayerHolder enemy = GM.getOpponentHolder(p.photonId);

                for (int i = 0; i < p.currentHolder.playedPoisonChipHolder.value.childCount; i++)
                {
                    GameObject chip = p.currentHolder.playedPoisonChipHolder.value.GetChild(i).gameObject;
                    FloatingDefenseHolder floatingDefense = enemy.GetFloatingDefend(ChipType.POISON);

                    if (floatingDefense != null)
                    {
                        if (chip.GetComponent<Chip>().type == ChipType.POISON)
                        {
                            GameObject parentTo = p.currentHolder.poisonChipHolder.value.gameObject;
                            Vector3 targetPosition = floatingDefense.effect.card.cardPhysicalInst.gameObject.transform.position;

                            LinkAnimation(GM.animationManager.MoveChip(chip, actionId, -1, targetPosition, parentTo));
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
                    FloatingDefenseHolder floatCombatDefense = enemy.GetFloatingDefend(ChipType.COMBAT_OFFENSIVE);
                    FloatingDefenseHolder floatPoisonDefense = enemy.GetFloatingDefend(ChipType.POISON);

                    if (floatCombatDefense == null && floatPoisonDefense == null)
                        break;

                    if (floatCombatDefense != null)
                    {
                        if (chip.GetComponent<Chip>().type == ChipType.COMBAT || chip.GetComponent<Chip>().type == ChipType.OFFENSIVE)
                        {
                            GameObject parentTo = p.currentHolder.combatChipHolder.value.gameObject;
                            Vector3 targetPosition = floatCombatDefense.effect.card.cardPhysicalInst.gameObject.transform.position;

                            LinkAnimation(GM.animationManager.MoveChip(chip, actionId, -1, targetPosition, parentTo));
                        }
                        enemy.RemoveFloatingDefend(floatCombatDefense);
                    }

                    if(floatPoisonDefense != null)
                    {
                        if(chip.GetComponent<Chip>().type == ChipType.POISON)
                        {
                            GameObject parentTo = p.currentHolder.poisonChipHolder.value.gameObject;
                            Vector3 targetPosition = floatPoisonDefense.effect.card.cardPhysicalInst.gameObject.transform.position;

                            LinkAnimation(GM.animationManager.MoveChip(chip, actionId, -1, targetPosition, parentTo));
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
