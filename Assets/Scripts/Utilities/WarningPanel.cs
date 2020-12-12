using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPanel : MonoBehaviour
{
    public TMPro.TextMeshProUGUI description;
    public static WarningPanel singleton;

    Vector3 originalScale;
    bool important = false;

    string lastEternalMessage = null;

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }

        originalScale = this.transform.localScale;
        this.gameObject.SetActive(false);
    }
    
    public void ShowWarning(string text, bool isImportant = false, bool isEternal = false)
    {
        if (!important || isImportant) {

            iTween.Stop(this.gameObject);

            this.gameObject.SetActive(true);
            description.text = text;

            this.transform.localScale = Vector3.zero;

            if (isEternal)
            {
                lastEternalMessage = text;

                iTween.ScaleTo(this.gameObject, iTween.Hash(
                    "scale", originalScale,
                    "time", 0.5f,
                    "easetype", "easeOutElastic"));
            }
            else
            {
                iTween.ScaleTo(this.gameObject, iTween.Hash(
                    "scale", originalScale,
                    "time", 0.5f,
                    "easetype", "easeOutElastic",
                    "oncomplete", "Dissapear",
                    "oncompletetarget", this.gameObject
                    ));
            }

            important = isImportant;
        }
    }

    public void Dissapear()
    {
        if(lastEternalMessage != null)
        {
            ShowWarning(lastEternalMessage, false, true);
        }
        else
        {
            iTween.ScaleTo(this.gameObject, iTween.Hash(
                        "scale", Vector3.zero,
                        "time", 0.5f,
                        "delay", 1f,
                        "easetype", "easeInElastic",
                        "oncomplete", "Disable",
                        "oncompletetarget", this.gameObject
                        ));
        }
    }

    public void Disable()
    {
        important = false;
        this.gameObject.SetActive(false);
        lastEternalMessage = null;
    }
}
