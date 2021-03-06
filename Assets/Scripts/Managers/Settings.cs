﻿using System.Collections.Generic;
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

    public static float VOLUME_MUSIC = 1f;
    public static float VOLUME_SFX = 1f;

    public static float TURN_BUTTON_COOLDOWN = 0.5f;

    public static float GENERAL_TURN_SPEED = 1f;

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

    public static float SCREEN_HEIGHT;

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

    public static Vector3 WorldToCanvasPosition(Vector3 worldPosition)
    {
        return Camera.main.WorldToScreenPoint(worldPosition);
    }
}