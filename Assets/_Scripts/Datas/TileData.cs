using System;

[Serializable]
public struct TileData {
    public int Type;

    public TileData(int type) {
        Type = type;
    }
}
