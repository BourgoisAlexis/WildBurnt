using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public UnityEvent OnClick;
    
    private Image _background;
    private Image _icon;

    private Color _hovered = Color.black;
    private Color _default = Color.white;
    private float _animDuration = 0.1f;


    private void Awake() {
        Image[] images = GetComponentsInChildren<Image>();

        if (images.Length > 0)
            _background = images[0];

        if (images.Length > 1)
            _icon = images[1];

        _background.color = _default;
        _icon.color = _hovered;
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        transform.DOScale(1.2f, _animDuration);
        _background.DOColor(_hovered, _animDuration);
        _icon.DOColor(_default, _animDuration);
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.DOScale(1f, _animDuration);
        _background.DOColor(_default, _animDuration);
        _icon.DOColor(_hovered, _animDuration);
    }
}
