using UnityEngine;
using UnityEngine.UI;

public class Testouille : MonoBehaviour {
    public SimpleTweenSettings TweenSettings;
    public Color c;

    public async void Test() {
        Image i = GetComponent<Image>();
        await HomeTween.TweenColor(i, c, TweenSettings);
        //Destroy(gameObject, 0.1f);
    }
}
