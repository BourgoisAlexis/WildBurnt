using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour {
    #region Variables
    [SerializeField] private RectTransform _top;
    [SerializeField] private RectTransform _mid;
    [SerializeField] private RectTransform _bot;

    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GameObject _tilePrefab;

    private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI _tmproMessage;
    [SerializeField] private List<TileView[]> _tileRows;
    [SerializeField] private int _showingRows = 4;

    private int _currentRowIndex => _tileRows.Count - _showingRows;
    #endregion



    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
        _tileRows = new List<TileView[]>();
    }

    public void ShowMessage(string message) {
        _tmproMessage.text = message;
    }

    public void AddCard() {
        GameObject go = Instantiate(_cardPrefab, _mid);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localScale = Vector3.zero;
        rect.DOScale(Vector3.one, UIConsts.ANIM_DURATION);
    }

    public void CreateTileRow(TileData[] tileDatas) {
        int size = tileDatas.Length;
        _tileRows.Add(new TileView[size]);

        Vector2 midSize = _rectTransform.rect.size;
        float ratio = _top.anchorMin.y - _bot.anchorMax.y;
        midSize.y *= ratio;

        //Horizontal
        float stepH = midSize.x / (float)(size + 1);
        float offsetH = midSize.x / 2 - stepH;

        for (int i = 0; i < size; i++) {
            Vector2 pos = new Vector2(i * stepH - offsetH, 0);
            _tileRows.Last()[i] = AddTile(pos, tileDatas[i]);
        }

        //Vertical
        float stepV = midSize.y / (float)(_showingRows + 1);
        float offsetV = midSize.y / 2 - stepV;

        for (int i = 0; i < _tileRows.Count; i++) {
            float y = 0;

            if (_tileRows.Count < _showingRows)
                y = i * stepV - offsetV;
            else if (i >= _currentRowIndex)
                y = (i - _currentRowIndex) * stepV - offsetV;
            else
                y = -midSize.y;

            foreach (TileView tile in _tileRows[i]) {
                RectTransform rect = tile.GetComponent<RectTransform>();
                rect.DOLocalMoveY(y, UIConsts.ANIM_DURATION);
            }
        }
    }

    private TileView AddTile(Vector2 position, TileData datas) {
        GameObject go = Instantiate(_tilePrefab, _mid);
        RectTransform rect = go.GetComponent<RectTransform>();
        TileView tile = go.GetComponent<TileView>();

        rect.localPosition = position;
        rect.localScale = Vector3.zero;
        rect.DOScale(Vector3.one, UIConsts.ANIM_DURATION);

        tile.Init(datas);

        return tile;
    }
}
