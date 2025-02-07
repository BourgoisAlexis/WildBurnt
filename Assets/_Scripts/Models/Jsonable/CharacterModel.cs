using System;

[Serializable]
public struct CharacterModel {
    public EntityModel EntityModel;
    public int[,] Inventory;


    public CharacterModel(EntityModel entityModel, int inventorySize = 5) {
        EntityModel = entityModel;
        Inventory = new int[inventorySize, inventorySize];

        for (int x = 0; x < Inventory.GetLength(0); x++)
            for (int y = 0; y < Inventory.GetLength(1); y++)
                Inventory[x, y] = GameUtilsAndConsts.EMPTY_ITEM;
    }
}
