using System;
using UnityEngine;

[Serializable]
public struct PlayerModel {
    public int Id;
    public string UserName;
    public CharacterModel CharacterModel;
    public int[] Inventory;
    public int[] Gears;


    public PlayerModel(int id) {
        Id = id;
        UserName = $"Player_{id}";
        CharacterModel = new CharacterModel(new EntityModel(new StatModel(10, 5, 5, 5)));
        Inventory = new int[5];
        Gears = new int[4];
        Array.Fill(Inventory, GameUtilsAndConsts.EMPTY_ITEM);
        Array.Fill(Gears, GameUtilsAndConsts.EMPTY_ITEM);
    }

    public void AddItemToInventory(int itemID) {
        for (int i = 0; i < Inventory.Length; i++) {
            if (Inventory[i] == GameUtilsAndConsts.EMPTY_ITEM) {
                Inventory[i] = itemID;
                return;
            }
        }
    }

    public void AddItemToGears(int inventoryIndex) {
        int value = Inventory[inventoryIndex];
        Inventory[inventoryIndex] = GameUtilsAndConsts.EMPTY_ITEM;
        for (int i = 0; i < Gears.Length; i++) {
            if (Gears[i] == GameUtilsAndConsts.EMPTY_ITEM) {
                Gears[i] = value;
                return;
            }
        }
    }

    public void UpdateInventory(PlayerModel updatedModel) {
        Array.Copy(updatedModel.Inventory, Inventory, Inventory.Length);
        Array.Copy(updatedModel.Gears, Gears, Gears.Length);
    }
}