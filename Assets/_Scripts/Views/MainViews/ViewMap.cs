using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ViewMap : ViewWildBurnt {
    #region Variables
    [SerializeField] private RectTransform _top;
    [SerializeField] private RectTransform _bot;

    [SerializeField] private GameObject _prefab;

    private List<TileView[]> _tileRows;

    private int _currentRowIndex => _tileRows.Count - GameUtilsAndConsts.SHOWING_ROWS;
    private int _nextRowIndex => _currentRowIndex + 1;
    #endregion


    private void Awake() {
        _tileRows = new List<TileView[]>();
    }


    public void AddTileRow(TileModel[] tileModels) {
        int size = tileModels.Length;
        TileView[] row = new TileView[size];

        Vector2 midSize = _rectTransform.rect.size;
        float ratio = _top.anchorMin.y - _bot.anchorMax.y;
        midSize.y *= ratio;

        //Horizontal
        float stepH = midSize.x / (float)(size + 1);
        float offsetH = midSize.x / 2 - stepH;

        for (int i = 0; i < size; i++) {
            Vector2 pos = new Vector2(i * stepH - offsetH, 0);
            row[i] = AddSingleTile(pos, tileModels[i], i);
        }

        _tileRows.Add(row);

        //Vertical
        float stepV = midSize.y / (float)(GameUtilsAndConsts.SHOWING_ROWS + 1);
        float offsetV = midSize.y / 2 - stepV;

        for (int i = 0; i < _tileRows.Count; i++) {
            float y = 0;

            if (_tileRows.Count < GameUtilsAndConsts.SHOWING_ROWS)
                y = i * stepV - offsetV;
            else if (i >= _currentRowIndex)
                y = (i - _currentRowIndex) * stepV - offsetV;
            else
                y = -midSize.y;

            foreach (TileView tile in _tileRows[i]) {
                tile.SetInteractable(i == _nextRowIndex);
                RectTransform rect = tile.GetComponent<RectTransform>();
                rect.DOLocalMoveY(y, GameUtilsAndConsts.ANIM_DURATION);
            }
        }
    }

    private TileView AddSingleTile(Vector2 position, TileModel tileModel, int index) {
        GameObject go = Instantiate(_prefab, _rectTransform);
        TileView tile = go.GetComponent<TileView>();

        go.AnimateRectTransform();
        go.transform.localPosition = position;

        tile.Init(tileModel, index);
        tile.OnClick.AddListener(ClickOnTile);

        return tile;
    }

    public void DisplayVotes(List<int> votes) {
        TileView[] tiles = _tileRows[_nextRowIndex];
        foreach (TileView tile in tiles)
            tile.DisplayVotes(votes);
    }

    public void DisplaySelectedTile(VoteResult result) {
        TileView[] tiles = _tileRows[_nextRowIndex];
        tiles[result.Index].Highlight(true);
    }


    //Actions
    public void ClickOnTile(int index) {
        _gameView.ClickOnTile(index);
    }
}
