using System;

[Serializable]
public struct TileModel {
    public int Index;
    public TileType TileType;

    public TileModel(TileType type, int index) {
        TileType = type;
        Index = index;
    }
}
