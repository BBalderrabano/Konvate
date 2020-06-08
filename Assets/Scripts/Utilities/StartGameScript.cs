using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameScript : MonoBehaviour
{
    bool timerEnd = false;
    float waitTimer = 0f;
    public Image img;
    public GameObject displayText;
    public GameObject playerVsText;

    bool fadeCourtain = false;

    void Start()
    {
        if(MultiplayerManager.singleton != null)
        {
            playerVsText.GetComponent<TMPro.TMP_Text>().text = MultiplayerManager.singleton.getVsText();
        }

        img.gameObject.SetActive(true);
        displayText.SetActive(true);
        playerVsText.SetActive(true);

        fadeCourtain = false;
        timerEnd = false;
        waitTimer = 0f;
    }

    void Update()
    {
        if (!timerEnd)
        {
            waitTimer += Time.deltaTime;
            int seconds = Convert.ToInt32(waitTimer % 60);

            if(seconds > 2 && !fadeCourtain)
            {
                StartCoroutine(FadeImage(true));
                fadeCourtain = true;
            }
            else if (seconds > 3)
            {
                if (GameManager.singleton.isMultiplayer)
                {
                    MultiplayerManager.singleton.PlayerIsReady();
                }

                displayText.SetActive(false);
                playerVsText.SetActive(false);
                img.gameObject.SetActive(false);

                timerEnd = true;
            }
        }
    }

    IEnumerator FadeImage(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color  = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                img.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
    }
}
