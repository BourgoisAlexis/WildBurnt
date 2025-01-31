using DG.Tweening;
using UnityEngine;

public class CoinView : MonoBehaviour {
    [SerializeField] private GameObject[] _faces;
    [SerializeField] private Ease _initEase;
    [SerializeField] private Ease _inEase;
    [SerializeField] private Ease _outEase;
    [SerializeField] private float _duration;
    [SerializeField] private float _height;
    [SerializeField] private float _startHeight;
    [SerializeField] private int _spinNumber;

    private RectTransform _rectTransform;
    private int _offSet;

    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
        _offSet = Random.Range(0, _faces.Length);
        _faces[0].SetActive(_offSet % 2 == 0);
        _faces[1].SetActive(_offSet % 2 != 0);
    }

    public async void Flip() {
        await _rectTransform.DOLocalMoveY(_startHeight, _duration).SetEase(_initEase).AsyncWaitForCompletion();
        SecondaryAnim(_duration * 1.3f);
        await _rectTransform.DOLocalMoveY(_height, _duration).SetEase(_inEase).AsyncWaitForCompletion();
        await _rectTransform.DOLocalMoveY(0, _duration).SetEase(_outEase).AsyncWaitForCompletion();
    }

    private async void SecondaryAnim(float duration) {
        float spinN = (float)(_spinNumber + Random.Range(0, _faces.Length));
        duration /= spinN * 2;

        for (int i = _offSet; i < spinN; i++) {
            await _rectTransform.DOScaleY(0, duration).AsyncWaitForCompletion();
            _faces[0].SetActive(i % 2 == 0);
            _faces[1].SetActive(i % 2 != 0);
            await _rectTransform.DOScaleY(1, duration).AsyncWaitForCompletion();
        }
    }
}
