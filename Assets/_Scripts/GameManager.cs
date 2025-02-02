using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [field: SerializeField] public ConnectionManager ConnectionManager { get; private set; }
    [field: SerializeField] public GameView GameView { get; private set; }

    public GameModel GameModel { get; private set; }


    private void Awake() {
        GameModel = new GameModel();
    }

    private void Start() {
        ConnectionManager.Init(this);
        GameView.Init(this);
    }


    public async void Init() {
        for (int i = 0; i < GameUtilsAndConsts.SHOWING_ROWS; i++) {
            TileData[] datas = GameUtilsAndConsts.CreateTileRow();
            string json = JsonUtility.ToJson(new JSONableArray<TileData>(datas));
            ConnectionManager.SendMessage(MessageType.AddTileRow, json);

            await Task.Delay(1000);
        }

        ConnectionManager.SendMessage(MessageType.UpdateGamePhase, (int)GamePhase.VotingForDestination);

        await Task.Delay(1000);

        StartCountDown(5, (int v) => {
            ConnectionManager.SendMessage(MessageType.VoteTimer, v);
        }, () => {
            ConnectionManager.SendMessage(MessageType.VoteEnd);
        });
    }

    private async void StartCountDown(int duration, Action<int> onTick, Action onEnd) {
        int value = duration;

        for (int i = 0; i < duration; i++) {
            onTick?.Invoke(value--);
            await Task.Delay(1000);
        }

        onEnd?.Invoke();
    }

    public void CheckForGamePhase(MessageType messageType) {
        if (GameModel.GamePhase == GamePhase.VotingForDestination) {
            if (messageType == MessageType.VoteEnd) {
                ConnectionManager.SendMessage(MessageType.MoveToDestination, JsonUtility.ToJson(GameModel.GetVoteResult()));
            }
            else if (messageType == MessageType.Ready) {
                foreach (bool b in GameModel.Readys)
                    if (b == false)
                        return;

                ConnectionManager.SendMessage(MessageType.UpdateGamePhase, (int)GamePhase.VotingForLoot);
            }
        }
    }
}
