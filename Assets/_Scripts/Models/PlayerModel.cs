using System;

[Serializable]
public struct PlayerModel {
    public int ID { get; private set; }
    public string UserName { get; private set; }
    public CharacterModel CharacterModel { get; private set; }


    public PlayerModel(int id) {
        ID = id;
        UserName = $"Player_{id}";
        CharacterModel = new CharacterModel(new EntityModel(10, 5, 5));
    }
}