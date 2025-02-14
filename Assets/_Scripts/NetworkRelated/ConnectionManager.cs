using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;


public class ConnectionManager : SubManager {
    #region Variables
    [Header("DEBUG")]
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
    private CharacterView _characterView => _manager.CharacterView;
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
            Debug.Log($"{(_host ? "Host" : "Guest")} received {message.MessageType.ToString()} : {message.ToString()}");

        switch (message.MessageType) {
            //Connection
            case MessageType.AskForID: {
                    int playerId = _gameModel.CreatePlayer();
                    SendMessage(MessageType.GiveID, playerId);
                }
                break;

            case MessageType.GiveID:
                _askingForID = false;
                _guestID = int.Parse(message.Message);
                _gameModel.CreatePlayer();
                _gameView.ShowMessage($"Got ID : {_guestID}");
                break;


            //Game
            case MessageType.SetGamePhase:
                int gamePhase = int.Parse(message.Message);
                _gameModel.SetGamePhase((GamePhase)gamePhase);
                _gameView.ShowMessage($"Phase : {_gameModel.GamePhase}");
                break;

            case MessageType.Ready:
                bool ready = bool.Parse(message.Message);
                _gameModel.Ready(message.SenderID, ready, out bool everybodyReady);
                if (everybodyReady && _host)
                    OnViewLoaded();
                break;


            //Vote
            case MessageType.Vote:
                int voteIndex = int.Parse(message.Message);
                _gameModel.Vote(message.SenderID, voteIndex, out List<int> votes);
                _gameView.DisplayVotes(votes);
                break;

            case MessageType.VoteTimer:
                int timer = int.Parse(message.Message);
                _gameView.ShowMessage($"Vote Timer : {timer}");
                break;

            case MessageType.VoteEnd:
                _gameView.ShowMessage($"Vote Ended");
                OnVoteEnd();
                break;


            //Map
            case MessageType.MoveToTile:
                VoteResult result = JsonUtility.FromJson<VoteResult>(message.Message);
                _gameModel.MapModel.MoveToTile(result);
                _gameView.MoveToTile(result);
                break;

            case MessageType.AddTileRow:
                TileModel[] tileModels = JsonUtility.FromJson<JSONableArray<TileModel>>(message.Message).Array;
                _gameModel.MapModel.AddTileRow(tileModels);
                _gameView.AddTileRow(tileModels);
                //Debug only
                if (_gameModel.MapModel.TileRows.Count >= GameUtilsAndConsts.SHOWING_ROWS)
                    VoteCountDown();
                break;

            case MessageType.BackToMap:
                _gameView.BackToMap();
                break;


            //Loot
            case MessageType.AddLoots:
                ItemModel[] itemModels = JsonUtility.FromJson<JSONableArray<ItemModel>>(message.Message).Array;
                _gameModel.LootModel.AddLoots(itemModels);
                _gameView.AddLoots(itemModels);
                VoteCountDown();
                break;

            case MessageType.TakeLoot: {
                    int lootIndex = int.Parse(message.Message);
                    ItemModel itemModel = _gameModel.LootModel.TakeLoot(lootIndex);
                    if (itemModel.Id != GameUtilsAndConsts.EMPTY_ITEM)
                        SendMessage(MessageType.LootTaken, JsonUtility.ToJson(new JSONableArray<int>(new int[] { lootIndex, message.SenderID, itemModel.Id })));
                }
                break;

            case MessageType.LootTaken: {
                    int[] lootInfos = JsonUtility.FromJson<JSONableArray<int>>(message.Message).Array;
                    int lootIndex = lootInfos[0];
                    int playerId = lootInfos[1];
                    int itemID = lootInfos[2];
                    _gameModel.LootTaken(playerId, lootIndex, itemID);
                    _gameView.LootTaken(lootIndex);
                }
                break;


            //Gear
            case MessageType.EquipGear: {
                    int index = int.Parse(message.Message);
                    int playerId = message.SenderID;
                    PlayerModel playerModel = _gameModel.PlayerModels[playerId];
                    CharacterModel characterModel = playerModel.CharacterModel;
                    if (characterModel.Inventory[index] >= 0) {
                        characterModel.AddItemToGears(index);
                        string json = JsonUtility.ToJson(_gameModel.PlayerModels[playerId]);
                        SendMessage(MessageType.GearEquiped, json);
                    }
                }
                break;

            case MessageType.GearEquiped: {
                    PlayerModel updatedModel = JsonUtility.FromJson<PlayerModel>(message.Message);
                    _gameModel.PlayerModels[updatedModel.Id].CharacterModel.UpdateInventory(updatedModel.CharacterModel);
                }
                break;

            //Default
            case MessageType.Default:
                break;
        }

