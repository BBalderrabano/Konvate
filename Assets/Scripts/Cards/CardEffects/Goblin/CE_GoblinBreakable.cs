using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Goblin/Goblin Destruible")]
public class CE_GoblinBreakable : SelectionCardEffect
{
    public int amount_required = 1;
    public CardEffect card_effect;
    bool breakeIt = false;

    public override void Execute()
    {
        base.Execute();

        breakeIt = false;

        PlayerHolder enemy = GM.GetOpponentHolder(card.owner.photonId);
        int reserve_combat_chips = enemy.currentHolder.combatChipHolder.value.childCount;

        if (reserve_combat_chips >= amount_required)
        {
            if (card.owner.isLocal)
            {
                GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId, enemy.photonId));
            }
            else
            {
                parentAction.PushAction(new A_CardSelection("¿Colocar " + amount_required + " <sprite=0> del lado oponente?\nTienes: " + reserve_combat_chips, card, enemy.photonId, this, base.card.instanceId).ModifyParameters(true, false, 0, 0));
            }
        }
        else
        {
            ExecuteGoblinEffect();
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds == null)
        {
            ExecuteGoblinEffect();
            breakeIt = false;
        }
        else
        {
            GameObject chip = GM.GetOpponentHolder(card.owner.photonId).currentHolder.combatChipHolder.value.GetChild(0).gameObject;

            chip.GetComponent<Chip>().state = ChipSate.PLAYED;

            parentAction.LinkAnimation(GM.animationManager.MoveChip(chip, 
                                                                    parentAction.actionId, 
                                                                    card.owner.photonId, 
                                                                    card.owner.currentHolder.playedCombatChipHolder.value.position, 
                                                                    card.owner.currentHolder.playedCombatChipHolder.value.gameObject));

            breakeIt = true;
        }
    }

    void ExecuteGoblinEffect()
    {
        CardEffect clone = card_effect.Clone(card);

        clone.isDone = true;

        card.cardEffects.Add(clone);

        parentAction.PushAction(new A_ExecuteEffect(card.instanceId, clone.effectId, card.owner.photonId));
    }

    public override void Finish()
    {
        if (breakeIt)
        {
            card.BreakeCard();
        }
        else
        {
            card.MakeBorderActive();
        }

        base.Finish();
    }
}
