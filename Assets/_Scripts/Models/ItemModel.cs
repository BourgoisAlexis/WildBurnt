using System;

[Serializable]
public struct ItemModel {
    public int Id;
    public int Index;

    public ItemModel(int id, int index) {
        Id = id;
        Index = index;
    }
}
