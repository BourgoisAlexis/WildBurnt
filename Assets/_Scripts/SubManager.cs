using UnityEngine;

public class SubManager : MonoBehaviour {
    protected GameManager _manager;

    public virtual void Init(GameManager manager, params object[] parameters) {
        _manager = manager;
    }
}
