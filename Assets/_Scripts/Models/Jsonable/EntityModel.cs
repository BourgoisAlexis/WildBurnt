using System;

[Serializable]
public struct EntityModel {
    public StatModel StatModel;


    public EntityModel(StatModel statModel) {
        StatModel = statModel;
    }
}
