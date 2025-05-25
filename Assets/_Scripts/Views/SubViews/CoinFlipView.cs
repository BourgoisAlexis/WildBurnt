using System.Threading.Tasks;
using UnityEngine;

public class CoinFlipView : MonoBehaviour {
    #region Variables
    [SerializeField] private GameObject[] _faces;
    [SerializeField] private SimpleTweenSettings _initEase;
    [SerializeField] private SimpleTweenSettings _inEase;
    [SerializeField] private SimpleTweenSettings _outEase;
    [SerializeField] private SimpleTweenSettings _flipEase;
    [SerializeField] private float _height;
    [SerializeField] private float _startHeight;
    [SerializeField] private int _spinNumber;

    private RectTransform _rectTransform;
    private int _offSet;
    #endregion


    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
        _offSet = Random.Range(0, _faces.Length);
        _faces[0].SetActive(_offSet % 2 == 0);
        _faces[1].SetActive(_offSet % 2 != 0);
    }


    public async Task Flip() {
        await HomeTween.TweenLocalPositionY(_rectTransform, _startHeight, _initEase);
        SecondaryAnim(_inEase.Duration + _outEase.Duration * 0.4f);
        await HomeTween.TweenLocalPositionY(_rectTransform, _height, _inEase);
        await HomeTween.TweenLocalPositionY(_rectTransform, 0, _outEase);
    }

    private async void SecondaryAnim(float duration) {
        float spinN = (float)(_spinNumber + Random.Range(0, _faces.Length));
        duration /= spinN * 2;

        _flipEase.Duration = duration;

        for (int i = _offSet; i < spinN; i++) {
            await HomeTween.TweenLocalScaleY(_rectTransform, 0, _flipEase);
            _faces[0].SetActive(i % 2 == 0);
            _faces[1].SetActive(i % 2 != 0);
            await HomeTween.TweenLocalScaleY(_rectTransform, 1, _flipEase);
        }
    }
}
