using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;


public class ConnectionManager : SubManager {
    #region Variables
    [SerializeField] private bool _logSentMessages;
    [SerializeField] private bool _logReceivedMessages;
    [SerializeField] private ConnectionManager _guest;
    [SerializeField] private bool _host;

    private SimpleTCPConnection<PeerMessageWildBurnt> _connection;
    private Queue<PeerMessageWildBurnt> _messageQueue;

    private bool _sendingMessage = false;
    private bool _askingForID;

    private int _guestID;

    private GameModel _gameModel => _manager.GameModel;
    private GameView _gameView => _manager.GameView;
    #endregion


    private void Awake() {
        _connection = new SimpleTCPConnection<PeerMessageWildBurnt>();
        _messageQueue = new Queue<PeerMessageWildBurnt>();
    }

    private void OnDestroy() {
        _connection?.CloseConnection();
    }

    private void Update() {
        UpdateMessageQueue();
    }


    public override void Init(GameManager manager, params object[] parameters) {
        base.Init(manager, parameters);

        _gameModel.CreatePlayer();
        _connection.Init(OnInit, _host, 1);
        _connection.OnMessageReceived += ReceivedMessage;
    }

    private void OnInit(IPEndPoint iPEndPoint) {
        if (_host && _guest != null)
            _guest.Connect(iPEndPoint);
    }

    public void Connect(IPEndPoint iPEndPoint) {
        _connection.Connect(iPEndPoint, AskForID);
    }


    private async void AskForID() {
        int i = 0;
        _askingForID = true;

        while (_askingForID) {
            SendMessage(MessageType.AskForID);

            await Task.Delay(Mathf.RoundToInt(GameUtilsAndConsts.MESSAGE_DELAY * 10 * 1000));

            i++;
            if (i > 20) {
                Debug.LogError("Expired");
                break;
            }
        }
    }

    private void ReceivedMessage(PeerMessageWildBurnt message) {
        if (_logReceivedMessages)
            Debug.Log($"{(_host ? "Host" : "Guest")} received a message : {message.ToString()}");

        int i = GameUtilsAndConsts.EMPTY_VOTE;
        string s = GameUtilsAndConsts.EMPTY_MESSAGE;

        switch (message.MessageType) {
            //Connection
            case MessageType.AskForID:
                if (!_host)
                    break;

                int playerID = _gameModel.CreatePlayer();
                SendMessage(MessageType.GiveID, playerID);
                break;

            case MessageType.GiveID:
                if (_host)
                    break;

                _askingForID = false;
                _guestID = int.Parse(message.Message);
                _gameModel.CreatePlayer();
                _gameView.ShowMessage($"Got ID : {_guestID}");
                break;


            //Game
            case MessageType.UpdateGamePhase:
                i = int.Parse(message.Message);
                _gameModel.UpdateGamePhase((GamePhase)i);
                _gameView.ShowMessage($"Phase : {_gameModel.GamePhase}");
                break;


            //Vote
            case MessageType.Vote:
                i = int.Parse(message.Message);
                List<int> votes = _gameModel.UpdateVote(message.SenderID, i);
                _gameView.DisplayVotes(votes);
                break;

            case MessageType.VoteTimer:
                i = int.Parse(message.Message);
                _gameView.ShowMessage($"Vote Timer : {i}");
                break;

            case MessageType.VoteEnd:
                _gameView.ShowMessage($"Vote Ended");
                break;


            //Map
            case MessageType.MoveToDestination:
                i = int.Parse(message.Message);
                _gameView.ShowMessage($"MoveToDestination : {i}");
                break;

            case MessageType.AddTileRow:
                TileData[] datas = JsonUtility.FromJson<JSONableArray<TileData>>(message.Message).Array;
                _gameModel.AddTileRow(datas);
                _gameView.AddTileRow(_gameModel.TileRows.Last());
                break;


            //Default
            case MessageType.Default:
                break;
        }

        if (_host)
            _manager.CheckForGamePhase(message.MessageType);
    }


    public void SendMessage(MessageType messageType, object obj) {
        SendMessage(messageType, obj.ToString());
    }

    public void SendMessage(MessageType messageType, string content = "Empty") {
        PeerMessageWildBurnt m = new PeerMessageWildBurnt(_guestID, messageType, content);
        _messageQueue.Enqueue(m);
    }

    private async void UpdateMessageQueue() {
        if (_sendingMessage == true)
            return;

        if (_messageQueue.Count > 0) {
            _sendingMessage = true;

            PeerMessageWildBurnt m = _messageQueue.Dequeue();

            await _connection.SendMessageTask(m);

            if (_logSentMessages)
                Debug.Log($"{(_host ? "Host" : "Guest")} sent a message : {m.ToString()}");

            _sendingMessage = false;

            if (GameUtilsAndConsts.DONT_SEND_TO_SELF.Contains(m.MessageType))
                return;

            ReceivedMessage(m);
        }

    }
}
