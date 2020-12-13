using UnityEngine;
using UnityEngine.UI;

public class InifinteScroll : MonoBehaviour
{
    public float speed = 0.5f;

    void Start()
    {

    }

    void Update()
    {
        Vector2 offset = new Vector2(0, Time.time * speed);

        gameObject.GetComponent<Image>().material.mainTextureOffset = offset;
    }
}