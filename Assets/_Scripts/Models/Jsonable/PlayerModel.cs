using System;

[Serializable]
public struct PlayerModel {
    public int ID;
    public string UserName;
    public CharacterModel CharacterModel;
    public int[] Inventory;


    public PlayerModel(int id) {
        ID = id;
        UserName = $"Player_{id}";
        CharacterModel = new CharacterModel(new EntityModel(new StatModel(10, 5, 5, 5)));
        Inventory = new int[5];
        Array.Fill(Inventory, GameUtilsAndConsts.EMPTY_ITEM);
    }

    public void AddItemToInventory(int itemID) {
        for (int i = 0; i < Inventory.Length; i++) {
            if (Inventory[i] == GameUtilsAndConsts.EMPTY_ITEM) {
                Inventory[i] = itemID;
                return;
            }
        }
    }
}