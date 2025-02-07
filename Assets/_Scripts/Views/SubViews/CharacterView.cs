using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterView : SubManager {
    [SerializeField] private UIButtonToggle _inventoryButton;
    [SerializeField] private RectTransform _inventoryView;
    [SerializeField] private GameObject _prefab;

    private Vector2 _initMin;
    private Vector2 _initMax;

    private int _focusIndex = 0;
    private List<ItemView> _views = new List<ItemView>();


    private void Awake() {
        _initMin = _inventoryView.anchorMin;
        _initMax = _inventoryView.anchorMax;

        _inventoryView.gameObject.SetActive(false);
        _inventoryView.anchorMax = _initMin;
        _inventoryButton.OnClick.AddListener(ShowInventory);
    }


    public async void ShowInventory(bool show) {
        if (show) {
            _inventoryView.gameObject.SetActive(true);
            await _inventoryView.DOAnchorMax(_initMax, UIUtilsAndConsts.ANIM_DURATION).AsyncWaitForCompletion();
            UpdateInventoryView();
        }
        else {
            await _inventoryView.DOAnchorMax(_initMin, UIUtilsAndConsts.ANIM_DURATION).AsyncWaitForCompletion();
            _inventoryView.gameObject.SetActive(false);
            ClearInventoryView();
        }
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
