using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurnButtonBehaviour : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    bool buttonDown = false;

    [System.NonSerialized]
    public float timeButtonDown;

    Button turnButton;

    void Start()
    {
        turnButton = GetComponent<Button>();
    }

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
                GameManager.singleton.turn.currentPhase.value.OnTurnButtonHold(turnButton);
            }
            else
            {
                GameManager.singleton.turn.currentPhase.value.OnTurnButtonPress(turnButton);
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
                GameManager.singleton.turn.currentPhase.value.OnTurnButtonHold(turnButton);

                buttonDown = false;
                timeButtonDown = 0;
            }
        }
    }
}
