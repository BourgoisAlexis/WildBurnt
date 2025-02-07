using System;

[Serializable]
public struct ItemModel {
    public int Id;
    public StatModel StatModel;


    public ItemModel(int id, StatModel statModel) {
        Id = id;
        StatModel = statModel;
    }

    public ItemModel(ItemScriptable scriptable) {
        Id = scriptable.Id;
        StatModel = scriptable.StatModel;
    }
}
