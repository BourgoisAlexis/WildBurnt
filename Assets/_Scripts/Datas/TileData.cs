using System;

[Serializable]
public struct TileData {
    public int Index;
    public TileType TileType;

    public TileData(TileType type, int index) {
        TileType = type;
        Index = index;
    }
}