        if (!_host)
            return;

        if (message.SenderID != 0 && !message.IsBroadcasted && !GameUtilsAndConsts.SENT_TO_HOST_ONLY.Contains(message.MessageType))
            Broadcast(message);
    }


    private void SendMessage(PeerMessageWildBurnt message) {
        _messageQueue.Enqueue(message);
    }

    public void SendMessage(MessageType messageType, object obj) {
        SendMessage(messageType, obj.ToString());
    }

    public void SendMessage(MessageType messageType, string content = "Empty") {
        PeerMessageWildBurnt m = new PeerMessageWildBurnt(_guestID, false, messageType, content);
        _messageQueue.Enqueue(m);
    }

    private async void UpdateMessageQueue() {
        if (Application.isPlaying == false)
            return;

        if (_sendingMessage == true)
            return;

        if (_messageQueue.Count > 0) {
            _sendingMessage = true;

            PeerMessageWildBurnt m = _messageQueue.Dequeue();

            if (_host) {
                if (!GameUtilsAndConsts.SENT_TO_HOST_ONLY.Contains(m.MessageType))
                    await _connection.SendMessageTask(m);
                if (!m.IsBroadcasted && !GameUtilsAndConsts.SENT_TO_GUEST_ONLY.Contains(m.MessageType))
                    ReceivedMessage(m);
            }
            else if (!GameUtilsAndConsts.SENT_TO_GUEST_ONLY.Contains(m.MessageType)) {
                await _connection.SendMessageTask(m);
            }

            if (_logSentMessages)
                Debug.Log($"{(_host ? "Host" : "Guest")} sent {m.MessageType.ToString()} : {m.ToString()}");

            _sendingMessage = false;
        }
    }

    private void Broadcast(PeerMessageWildBurnt message) {
        if (!_host)
            return;

        message.IsBroadcasted = true;
        SendMessage(message);
    }


    private void OnViewLoaded() {
        if (_gameModel.GamePhase == GamePhase.Map) {
            TileModel[] tileModels = _gameModel.MapModel.CreateTileRow();
            string json = JsonUtility.ToJson(new JSONableArray<TileModel>(tileModels));
            SendMessage(MessageType.AddTileRow, json);
        }
        else if (_gameModel.GamePhase == GamePhase.Tile) {
            TileModel tileModel = _gameModel.MapModel.GetCurrentTile();
            if (tileModel.TileType == TileType.Loot) {
                ItemModel[] itemModels = _gameModel.LootModel.CreateItemSet();
                string json = JsonUtility.ToJson(new JSONableArray<ItemModel>(itemModels));
                SendMessage(MessageType.AddLoots, json);
            }
        }

        _gameModel.ClearReadys();
    }

    private void OnVoteEnd() {
        if (_gameModel.GamePhase == GamePhase.Map) {
            SendMessage(MessageType.SetGamePhase, (int)GamePhase.Tile);
            SendMessage(MessageType.MoveToTile, JsonUtility.ToJson(_gameModel.GetVoteResult()));
        }
        else if (_gameModel.GamePhase == GamePhase.Tile) {
            SendMessage(MessageType.SetGamePhase, (int)GamePhase.Map);
            SendMessage(MessageType.BackToMap, "Test");
        }

        _gameModel.ClearVotes();
    }

    private void VoteCountDown() {
        if (_host) {
            _manager.StartCountDown(3, (int v) => {
                SendMessage(MessageType.VoteTimer, v);
            }, () => {
                SendMessage(MessageType.VoteEnd);
            });
        }
    }

    public void TestMessage() {
        SendMessage(MessageType.TakeLoot, 1);
    }
}
