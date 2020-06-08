using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPanel : MonoBehaviour
{
    public TMPro.TextMeshProUGUI description;
    public static WarningPanel singleton;

    Vector3 originalScale;
    bool important = false;

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }

        originalScale = this.transform.localScale;
        this.gameObject.SetActive(false);
    }
    
    public void ShowWarning(string text, bool isImportant = false)
    {
        if (!important) {
            iTween.Stop(this.gameObject);

            this.gameObject.SetActive(true);
            description.text = text;

            this.transform.localScale = Vector3.zero;

            iTween.ScaleTo(this.gameObject, iTween.Hash(
                            "scale", originalScale,
                            "time", 0.5f,
                            "easetype", "easeOutElastic",
                            "oncomplete", "Dissapear",
                            "oncompletetarget", this.gameObject
                            ));

            important = isImportant;
        }
    }

    public void Dissapear()
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

    public void Disable()
    {
        important = false;
        this.gameObject.SetActive(false);
    }
}
