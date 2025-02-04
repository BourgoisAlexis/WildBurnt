using System;

[Serializable]
public struct PlayerModel {
    public int ID;
    public string UserName;
    public int[,] Inventory;

    public PlayerModel(int id) {
        ID = id;
        UserName = $"Player_{id}";
        Inventory = new int[5, 5];

        for (int x = 0; x < Inventory.GetLength(0); x++)
            for (int y = 0; y < Inventory.GetLength(1); y++)
                Inventory[x, y] = GameUtilsAndConsts.EMPTY_ITEM;
    }
}