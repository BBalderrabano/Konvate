using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Duelista/Choque de Espadas")]
public class CE_ChoqueDeEspadas : CardEffect
{
    public CardEffect remove;
    public CardTextMod abTextMod;

    public override void Execute()
    {
        base.Execute();

        List<Card> ataques_basicos = card.owner.all_cards.FindAll(a => a.HasTags(CardTag.ATAQUE_BASICO));

        for (int i = 0; i < ataques_basicos.Count; i++)
        {
            if(!ataques_basicos[i].textMods.Find(a => a.GetType() == abTextMod.GetType()))
            {
                ataques_basicos[i].cardEffects.Add(remove.Clone(ataques_basicos[i]));
                
                CardTextMod textMod = (CardTextMod)abTextMod.Clone();

                textMod.Init(ataques_basicos[i]);
                ataques_basicos[i].textMods.Add(textMod);
                textMod.UpdateText();
            }
        }
    }
}
