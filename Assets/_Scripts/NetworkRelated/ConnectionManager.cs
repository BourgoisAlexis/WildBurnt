using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;


public class ConnectionManager : MonoBehaviour {
    #region Variables
    [SerializeField] private ConnectionManager _guest;
    [SerializeField] private bool _host;

    private GameView _gameView;
    private GameModel _gameModel;
    private SimpleTCPConnection<PeerMessageWildBurnt> _connection;

    private bool _askingForID;
    private int _guestID;

    private List<MessageType> _sendToSelf = new List<MessageType> { 
        MessageType.Vote,
        MessageType.CreateTileRow,
    };
    #endregion


    private void Awake() {
        _gameModel = new GameModel();
        _gameView = GetComponent<GameView>();
        _connection = new SimpleTCPConnection<PeerMessageWildBurnt>();
    }

    private void Start() {
        _gameModel.CreatePlayer();

        _connection.Init(OnInit, _host, 1);
        _connection.OnMessageReceived += ReceivedMessage;
    }

    private void OnDestroy() {
        _connection?.CloseConnection();
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

            await Task.Delay(1000);

            i++;
            if (i > 20) {
                Debug.LogError("Expired");
                break;
            }
        }
    }


    private void ReceivedMessage(PeerMessageWildBurnt message) {
        Debug.Log($"{(_host ? "Host" : "Guest")} received a message : {message.ToString()}");

        switch (message.MessageType) {
            case MessageType.AskForID:
                if (!_host)
                    break;

                int playerID = _gameModel.CreatePlayer();
                SendMessage(MessageType.IDAttribution, playerID.ToString());
                break;

            case MessageType.IDAttribution:
                if (_host)
                    break;

                _askingForID = false;
                _guestID = int.Parse(message.Message);
                _gameModel.CreatePlayer();
                _gameView.ShowMessage($"Got ID : {_guestID}");
                break;

            case MessageType.Vote:
                int value = int.Parse(message.Message);
                List<int> votes = _gameModel.Vote(message.SenderID, value);
                _gameView.DisplayVotes(votes);
                break;

            case MessageType.CreateTileRow:
                TileData[] datas = JsonUtility.FromJson<JSONableArray<TileData>>(message.Message).Array;
                _gameView.AddTileRow(datas);
                break;


            case MessageType.Default:
                break;
        }
    }

    public void SendMessage(MessageType messageType, string content = "Empty") {
        PeerMessageWildBurnt m = new PeerMessageWildBurnt(_guestID, messageType, content);
        _connection.SendMessage(m);

        if (!_sendToSelf.Contains(messageType))
            return;

        ReceivedMessage(m);
    }

    public void Test() {
        TileData[] datas = GameUtils.CreateTileRow();
        string json = JsonUtility.ToJson(new JSONableArray<TileData>(datas));
        SendMessage(MessageType.CreateTileRow, json);
    }
}
