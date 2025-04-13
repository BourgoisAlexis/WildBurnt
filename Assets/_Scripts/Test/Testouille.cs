using UnityEngine;

public class Testouille : MonoBehaviour {
    public SimpleTweenSettings TweenSettings;
    public Vector3 destination;

    public void Test() {
        Transform t = transform;
        HomeTween.TweenPosition(t, destination, TweenSettings);
        Destroy(gameObject, 0.1f);
        Debug.LogError("testouille is ready to move");
    }
}
