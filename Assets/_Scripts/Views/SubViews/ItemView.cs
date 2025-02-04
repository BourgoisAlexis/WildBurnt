using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemView : UIButtonAbstract<UnityEvent<int>> {
    #region Variables
    [SerializeField] Image _highlight;
    [SerializeField] private List<GameObject> _voteSpots;

    private ItemModel _model;
    private ViewLoot _viewLoot;
    #endregion


    public void Init(ItemModel itemModel, ViewLoot viewLoot) {
        _model = itemModel;
        _viewLoot = viewLoot;
        _icon.sprite = DataLoader.Instance.LoadTileSprite(itemModel.Id);

        Highlight(false);

        foreach (GameObject spot in _voteSpots)
            spot.transform.localScale = Vector3.zero;
    }

    public void DisplayVotes(List<int> indexes) {
        for (int i = 0; i < indexes.Count; i++) {
            _voteSpots[i].GetComponent<Image>().color = GameUtilsAndConsts.ColorFromPlayerID(i);
            _voteSpots[i].transform.DOScale(Vector3.one * (indexes[i] == _model.Index ? 1 : 0), GameUtilsAndConsts.ANIM_DURATION);
        }
    }

    public void Highlight(bool highlight) {
        Color color = highlight ? _uiStyle.Light.SetAlpha(0.5f) : _uiStyle.Light.SetAlpha(0f);
        _highlight.DOColor(color, UIUtilsAndConsts.ANIM_DURATION);
    }

    public override void OnPointerClick(PointerEventData eventData) {
        _viewLoot.ClickOnItem(_model.Index);
    }
}
