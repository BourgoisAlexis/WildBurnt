using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterView : SubManager {
    #region Variables
    [SerializeField] private RectTransform _parentView;
    [SerializeField] private UIButtonTab[] _characterButtons;
    [SerializeField] private RectTransform _inventoryView;
    [SerializeField] private RectTransform _gearsView;
    [SerializeField] private StatView _statView;
    [SerializeField] private GameObject _prefab;

    private Vector2 _initMin;
    private Vector2 _initMax;

    private List<ItemView> _itemViews;
    private int _focusIndex;
    private bool _shown;
    #endregion


    private void Awake() {
        _initMin = _parentView.anchorMin;
        _initMax = _parentView.anchorMax;

        _parentView.gameObject.SetActive(false);
        _parentView.anchorMax = _initMin;

        _itemViews = new List<ItemView>();
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
        _parentView.gameObject.SetActive(true);
        CharacterModel characterModel = _manager.GameModel.PlayerModels[_focusIndex].CharacterModel;
        _statView.DisplayStats(characterModel.GetStats());
        await _parentView.DOAnchorMax(_initMax, UIUtilsAndConsts.ANIM_DURATION).AsyncWaitForCompletion();
        FillViewPart(_inventoryView, characterModel.Inventory);
        FillViewPart(_gearsView, characterModel.Gears);
        _shown = true;
    }

    private async Task HideAnim() {
        await _parentView.DOAnchorMax(_initMin, UIUtilsAndConsts.ANIM_DURATION).AsyncWaitForCompletion();
        _parentView.gameObject.SetActive(false);
        ClearInventoryView();
        _shown = false;
    }

    public void FillViewPart(RectTransform rect, int[] items) {
        for (int i = 0; i < items.Length; i++) {
            GameObject go = Instantiate(_prefab, rect);
            ItemView itemView = go.GetComponent<ItemView>();
            go.AnimateRectTransform();

            itemView.Init(new ItemModel(items[i], new StatModel(0, 0, 0, 0)), i);

            _itemViews.Add(itemView);

            if (rect == _inventoryView)
                itemView.OnClick.AddListener(ClickOnInventoryItem);
            else
                itemView.OnClick.AddListener(ClickOnGear);
        }
    }

    private void ClearInventoryView() {
        foreach (ItemView view in _itemViews)
            Destroy(view.gameObject);

        _itemViews.Clear();
    }


    private void ClickOnInventoryItem(int index) {
        _connectionManager.SendMessage(MessageType.EquipGear, index);
    }

    private void ClickOnGear(int index) {
        _connectionManager.SendMessage(MessageType.UnequipGear, index);
    }
}
