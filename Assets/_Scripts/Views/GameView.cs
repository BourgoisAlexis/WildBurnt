using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameView : SubManager {
    #region Variables
    [SerializeField] private TextMeshProUGUI _tmproMessage;

    [Header("MainViews")]
    [SerializeField] private UIViewManager _viewManager;
    [SerializeField] private ViewMap _viewMap;
    [SerializeField] private ViewLoot _viewLoot;
    [Header("SubViews")]
    [SerializeField] private CoinView _coinView;
    [SerializeField] private TransitionView _transitionView;

    private ConnectionManager _connectionManager => _manager.ConnectionManager;
    private GameModel _gameModel => _manager.GameModel;
    #endregion


    private void Start() {
        _viewManager.Init(this);
        _coinView.gameObject.SetActive(false);
        _transitionView.gameObject.SetActive(false);
    }

    public async void ShowMessage(string message) {
        _tmproMessage.maxVisibleCharacters = 0;
        _tmproMessage.text = message;

        while (_tmproMessage.maxVisibleCharacters < message.Length) {
            _tmproMessage.maxVisibleCharacters++;
            await Task.Delay(10);
        }
    }


    //Map
    public void AddTileRow(TileModel[] tileModels) {
        _viewMap.AddTileRow(tileModels);
    }

    public async void MoveToTile(VoteResult result) {
        int delay = 1000;

        if (result.Randomized) {
            _coinView.gameObject.SetActive(true);
            await _coinView.Flip();
            await Task.Delay(delay);
            _coinView.gameObject.SetActive(false);
        }

        _viewMap.DisplaySelectedTile(result);
        await Task.Delay(delay);
        _transitionView.gameObject.SetActive(true);
        await _transitionView.Transition(true, result);
        _viewManager.ShowView(result.Value, false, this);
        await _transitionView.Transition(false, result);
        _transitionView.gameObject.SetActive(false);
    }

    public void ClickOnTile(int index) {
        if (_gameModel.GamePhase != GamePhase.Map)
            return;

        _connectionManager.SendMessage(MessageType.Vote, index);
    }

    public void DisplayVotes(List<int> votes) {
        _viewMap.DisplayVotes(votes);
    }

    public void BackToMap() {
        _viewManager.ShowView(0, false, this);
    }


    //Loot
    private bool IsLootPhase() {
        return (_gameModel.GamePhase == GamePhase.Tile && _gameModel.MapModel.GetCurrentTile().TileType == TileType.Loot);
    }

    public void AddLoots(ItemModel[] itemModels) {
        _viewLoot.AddLoots(itemModels);
    }

    public void ClickOnLoot(int index) {
        if (!IsLootPhase())
            return;

        _connectionManager.SendMessage(MessageType.TakeLoot, index);
    }

    public void LootTaken(int index) {
        if (!IsLootPhase())
            return;

        _viewLoot.LootTaken(index);
    }


    //Views
    public void ViewLoaded() {
        _connectionManager.SendMessage(MessageType.Ready, true);
    }
}
