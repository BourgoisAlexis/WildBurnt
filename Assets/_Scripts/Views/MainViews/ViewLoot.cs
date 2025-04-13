using System.Collections.Generic;
using UnityEngine;


public class ViewLoot : ViewWildBurnt {
    #region Variables
    [SerializeField] private RectTransform _top;
    [SerializeField] private RectTransform _bot;

    [SerializeField] private GameObject _prefab;

    private List<ItemView> _items;
    #endregion


    private void Awake() {
        _items = new List<ItemView>();
    }


    private void Clear() {
        foreach (ItemView itemView in _items)
            Destroy(itemView.gameObject);

        _items.Clear();
    }

    public void AddLoots(int[] itemIds) {
        Clear();

        int size = itemIds.Length;

        Vector2 midSize = _rectTransform.rect.size;
        float ratio = _top.anchorMin.y - _bot.anchorMax.y;
        midSize.y *= ratio;

        //Horizontal
        float stepH = midSize.x / (float)(size + 1);
        float offsetH = midSize.x / 2 - stepH;

        for (int i = 0; i < size; i++) {
            Vector2 pos = new Vector2(i * stepH - offsetH, 0);
            _items.Add(AddSingleItem(pos, DataLoader.Instance.LoadItemModel(itemIds[i]), i));
        }
    }

    private ItemView AddSingleItem(Vector2 position, ItemModel itemModel, int index) {
        GameObject go = Instantiate(_prefab, _rectTransform);
        ItemView item = go.GetComponent<ItemView>();

        go.AnimateRectTransform();
        go.transform.localPosition = position;

        item.Init(itemModel, index);
        item.OnClick.AddListener(ClickOnLoot);
        item.SetInteractable(true);

        return item;
    }

    public void LootTaken(int index) {
        _items[index].SetInteractable(false);
    }


    //Actions
    public void ClickOnLoot(int index) {
        _gameView.ClickOnLoot(index);
    }
}
