using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TransitionView : MonoBehaviour {
    [SerializeField] private SimpleTweenSettings _tweenSettings;
    [SerializeField] private Image _mainVisual;

    public async Task Transition(bool transiIn, VoteResult result) {
        _mainVisual.sprite = DataLoader.Instance.LoadTileSprite(result.Value);
        Transform t = _mainVisual.transform;

        t.localScale = Vector3.one * (transiIn ? 0 : 100);
        await HomeTween.TweenLocalScale(t, Vector3.one * (transiIn ? 100 : 0), _tweenSettings);
    }
}
