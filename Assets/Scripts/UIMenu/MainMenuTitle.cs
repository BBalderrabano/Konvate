using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTitle : MonoBehaviour
{
    public bool repeat = false;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAnimate();
    }

    // Update is called once per frame
    void Update()
    {
        if (repeat)
        {
            SpawnAnimate();
            repeat = false;
        }
    }

    void SpawnAnimate()
    {
        iTween.ScaleFrom(gameObject, new Vector3(2f, 2f), 0.7f);
        iTween.FadeFrom(gameObject, 0.5f, 0.7f);
    }
}
