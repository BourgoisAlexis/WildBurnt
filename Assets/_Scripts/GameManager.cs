using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [field: SerializeField] public ConnectionManager ConnectionManager { get; private set; }
    [field: SerializeField] public GameView GameView { get; private set; }
    [field: SerializeField] public CharacterView CharacterView { get; private set; }

    [field: SerializeField] public GameModel GameModel { get; private set; }


    private void Awake() {
        GameModel = new GameModel();
    }

    private void Start() {
        ConnectionManager.Init(this);
        GameView.Init(this);
        CharacterView.Init(this);
    }


    public async void Init() {
        ConnectionManager.SendMessage(MessageType.SetGamePhase, (int)GamePhase.Map);

        for (int i = 0; i < GameUtilsAndConsts.SHOWING_ROWS; i++) {
            TileModel[] tileModels = GameModel.MapModel.CreateTileRow();

            string json = JsonUtility.ToJson(new JSONableArray<TileModel>(tileModels));
            ConnectionManager.SendMessage(MessageType.AddTileRow, json);

            await Task.Delay(1000);
        }
    }

    public async void StartCountDown(int duration, Action<int> onTick, Action onEnd) {
        int value = duration;

        for (int i = 0; i < duration; i++) {
            onTick?.Invoke(value--);
            await Task.Delay(1000);
        }

        onEnd?.Invoke();
    }
}
