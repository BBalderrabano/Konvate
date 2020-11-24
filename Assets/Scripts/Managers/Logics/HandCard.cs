using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Logics/Hand Card")]
public class HandCard : CardLogic
{
    public SO.GameEvent onCurrentCardSet;
    public SO.GameEvent onCardHighlight;

    public CardVariable currentCard;
    public CardVariable originalCard;
    public GameState highlightCard;

    public override void OnClick(CardInstance inst)
    {
    }

    public override void OnHighlight(CardInstance inst)
    {
        if (ScrollSelectionManager.singleton.isActive)
            return;

        if (originalCard.value != null && originalCard.value != inst)
            originalCard.value.viz.card.MakeBorderInactive();

        if (currentCard.value != null && currentCard.value != inst)
            currentCard.value.viz.card.MakeBorderInactive();

        originalCard.Set(inst);
        currentCard.Set(inst);
        GameManager.singleton.SetState(highlightCard);
        onCardHighlight.Raise();
    }

    public override void OnSetLogic(CardInstance inst)
    {
        inst.viz.cardBackImage.gameObject.SetActive(false);
    }
}
