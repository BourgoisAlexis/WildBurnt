using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;


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
    private bool _askingForId;

    private int _guestId;

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

        _gameModel.CreatePlayer(out int playerId);
        _connection.Init(OnInit, _host, 1);
        _connection.OnMessageReceived += ReceivedMessage;
    }

    private void OnInit(IPEndPoint iPEndPoint) {
        if (_host && _guest != null)
            _guest.Connect(iPEndPoint);
    }

    public void Connect(IPEndPoint iPEndPoint) {
        _connection.Connect(iPEndPoint, AskForId);
    }


    private async void AskForId() {
        int i = 0;
        _askingForId = true;

        while (_askingForId) {
            SendMessage(MessageType.AskForId);

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
            case MessageType.AskForId: {
                    _gameModel.CreatePlayer(out int playerId);
                    SendMessage(MessageType.GiveId, playerId);
                }
                break;

            case MessageType.GiveId: {
                    _askingForId = false;
                    _guestId = int.Parse(message.Message);
                    _gameModel.CreatePlayer(out int playerId);
                    _gameView.ShowMessage($"Got Id : {_guestId}");
                }
                break;


            //Game
            case MessageType.SetGamePhase:
                int gamePhase = int.Parse(message.Message);
                _gameModel.SetGamePhase((GamePhase)gamePhase);
                _gameView.ShowMessage($"Phase : {_gameModel.GamePhase}");
                break;

            case MessageType.Ready:
                bool ready = bool.Parse(message.Message);
                _gameModel.Ready(message.SenderId, ready, out bool everybodyReady);
                if (everybodyReady && _host)
                    OnEverybodyReady();
                break;


            //Vote
            case MessageType.Vote:
                int voteIndex = int.Parse(message.Message);
                _gameModel.Vote(message.SenderId, voteIndex, out List<int> votes);
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
                TileModel[] tileModels = JsonUtility.FromJson<JsonableArray<TileModel>>(message.Message).Values;
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
                int[] itemIds = JsonUtility.FromJson<JsonableArray<int>>(message.Message).Values;
                _gameModel.LootModel.AddLoots(itemIds);
                _gameView.AddLoots(itemIds);
                VoteCountDown();
                break;

            case MessageType.TakeLoot: {
                    int lootIndex = int.Parse(message.Message);
                    int itemId = _gameModel.LootModel.TakeLoot(lootIndex);
                    if (itemId != GameUtilsAndConsts.EMPTY_ITEM)
                        SendMessage(MessageType.LootTaken, JsonUtility.ToJson(new JsonableArray<int>(new int[] { lootIndex, message.SenderId, itemId })));
                }
                break;

            case MessageType.LootTaken: {
                    int[] lootInfos = JsonUtility.FromJson<JsonableArray<int>>(message.Message).Values;
                    int lootIndex = lootInfos[0];
                    int playerId = lootInfos[1];
                    int itemId = lootInfos[2];
                    _gameModel.LootTaken(playerId, lootIndex, itemId);
                    _gameView.LootTaken(lootIndex);
                    _characterView.UpdateView(playerId);
                }
                break;


            //Gear
            case MessageType.EquipGear: {
                    int index = int.Parse(message.Message);
                    int playerId = message.SenderId;
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
                    _gameModel.GearEquiped(updatedModel);
                    _characterView.UpdateView(updatedModel.Id);
                }
                break;

            //Default
            case MessageType.Default:
                break;
        }

        if (!_host)
            return;

        if (message.SenderId != 0 && !message.IsBroadcasted && !GameUtilsAndConsts.SENT_TO_HOST_ONLY.Contains(message.MessageType))
            Broadcast(message);
    }


    private void SendMessage(PeerMessageWildBurnt message) {
        _messageQueue.Enqueue(message);
    }

    public void SendMessage(MessageType messageType, object obj) {
        SendMessage(messageType, obj.ToString());
    }

    public void SendMessage(MessageType messageType, string content = "Empty") {
        PeerMessageWildBurnt m = new PeerMessageWildBurnt(_guestId, false, messageType, content);
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


    private void OnEverybodyReady() {
        if (_gameModel.GamePhase == GamePhase.Map) {
            TileModel[] tileModels = _gameModel.MapModel.CreateTileRow();
            string json = JsonUtility.ToJson(new JsonableArray<TileModel>(tileModels));
            SendMessage(MessageType.AddTileRow, json);
        }
        else if (_gameModel.GamePhase == GamePhase.Tile) {
            TileModel tileModel = _gameModel.MapModel.GetCurrentTile();
            if (tileModel.TileType == TileType.Loot) {
                int[] itemIds = _gameModel.LootModel.CreateItemSet();
                string json = JsonUtility.ToJson(new JsonableArray<int>(itemIds));
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
