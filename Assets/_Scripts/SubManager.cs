using UnityEngine;

public class SubManager : MonoBehaviour {
    protected GameManager _manager;

    protected ConnectionManager _connectionManager => _manager.ConnectionManager;
    protected GameModel _gameModel => _manager.GameModel;

    public virtual void Init(GameManager manager, params object[] parameters) {
        _manager = manager;
    }
}
