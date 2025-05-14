using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileView : UIButtonAbstract<UnityEvent<int>> {
    #region Variables
    [SerializeField] private Image _highlight;
    [SerializeField] private List<GameObject> _voteSpots;

    private TileModel _model;
    private int _index;
    #endregion


    public void Init(TileModel tileModel, int index) {
        _model = tileModel;
        _index = index;
        _icon.sprite = DataLoader.Instance.LoadTileSprite(_model.TileType);

        Highlight(false);

        foreach (GameObject spot in _voteSpots)
            spot.transform.localScale = Vector3.zero;
    }

    public void DisplayVotes(List<int> indexes) {
        for (int i = 0; i < indexes.Count; i++) {
            _voteSpots[i].GetComponent<Image>().color = GameUtilsAndConsts.ColorFromPlayerID(i);
            _voteSpots[i].transform.DOScale(Vector3.one * (indexes[i] == _index ? 1 : 0), GameUtilsAndConsts.ANIM_DURATION);
        }
    }

    public void Highlight(bool highlight) {
        Color targetColor = highlight ? _uiStyle.Light.SetAlpha(0.5f) : _uiStyle.Light.SetAlpha(0f);
        HomeTween.TweenColor(_highlight, targetColor, _tweenSettings);
    }

    public override void OnPointerClick(PointerEventData eventData) {
        OnClick?.Invoke(_index);
    }
}
