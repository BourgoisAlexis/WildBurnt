using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameView : SubManager {
    #region Variables
    [SerializeField] private TextMeshProUGUI _tmproMessage;

    [Header("UIViews")]
    [SerializeField] private UIViewManager _viewManager;
    [SerializeField] private ViewMap _viewMap;
    [Header("SubViews")]
    [SerializeField] private CoinView _coinView;
    [SerializeField] private TransitionView _transitionView;

    private ConnectionManager _connectionManager => _manager.ConnectionManager;
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

    public void AddTileRow(TileData[] tileDatas) {
        _viewMap.AddTileRow(tileDatas);
    }

    public void DisplayVotes(List<int> votes) {
        _viewMap.DisplayVotes(votes);
    }

    public async void MoveToDestination(VoteResult result) {
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
        _viewManager.ShowView(1, false, this);
        await _transitionView.Transition(false, result);
        _transitionView.gameObject.SetActive(false);
    }

    public void ClickOnTile(int index) {
        if (_manager.GameModel.GamePhase == GamePhase.VotingForDestination)
            _connectionManager.SendMessage(MessageType.Vote, index);
    }

    public void ViewLodaded() {
        _connectionManager.SendMessage(MessageType.Ready, true);
    }
}
