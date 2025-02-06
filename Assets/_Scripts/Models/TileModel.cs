using System;

[Serializable]
public struct TileModel {
    public TileType TileType;


    public TileModel(TileType type) {
        TileType = type;
    }
}
