using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameView : SubManager {
    #region Variables
    [SerializeField] private TextMeshProUGUI _tmproMessage;

    [Header("Views")]
    [SerializeField] private UIViewManager _viewManager;
    [SerializeField] private ViewMap _viewMap;

    private ConnectionManager _connectionManager => _manager.ConnectionManager;
    #endregion


    private void Start() {
        _viewManager.Init(this);
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

    public void DisplayVoteResults() {

    }

    public void ClickOnTile(int index) {
        if (_manager.GameModel.GamePhase == GamePhase.VotingForDestination)
            _connectionManager.SendMessage(MessageType.Vote, index);
    }
}
