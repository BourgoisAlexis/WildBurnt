using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[Serializable]
public struct GameViewInfos {
    public GameView GameView;
}


public class ConnectionManager : MonoBehaviour {
    #region Variables
    [SerializeField] private GameViewInfos[] _views;

    private SimpleTCPConnection<PeerMessageWildBurnt> _host = new SimpleTCPConnection<PeerMessageWildBurnt>();
    private SimpleTCPConnection<PeerMessageWildBurnt> _guest = new SimpleTCPConnection<PeerMessageWildBurnt>();
    private int _hostIndex = 0;
    private int _guestIndex = 1;

    private Dictionary<MessageType, List<Action>> _actions = new Dictionary<MessageType, List<Action>>();
    #endregion


    private void Awake() {
        _guest.Init(null);
        _host.Init(OnHostInitialized, true, 1);
        _host.OnMessageReceived += OnHostReceivedMessage;
    }

    private void OnDestroy() {
        _guest?.CloseConnection();
        _host?.CloseConnection();
    }


    private void OnHostInitialized(IPEndPoint iPEndPoint) {
        _views[_hostIndex].GameView.ShowMessage("Host open to connection");
        _guest.Connect(iPEndPoint, () => {
            _views[_guestIndex].GameView.ShowMessage("Guest connected to Host");
            _guest.OnMessageReceived += OnGuestReceivedMessage;
        });
    }

    private void OnHostReceivedMessage(PeerMessageWildBurnt message) {
        Debug.Log($"host received a message : {message.ToString()}");
        ReceivedMessage(message, _hostIndex);
    }

    private void OnGuestReceivedMessage(PeerMessageWildBurnt message) {
        Debug.Log($"guest received a message : {message.ToString()}");
        ReceivedMessage(message, _guestIndex);
    }

    private void ReceivedMessage(PeerMessageWildBurnt message, int index) {
        Enum.TryParse(message.Message, out MessageType messageType);

        switch (messageType) {
            case MessageType.CreateTileRow:
                TileData[] datas = JsonUtility.FromJson<JSONableArray<TileData>>(message.Content).Array;
                _views[index].GameView.CreateTileRow(datas);
                break;


            case MessageType.Default:
                break;
        }
    }

    public void Test() {
        TileData[] datas = GameUtils.CreateTileRow();
        _views[0].GameView.CreateTileRow(datas);
        string json = JsonUtility.ToJson(new JSONableArray<TileData>(datas));
        _host.SendMessage(new PeerMessageWildBurnt(MessageType.CreateTileRow, json));
    }
}
