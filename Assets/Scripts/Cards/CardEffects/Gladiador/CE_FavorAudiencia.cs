using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Gladiador/Favor de la Audiencia")]
public class CE_FavorAudiencia : SelectionCardEffect
{
    public override void Execute()
    {
        Card copy = card.owner.all_cards.Find(a => a.cardName == card.cardName && a != this);
        
        copy.statMods.Add(new SMOD_ZeroCost());
        copy.cardViz.RefreshStats();

        List<Card> ataques_basicos = card.owner.handCards.FindAll(a => a.HasTags(CardTag.ATAQUE_BASICO));
        List<Card> defensas = card.owner.handCards.FindAll(a => a.HasTags(CardTag.DEFENSA));

        if(ataques_basicos.Count > 0 && defensas.Count > 0)
        {
            if (card.owner.isLocal)
            {
                List<Card> available = new List<Card>
                {
                    ataques_basicos.First(),
                    defensas.First()
                };

                parentAction.PushAction(new A_CardSelection("Reduce el coste a <sprite=7> y gana <sprite=3>", available, card.owner.photonId, this, card.instanceId));
            }
            else
            {
                GM.actionManager.PushAction(parentAction.actionId, new A_CardSelectionWait(GM.localPlayer.photonId, this, card.instanceId));
            }
        }
        else if(ataques_basicos.Count > 0)
        {
            Card ab = ataques_basicos.First();

            ab.statMods.Add(new SMOD_QuickPlay());
            ab.statMods.Add(new SMOD_ZeroCost());
            ab.cardViz.RefreshStats();
        }
        else if(defensas.Count > 0)
        {
            Card def = defensas.First();

            def.statMods.Add(new SMOD_QuickPlay());
            def.statMods.Add(new SMOD_ZeroCost());
            def.cardViz.RefreshStats();
        }
    }

    public override void DoneSelecting(int[] cardIds)
    {
        if (cardIds != null)
        {
            Card selected = GM.GetCard(cardIds.First());

            selected.statMods.Add(new SMOD_QuickPlay());
            selected.statMods.Add(new SMOD_ZeroCost());
            selected.cardViz.RefreshStats();
        }

        Finish();
    }
}
