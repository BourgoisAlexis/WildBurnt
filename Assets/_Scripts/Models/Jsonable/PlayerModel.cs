using System;

[Serializable]
public struct PlayerModel {
    public int Id;
    public string UserName;
    public CharacterModel CharacterModel;


    public PlayerModel(int id) {
        Id = id;
        UserName = $"Player_{id}";
        CharacterModel = new CharacterModel(new EntityModel(new StatModel(10, 5, 5, 5)));
    }
}