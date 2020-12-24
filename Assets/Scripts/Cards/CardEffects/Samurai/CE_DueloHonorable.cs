using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Samurai/Duelo Honorable")]
public class CE_DueloHonorable : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        PlayerHolder opponent = GM.GetOpponentHolder(card.owner.photonId);

        List<Chip> myChips_component = card.owner.currentHolder.playedCombatChipHolder.value.GetComponentsInChildren<Chip>().ToList();
        List<Chip> oppChips_component = opponent.currentHolder.playedCombatChipHolder.value.GetComponentsInChildren<Chip>().ToList();

        int myChips = myChips_component.FindAll(a => a.type == ChipType.COMBAT || a.type == ChipType.OFFENSIVE && a.state == ChipSate.PLAYED).Count;
        int oppChips = oppChips_component.FindAll(a => a.type == ChipType.COMBAT || a.type == ChipType.OFFENSIVE && a.state == ChipSate.PLAYED).Count;

        if(myChips > oppChips || myChips == oppChips)
        {
            opponent.statMods.Add(new SMOD_FistToBleed());
        }
        
        if(myChips < oppChips || myChips == oppChips)
        {
            card.owner.statMods.Add(new SMOD_FistToBleed());
        }
    }
}
