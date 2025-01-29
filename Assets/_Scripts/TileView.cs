using UnityEngine;

public class TileView : MonoBehaviour {
    public TileData Data { get; private set; }

    public void Init(TileData data) {
        Data = data;
    }
}
