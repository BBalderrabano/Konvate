using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Samurai/Espiritu Inquebrantable")]
public class CE_EspirituInquebrantable : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        AudioManager.singleton.Play(SoundEffectType.TRYNDAMERE);
        card.owner.statMods.Add(new SMOD_Tryndamere());
    }
}
