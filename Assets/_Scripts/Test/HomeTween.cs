using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public static class Extensions {
    public static float[] ToArray(this Vector2 v) => new float[] { v.x, v.y };
    public static float[] ToArray(this Vector3 v) => new float[] { v.x, v.y, v.z };
    public static float[] ToArray(this Vector4 v) => new float[] { v.x, v.y, v.z, v.w };
    public static float[] ToArray(this Color c) => new float[] { c.r, c.g, c.b, c.a };

    public static Vector2 ToVector2(this float[] array) => new Vector2(array[0], array[1]);
    public static Vector3 ToVector3(this float[] array) => new Vector3(array[0], array[1], array[2]);
    public static Vector4 ToVector4(this float[] array) => new Vector4(array[0], array[1], array[2], array[3]);
}

public static class HomeTween {
    public static async Task TweenPosition(Transform t, Vector3 target, SimpleTweenSettings settings) {
        await ExecuteTween(t.position.ToArray(), target.ToArray(), (float[] array) => t.position = array.ToVector3(), settings);
    }

    public static async Task TweenLocalPosition(Transform t, Vector3 target, SimpleTweenSettings settings) {
        await ExecuteTween(t.localPosition.ToArray(), target.ToArray(), (float[] array) => t.localPosition = array.ToVector3(), settings);
    }

    public static async Task TweenLocalPositionY(Transform t, float value, SimpleTweenSettings settings) {
        Vector3 target = new Vector3(t.localPosition.x, value, t.localPosition.z);
        await TweenLocalPosition(t, target, settings);
    }

    public static async Task TweenLocalScale(Transform t, Vector3 target, SimpleTweenSettings settings) {
        await ExecuteTween(t.localScale.ToArray(), target.ToArray(), (float[] array) => t.localScale = array.ToVector3(), settings);
    }

    public static async Task TweenLocalScaleY(Transform t, float value, SimpleTweenSettings settings) {
        Vector3 target = new Vector3(t.localScale.x, value, t.localScale.z);
        await TweenLocalScale(t, target, settings);
    }

    public static async Task TweenSizeDelta(RectTransform t, Vector2 target, SimpleTweenSettings settings) {
        await ExecuteTween(t.sizeDelta.ToArray(), target.ToArray(), (float[] array) => t.sizeDelta = array.ToVector2(), settings);
    }

    public static async Task TweenAnchorMax(RectTransform t, Vector2 target, SimpleTweenSettings settings) {
        await ExecuteTween(t.anchorMax.ToArray(), target.ToArray(), (float[] array) => t.anchorMax = array.ToVector2(), settings);
    }

    public static async Task TweenAnchorMin(RectTransform t, Vector2 target, SimpleTweenSettings settings) {
        await ExecuteTween(t.anchorMin.ToArray(), target.ToArray(), (float[] array) => t.anchorMin = array.ToVector2(), settings);
    }

    public static async Task TweenColor(Image i, Color target, SimpleTweenSettings settings) {
        await ExecuteTween(i.color.ToArray(), target.ToArray(), (float[] array) => i.color = array.ToVector4(), settings);
    }

    public static async Task TweenColor(TextMeshProUGUI t, Color target, SimpleTweenSettings settings) {
        await ExecuteTween(t.color.ToArray(), target.ToArray(), (float[] array) => t.color = array.ToVector4(), settings);
    }

    public static async Task ExecuteTween(float[] start, float[] end, Action<float[]> onUpdate, SimpleTweenSettings settings) {
        if (start.Length != end.Length)
            return;

        if (start.Length == 0)
            return;

        float elapsedTime = 0f;
        int size = start.Length;

        float[] diffs = new float[size];
        float[] result = new float[size];

        for (int i = 0; i < size; i++)
            diffs[i] = end[i] - start[i];

        bool valid = false;

        foreach (float diff in diffs)
            if (diff != 0) {
                valid = true;
                break;
            }

        if (!valid) {
            Debug.LogWarning("start and end values are the same");
            return;
        }

        try {
            while (elapsedTime < settings.Duration) {
                elapsedTime += Time.deltaTime;
                float ratio = elapsedTime / settings.Duration;
                float value = settings.Ease.Evaluate(ratio);

                for (int i = 0; i < size; i++)
                    result[i] = start[i] + diffs[i] * value;

                onUpdate(result);

                await Task.Yield();
            }
        }
        catch (Exception ex) {
            Debug.LogException(ex);
            return;
        }
    }

    public static async void FireAndForget(this Task task, Action<Exception> errorHandler = null) {
        try {
            await task;
        }
        catch (Exception ex) {
            errorHandler?.Invoke(ex);
            Debug.LogException(ex);
        }
    }
}
