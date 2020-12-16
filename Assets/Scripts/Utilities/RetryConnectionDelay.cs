using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RetryConnectionDelay : MonoBehaviour
{
    bool startCountdown = false;
    float timer = 0f;
    public string waiting_text = "Reintentar %%...";
    public string done_text = "Reintentar";
    public int amount_in_seconds = 5;

    public Button button;
    TextMeshProUGUI button_text;

    private void Awake()
    {
        button_text = button.GetComponentInChildren<TextMeshProUGUI>();

        startCountdown = false;
        timer = 0f;
    }

    private void OnEnable()
    {
        startCountdown = true;
        timer = 0f;
    }

    private void OnDisable()
    {
        startCountdown = false;
        timer = 0f;
    }

    void Update()
    {
        if (startCountdown)
        {
            timer += Time.deltaTime;

            int seconds = (int) timer % 60;

            button_text.text = waiting_text.Replace("%%", (amount_in_seconds - seconds).ToString());

            if(seconds >= amount_in_seconds)
            {
                startCountdown = false;
                timer = 0f;
                button.interactable = true;

                button_text.text = done_text;
            }
        }
    }
}
