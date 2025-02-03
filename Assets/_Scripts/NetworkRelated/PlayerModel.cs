using System;

[Serializable]
public struct PlayerModel {
    public int ID;
    public string UserName;

    public PlayerModel(int id) {
        ID = id;
        UserName = $"Player_{id}";
    }
}