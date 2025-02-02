using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileView : UIButtonAbstract<UnityEvent<int>> {
    #region Variables
    [SerializeField] Image _highlight;
    [SerializeField] private List<GameObject> _voteSpots;

    private TileData _tileData;
    private ViewMap _viewMap;
    #endregion


    public void Init(TileData tileData, ViewMap viewMap) {
        _tileData = tileData;
        _viewMap = viewMap;
        _icon.sprite = DataLoader.Instance.LoadTileSprite((int)tileData.TileType);

        Highlight(false);

        foreach (GameObject spot in _voteSpots)
            spot.transform.localScale = Vector3.zero;
    }

    public void DisplayVotes(List<int> indexes) {
        for (int i = 0; i < indexes.Count; i++) {
            _voteSpots[i].GetComponent<Image>().color = GameUtilsAndConsts.ColorFromPlayerID(i);
            _voteSpots[i].transform.DOScale(Vector3.one * (indexes[i] == _tileData.Index ? 1 : 0), GameUtilsAndConsts.ANIM_DURATION);
        }
    }

    public void Highlight(bool highlight) {
        Color color = highlight ? _uiStyle.Light.SetAlpha(0.5f) : _uiStyle.Light.SetAlpha(0f);
        _highlight.DOColor(color, UIUtilsAndConsts.ANIM_DURATION);
    }

    public override void OnPointerClick(PointerEventData eventData) {
        _viewMap.ClickOnTile(_tileData.Index);
    }
}
