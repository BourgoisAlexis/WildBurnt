using DG.Tweening;
using UnityEngine;

public class CharacterView : UIElement {
    [SerializeField] private UIButtonToggle _inventoryButton;
    [SerializeField] private RectTransform _inventoryView;

    private Vector2 _initMin;
    private Vector2 _initMax;

    protected override void Awake() {
        base.Awake();
        _initMin = _inventoryView.anchorMin;
        _initMax = _inventoryView.anchorMax;

        _inventoryView.gameObject.SetActive(false);
        _inventoryView.anchorMax = _initMin;
        _inventoryButton.OnClick.AddListener(ShowInventory);
    }

    public async void ShowInventory(bool show) {
        if (show) {
            _inventoryView.gameObject.SetActive(true);
            _inventoryView.DOAnchorMax(_initMax, _animDuration);
        }
        else {
            await _inventoryView.DOAnchorMax(_initMin, _animDuration).AsyncWaitForCompletion();
            _inventoryView.gameObject.SetActive(false);
        }
    }
}
