using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Actions/Mouse Hold With Card")]
public class MouseHoldWithCard : PlayerAction
{
    public CardVariable currentCard;
    public CardVariable originalCard;
    public GameState playerControlState;
    public SO.GameEvent onPlayerControlState;

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

            currentCard.value.gameObject.SetActive(true);
            currentCard.value = null;
            originalCard.value = null;

            GameManager.singleton.SetState(playerControlState);
            onPlayerControlState.Raise();

            return;
        }
    }
}
