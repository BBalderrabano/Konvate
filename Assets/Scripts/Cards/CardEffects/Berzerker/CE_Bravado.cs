using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Berzerker/Bravado")]
public class CE_Bravado : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        List<Chip> myChips_component = card.owner.currentHolder.playedCombatChipHolder.value.GetComponentsInChildren<Chip>().ToList();
        List<Chip> oppChips_component = opponent.currentHolder.playedCombatChipHolder.value.GetComponentsInChildren<Chip>().ToList();

        int myChips = myChips_component.FindAll(a => a.type == ChipType.COMBAT || a.type == ChipType.OFFENSIVE && a.state == ChipSate.PLAYED).Count;
        int oppChips = oppChips_component.FindAll(a => a.type == ChipType.COMBAT || a.type == ChipType.OFFENSIVE && a.state == ChipSate.PLAYED).Count;

        if(myChips > oppChips)
        {
            card.owner.statMods.Add(new SMOD_DamageMod());
            card.MakeBorderActive();
        }
        else
        {
            card.BreakeCard();
        }
    }
}
