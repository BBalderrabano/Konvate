using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuTitle : MonoBehaviour
{
    public AudioClip titleSpawn;
    public AudioClip titleGradient;

    [SerializeField]
    bool repeat = false;
    [SerializeField]
    float big_size = 3f;
    [SerializeField]
    float delay = 0.3f;
    [SerializeField]
    float title_anim_time = 0.7f;

    LeanTweenType type = LeanTweenType.easeSpring;

    TextMeshProUGUI text;
    Color alphaColor;

    public List<Button> mainMenuButtons;
    public Button multiplayerButton;

    [SerializeField]
    bool tryGradient = false;

    [SerializeField]
    float gradientDuration = 0.2f;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        alphaColor = text.color;
        alphaColor.a = 0f;

        SpawnAnimate();
    }

    void Update()
    {
        if (repeat)
        {
            SpawnAnimate();
            repeat = false;
        }

        if (tryGradient)
        {
            text.color = Color.white;
            StartGradient();
            tryGradient = false;
        }
    }

    [SerializeField]
    float shakeIntensity = 1f;
    [SerializeField]
    float shakeDuration = 0.2f;
    [SerializeField]
    int shakeRepeat = 3;

    float soundDelay = 0.27f;

    void SpawnAnimate()
    {
        gameObject.transform.localScale = new Vector3(big_size, big_size);
        text.color = alphaColor;

        LeanTween.value(0f, 1f, soundDelay).setOnComplete(()=> {
            LeanAudio.play(titleSpawn, 1f);
        });

        LeanTween.scale(gameObject, Vector3.one, title_anim_time).setDelay(delay).setEase(type);

        LeanTween.move(gameObject, new Vector3(gameObject.transform.position.x + (Random.Range(-1f, 1f) * shakeIntensity), gameObject.transform.position.y + (Random.Range(-1f, 1f) * shakeIntensity)), shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setRepeat(shakeRepeat)
            .setOnComplete(ActivateButtons)
            .setDelay(delay + (title_anim_time * 0.8f));


        text.LeanAlphaText(1, title_anim_time).setDelay(delay);
    }

    void ActivateButtons()
    {
        foreach (Button button in mainMenuButtons)
        {
            button.interactable = true;
        }

        multiplayerButton.interactable = PhotonNetwork.IsConnectedAndReady;

        StartGradient();
    }

    void StartGradient()
    {
        float firstVal = 0f;
        float delay = 0.3f;

        LeanAudio.play(titleGradient, 1f).PlayDelayed(delay);

        LeanTween.value(gameObject, 1f, 0.5f, gradientDuration).setOnUpdate((float val) =>
        {
            Color firstColor = new Color(val, val, val, 1);

            text.colorGradient = new VertexGradient(firstColor, Color.white, firstColor, Color.white);

            firstVal = val;

        })
        .setDelay(delay)
        .setEaseInSine()
        .setOnComplete( ()=>
        {
            LeanTween.value(gameObject, 0.5f, 1f, gradientDuration).setOnUpdate((float val) =>
            {
                Color firstColor = new Color(val, val, val, 1);

                text.colorGradient = new VertexGradient(firstColor, Color.white, firstColor, Color.white);

                firstVal = val;
            }).setEaseOutSine();
        });

        LeanTween.value(gameObject, 1f, 0.5f, gradientDuration).setOnUpdate((float val) =>
        {
            Color firstColor = new Color(firstVal, firstVal, firstVal, 1);
            Color secondColor = new Color(val, val, val, 1);

            text.colorGradient = new VertexGradient(firstColor, secondColor, firstColor, secondColor);

        })
        .setEaseInSine()
        .setDelay(delay + (gradientDuration * .5f))
        .setOnComplete( ()=> 
        {
            LeanTween.value(gameObject, 0.5f, 1f, gradientDuration).setOnUpdate((float val) =>
            {
                Color firstColor = new Color(firstVal, firstVal, firstVal, 1);
                Color secondColor = new Color(val, val, val, 1);

                text.colorGradient = new VertexGradient(firstColor, secondColor, firstColor, secondColor);
            });
        });
    }
}
