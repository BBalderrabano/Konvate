using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

[CreateAssetMenu(menuName = "Actions/Highlight Card")]
public class MouseHighlightCard : PlayerAction
{
    public SO.GameEvent onCurrentCardSet;
    public SO.GameEvent onPlayerControlState;

    public CardVariable currentCard;
    public CardVariable originalCard;
    public GameState playerControlState;

    public SO.BoolVariable cardIsSelected;

    public override void Execute(float d)
    {
        bool isMouseDown = Input.GetMouseButton(0);

        if (!isMouseDown)
        {
            List<RaycastResult> results = Settings.GetUIObjects();

            foreach (RaycastResult r in results) 
            {
                Area a = r.gameObject.GetComponentInParent<Area>();

                if (a != null)
                {
                    a.OnDrop(Input.mousePosition);
                    break;
                }
            }

            cardIsSelected.value = false;

            originalCard.value.viz.card.MakeBorderInactive();
            currentCard.value.gameObject.SetActive(true);
            currentCard.value = null;
            originalCard.value = null;

            GameManager.singleton.SetState(playerControlState);
            onPlayerControlState.Raise();

            return;
        }
        else
        {
            List<RaycastResult> results = Settings.GetUIObjects();

            bool isInsideCard = false;

            foreach (RaycastResult r in results)
            {
                IClickable c = r.gameObject.GetComponentInParent<IClickable>();
                CardInstance inst = r.gameObject.GetComponentInParent<CardInstance>();

                if (c != null && inst.GetCurrentLogic() is HandCard)
                {
                    currentCard.value.gameObject.SetActive(true);
                    originalCard.value.viz.cardBorder.color = Color.green;
                    cardIsSelected.value = false;
                    c.OnHighlight();

                    isInsideCard = true;

                    break;
                }
            }

            if(!isInsideCard)
            {
                currentCard.value.gameObject.SetActive(false);
                originalCard.value.viz.card.MakeBorderInactive();
                onCurrentCardSet.Raise();
                cardIsSelected.value = true;
            }
        }
    }
}
