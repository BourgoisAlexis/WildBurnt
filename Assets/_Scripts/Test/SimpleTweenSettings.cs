using System;
using UnityEngine;


[Serializable]
public class SimpleTweenSettings {
    public float Duration;
    public AnimationCurve Ease;
    public Transform t;

    public SimpleTweenSettings() {
        Ease = new AnimationCurve();
        Ease.keys = new Keyframe[] {
            new Keyframe(0, 0),
            new Keyframe(1, 1),
        };
    }
}
