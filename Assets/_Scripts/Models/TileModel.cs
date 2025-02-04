using System;

[Serializable]
public struct TileModel {
    public TileType TileType;
    public int Index;

    public TileModel(TileType type, int index) {
        TileType = type;
        Index = index;
    }
}
