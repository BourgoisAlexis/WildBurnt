using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemView : UIButtonAbstract<UnityEvent<int>> {
    #region Variables
    private ItemModel _model;
    #endregion


    public void Init(ItemModel itemModel) {
        _model = itemModel;
        _text.text = itemModel.Id.ToString();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        OnClick?.Invoke(_model.Index);
    }
}
