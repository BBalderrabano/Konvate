using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Settings
{
    public static ResourcesManager _resourcesManager;

    public static float ANIMATION_TIME = 0.3f;
    public static string ANIMATION_STYLE = "easeInOutQuad";
    public static float ANIMATION_DELAY = 0f;
    public static float ANIMATION_INTERVAL = 0.2f;

    public static float CHIP_ANIMATION_TIME = 0.5f;
    public static float CHIP_ANIMATION_DELAY = 0.5f;

    public static float CARD_EFFECT_MIN_PREVIEW = 1f;
    public static float CARD_EFFECT_PREVIEW_ANIM_DURATION = 0.5f;
    public static float SHUFFLE_MIN_PREVIEW = 0.5f;

    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static ResourcesManager GetResourcesManager() 
    {
        if (_resourcesManager == null)
        {
            _resourcesManager = Resources.Load("ResourcesManager") as ResourcesManager;
            _resourcesManager.Init();
        }

        return _resourcesManager;
    }

    public static List<RaycastResult> GetUIObjects() 
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerData, results);

        return results;
    }

    public static void SetParent(Transform c, Transform p)
    {
        if (c.parent != p)
        {
            c.SetParent(p);
            c.localPosition = Vector3.zero;
            c.localEulerAngles = Vector3.zero;
            c.localScale = Vector3.one;
        }
    }
}