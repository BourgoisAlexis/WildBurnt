using System.Collections.Generic;
using UnityEngine;

public class ViewLoot : ViewWildBurntBasic {
    #region Variables
    [SerializeField] private RectTransform _top;
    [SerializeField] private RectTransform _bot;

    [SerializeField] private GameObject _prefab;

    private List<ItemView> _items;
    #endregion


    protected override void Awake() {
        _items = new List<ItemView>();
        base.Awake();
    }


    private void Clear() {
        foreach (ItemView itemView in _items)
            Destroy(itemView.gameObject);

        _items.Clear();
    }

    public void AddLoots(ItemModel[] itemModels) {
        Clear();

        int size = itemModels.Length;

        Vector2 midSize = _rectTransform.rect.size;
        float ratio = _top.anchorMin.y - _bot.anchorMax.y;
        midSize.y *= ratio;

        //Horizontal
        float stepH = midSize.x / (float)(size + 1);
        float offsetH = midSize.x / 2 - stepH;

        foreach (ItemModel itemModel in itemModels) {
            int index = itemModel.Index;
            Vector2 pos = new Vector2(index * stepH - offsetH, 0);
            _items.Add(AddSingleItem(pos, itemModels[index]));
        }
    }

    private ItemView AddSingleItem(Vector2 position, ItemModel itemModel) {
        GameObject go = Instantiate(_prefab, _rectTransform);
        ItemView item = go.GetComponent<ItemView>();

        go.AnimateRectTransform();
        go.transform.localPosition = position;

        item.Init(itemModel);
        item.OnClick.AddListener(ClickOnLoot);
        item.SetInteractable(true);

        return item;
    }


    //Actions
    public void ClickOnLoot(int index) {
        _gameView.ClickOnLoot(index);
    }
}
