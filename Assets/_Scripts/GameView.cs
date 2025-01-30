using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameView : SubManager {
    #region Variables
    [SerializeField] private RectTransform _top;
    [SerializeField] private RectTransform _mid;
    [SerializeField] private RectTransform _bot;

    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GameObject _tilePrefab;

    private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI _tmproMessage;
    private List<TileView[]> _tileRows;

    private int _currentRowIndex => _tileRows.Count - GameUtilsAndConsts.SHOWING_ROWS;
    private int _nextRowIndex => _currentRowIndex + 1;
    private ConnectionManager _connectionManager => _manager.ConnectionManager;
    #endregion


    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
        _tileRows = new List<TileView[]>();
    }

    public async void ShowMessage(string message) {
        _tmproMessage.maxVisibleCharacters = 0;
        _tmproMessage.text = message;

        while (_tmproMessage.maxVisibleCharacters < message.Length) {
            _tmproMessage.maxVisibleCharacters++;
            await Task.Delay(10);
        }
    }


    public void AddCard() {
        GameObject go = Instantiate(_cardPrefab, _mid);
        go.AnimateRectTransform();
    }


    //Tiles
    public void AddTileRow(TileData[] tileDatas) {
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
            _tileRows.Last()[i] = AddTile(pos, tileDatas[i], i);
        }

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

    private TileView AddTile(Vector2 position, TileData datas, int index) {
        GameObject go = Instantiate(_tilePrefab, _mid);
        TileView tile = go.GetComponent<TileView>();

        go.AnimateRectTransform();
        go.transform.localPosition = position;

        tile.Init(datas, index, this);

        return tile;
    }

    public void DisplayVotes(List<int> votes) {
        TileView[] tiles = _tileRows[_nextRowIndex];
        foreach (TileView tile in tiles)
            tile.DisplayVotes(votes);
    }


    //Actions
    public void ClickOnTile(int index) {
        if (_manager.GameModel.GamePhase == GamePhase.VotingForDestination)
            _connectionManager.SendMessage(MessageType.Vote, index);
    }
}
