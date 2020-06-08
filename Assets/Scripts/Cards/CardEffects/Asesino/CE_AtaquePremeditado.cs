using SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(menuName = "Card Effects/Asesino/Ataque Premeditado")]
public class CE_AtaquePremeditado : CardEffect
{
    public BoolVariable doneSelecting;
    public CardListVariable selectedCards;
    public CardEffect maintainEffect;

    public SO.GameEvent PhaseControllerChangeEvent;

    bool init = false;

    public override void Execute()
    {
        base.Execute();

        if (!init)
        {
            init = true;
            doneSelecting.value = false;

            List<Card> handCards = card.owner.handCards;

            handCards.RemoveAll(a => a.cardEffects.Exists(b => b.type == EffectType.MAINTAIN));

            if(handCards.Count <= 0)
            {
                doneSelecting.value = false;
                selectedCards.values.Clear();
                isDone = true;
                init = false;

                return;
            }
            else
            {
                if (card.owner.photonId == GameManager.singleton.localPlayer.photonId)
                { 
                    ScrollSelectionManager.singleton.SelectCards(handCards, "Puedes <b>mantener</b> una carta", false, false, 0, 0, this);
                }
                else
                {
                    ScrollSelectionManager.singleton.WaitForOpponent(this);

                    GameManager.singleton.currentPlayer = GameManager.singleton.clientPlayer;
                    PhaseControllerChangeEvent.Raise();
                }
            }
        }
        else if (doneSelecting.value)
        {
            selectedCards.values[0].cardEffects.Add(maintainEffect);

            doneSelecting.value = false;
            selectedCards.values.Clear();
            isDone = true;
            init = false;

            if (card.owner.photonId != GameManager.singleton.localPlayer.photonId)
            {
                GameManager.singleton.currentPlayer = null;
                PhaseControllerChangeEvent.Raise();
            }
        }
    }
}
