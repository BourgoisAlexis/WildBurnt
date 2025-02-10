using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterView : SubManager {
    [SerializeField] private UIButtonTab[] _characterButtons;
    [SerializeField] private RectTransform _inventoryView;
    [SerializeField] private RectTransform _statView;
    [SerializeField] private GameObject _prefab;

    private Vector2 _initMin;
    private Vector2 _initMax;

    private List<ItemView> _views;
    private int _focusIndex;
    private bool _shown;


    private void Awake() {
        _initMin = _statView.anchorMin;
        _initMax = _statView.anchorMax;

        _statView.gameObject.SetActive(false);
        _statView.anchorMax = _initMin;

        _views = new List<ItemView>();
        _focusIndex = 0;
        _shown = false;

        for (int i = 0; i < _characterButtons.Length; i++) {
            UIButtonTab b = _characterButtons[i];
            b.SetIndex(i);
            b.SetSelected(false);
            b.OnClick.AddListener(Show);
        }
    }


    public async void Show(int index) {
        if (_shown) {
            _characterButtons[_focusIndex].SetSelected(false);
            if (index == _focusIndex) {
                HideAnim();
            }
            else {
                await HideAnim();
                _characterButtons[index].SetSelected(true);
                ShowAnim();
            }
        }
        else {
            _characterButtons[index].SetSelected(true);
            ShowAnim();
        }

        _focusIndex = index;
    }

    private async Task ShowAnim() {
        _statView.gameObject.SetActive(true);
        await _statView.DOAnchorMax(_initMax, UIUtilsAndConsts.ANIM_DURATION).AsyncWaitForCompletion();
        UpdateInventoryView();
        _shown = true;
    }

    private async Task HideAnim() {
        await _statView.DOAnchorMax(_initMin, UIUtilsAndConsts.ANIM_DURATION).AsyncWaitForCompletion();
        _statView.gameObject.SetActive(false);
        ClearInventoryView();
        _shown = false;
    }

    public void UpdateInventoryView() {
        int[] inventory = _manager.GameModel.PlayerModels[_focusIndex].Inventory;
        foreach (int item in inventory) {
            GameObject go = Instantiate(_prefab, _inventoryView);
            ItemView itemView = go.GetComponent<ItemView>();

            go.AnimateRectTransform();

            itemView.Init(new ItemModel(item, new StatModel(0, 0, 0, 0)), 0);

            _views.Add(itemView);
        }
    }

    private void ClearInventoryView() {
        foreach (ItemView view in _views)
            Destroy(view.gameObject);

        _views.Clear();
    }
}
