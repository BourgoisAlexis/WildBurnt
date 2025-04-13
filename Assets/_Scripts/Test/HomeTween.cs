using System;
using System.Threading.Tasks;
using UnityEngine;


public static class Extensions {
    public static float[] ToArray(this Vector2 v) => new float[] { v.x, v.y };
    public static float[] ToArray(this Vector3 v) => new float[] { v.x, v.y, v.z };

    //public static Vector2 FromArray(this float[] array) => new Vector2(array[0], array[1]);
    public static Vector3 ToVector(this float[] array) => new Vector3(array[0], array[1], array[2]);
}

public static class HomeTween {
    public static void TweenPosition(Transform t, Vector3 destination, SimpleTweenSettings settings) {
        ExecuteTween(t.position.ToArray(), destination.ToArray(), (float[] array) => t.position = array.ToVector(), settings);
    }

    public static void TweenLocalPosition(Transform t, Vector3 destination, SimpleTweenSettings settings) {
        ExecuteTween(t.localPosition.ToArray(), destination.ToArray(), (float[] array) => t.localPosition = array.ToVector(), settings);
    }

    public static async void ExecuteTween(float[] start, float[] end, Action<float[]> onUpdate, SimpleTweenSettings settings) {
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
        catch (Exception e) {
            Debug.LogError($"{e.Message} {e.StackTrace}");
            return;
        }
    }
}
