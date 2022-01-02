using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioClip BUTTON_CLICK;

    public Slider MUSIC_SLIDER;
    public Slider SFX_SLIDER;
    public Slider SPEED_SLIDER;

    public AudioMixer AudioMixer;

    CanvasGroup optionsMenuCanvas;

    readonly string MUSIC_KEY = "MusicVolume";
    readonly string SFX_KEY = "SfxVolume";
    readonly string SPEED_KEY = "TurnSpeed";

    bool active = false;
    bool animating = false;

    public void Start()
    {
        optionsMenuCanvas = GetComponent<CanvasGroup>();

        active = false;
        animating = false;

        SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_KEY, 1f));

        MUSIC_SLIDER.value = Settings.VOLUME_MUSIC;

        SetSfxVolume(PlayerPrefs.GetFloat(SFX_KEY, 1f));

        SFX_SLIDER.value = Settings.VOLUME_SFX;

        SetTurnSpeed(PlayerPrefs.GetFloat(SPEED_KEY, 1f));

        SPEED_SLIDER.value = Settings.GENERAL_TURN_SPEED;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            ToggleOptionsMenu();
        }
    }

    public void ToggleOptionsMenu()
    {
        if (animating)
            return;

        LeanAudio.play(BUTTON_CLICK, Settings.VOLUME_SFX);

        if (!active)
        {
            animating = true;

            optionsMenuCanvas.LeanAlpha(1f, 0.5f).setOnComplete( () => {
                active = true;
                animating = false;
                optionsMenuCanvas.interactable = true;
                optionsMenuCanvas.blocksRaycasts = true;
            });
        }
        else
        {
            animating = true;
            optionsMenuCanvas.interactable = false;

            optionsMenuCanvas.LeanAlpha(0f, 0.5f).setOnComplete(() => {
                active = false;
                animating = false;

                optionsMenuCanvas.blocksRaycasts = false;

                CloseOptionsMenu();
            });
        }
    }

    void CloseOptionsMenu()
    {
        PlayerPrefs.SetFloat(MUSIC_KEY, Settings.VOLUME_MUSIC);
        PlayerPrefs.SetFloat(SFX_KEY, Settings.VOLUME_SFX);
        PlayerPrefs.SetFloat(SPEED_KEY, Settings.GENERAL_TURN_SPEED);
    }

    public void SetMusicVolume(float amount)
    {
        Settings.VOLUME_MUSIC = amount;
        AudioMixer.SetFloat(MUSIC_KEY, (Mathf.Log10(amount) * 20));
    }

    public void SetSfxVolume(float amount)
    {
        Settings.VOLUME_SFX = amount;
        AudioMixer.SetFloat(SFX_KEY, (Mathf.Log10(amount) * 20));
    }

    public void SetTurnSpeed(float amount)
    {
        if (amount < 1)
        {
            Settings.ANIMATION_TIME = 0.5f;
            Settings.ANIMATION_DELAY = 0.2f;
            Settings.ANIMATION_INTERVAL = 0.3f;

            Settings.CHIP_ANIMATION_TIME = 0.7f;
            Settings.CHIP_ANIMATION_DELAY = 0.5f;

            Settings.CARD_EFFECT_MIN_PREVIEW = 1.5f;
            Settings.CARD_EFFECT_PREVIEW_ANIM_DURATION = 0.5f;
        }
        else if (amount > 1)
        {
            Settings.ANIMATION_TIME = 0.3f;
            Settings.ANIMATION_DELAY = 0f;
            Settings.ANIMATION_INTERVAL = 0.1f;

            Settings.CHIP_ANIMATION_TIME = 0.3f;
            Settings.CHIP_ANIMATION_DELAY = 0.3f;

            Settings.CARD_EFFECT_MIN_PREVIEW = 0.6f;
            Settings.CARD_EFFECT_PREVIEW_ANIM_DURATION = 0.3f;
        }
        else if (amount == 1)
        {
            Settings.ANIMATION_TIME = 0.3f;
            Settings.ANIMATION_DELAY = 0f;
            Settings.ANIMATION_INTERVAL = 0.2f;

            Settings.CHIP_ANIMATION_TIME = 0.5f;
            Settings.CHIP_ANIMATION_DELAY = 0.5f;

            Settings.CARD_EFFECT_MIN_PREVIEW = 1f;
            Settings.CARD_EFFECT_PREVIEW_ANIM_DURATION = 0.5f;
        }

        Settings.GENERAL_TURN_SPEED = amount;
    }

    public void Concede()
    {
        LeanAudio.play(BUTTON_CLICK, Settings.VOLUME_SFX);

        MultiplayerManager.singleton.SendConcede(GameManager.singleton.localPlayer.photonId);
    }
}
