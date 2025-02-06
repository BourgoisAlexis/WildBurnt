using System;
using UnityEngine;

[Serializable]
public struct PlayerModel {
    [field: SerializeField] public int ID { get; private set; }
    public string UserName { get; private set; }
    public CharacterModel CharacterModel { get; private set; }
    [field: SerializeField] public int[] Inventory { get; private set; }


    public PlayerModel(int id) {
        ID = id;
        UserName = $"Player_{id}";
        CharacterModel = new CharacterModel(new EntityModel(10, 5, 5));
        int inventorySize = 10;
        Inventory = new int[inventorySize];
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