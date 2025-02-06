using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemView : UIButtonAbstract<UnityEvent<int>> {
    #region Variables
    private ItemModel _model;
    private int _index;
    #endregion


    public void Init(ItemModel itemModel, int index) {
        _model = itemModel;
        _index = index;
        _text.text = itemModel.Id.ToString();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        OnClick?.Invoke(_index);
    }
}
