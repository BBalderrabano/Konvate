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
            if(!ataques_basicos[i].cardEffects.Find(a => a.GetType() == abTextMod.GetType()))
            {
                CardEffect clone = (CardEffect)remove.Clone();

                clone.effectId = int.Parse((i).ToString() + effectId.ToString());
                clone.card = ataques_basicos[i];
                clone.isTemporary = true;

                ataques_basicos[i].cardEffects.Add(clone);

                CardTextMod textMod = (CardTextMod)abTextMod.Clone();

                textMod.Init(ataques_basicos[i]);
                ataques_basicos[i].textMods.Add(textMod);
                textMod.UpdateText();
            }
        }
    }
}
