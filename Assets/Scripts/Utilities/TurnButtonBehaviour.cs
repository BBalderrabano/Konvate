using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurnButtonBehaviour : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    bool buttonDown = false;
    public float timeButtonDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonDown)
        {
            if (timeButtonDown > 0.4f)
            {
                GameManager.singleton.turn.currentPhase.value.OnTurnButtonHold();
            }
            else
            {
                GameManager.singleton.turn.currentPhase.value.OnTurnButtonPress();
            }

            buttonDown = false;
            timeButtonDown = 0;
        }
    }

    void Update()
    {
        if (buttonDown)
        {
            timeButtonDown += Time.deltaTime;

            if (timeButtonDown > 0.4f)
            {
                GameManager.singleton.turn.currentPhase.value.OnTurnButtonHold();

                buttonDown = false;
                timeButtonDown = 0;
            }
        }
    }
}
