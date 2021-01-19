using UnityEngine;
using UnityEngine.UI;

public class InifinteScroll : MonoBehaviour
{
    public float speed = 0.5f;

    public Image targetImage;

    void Update()
    {
        float offset = Time.time * speed;

        targetImage.material.mainTextureOffset = new Vector2(0, offset);
    }
}