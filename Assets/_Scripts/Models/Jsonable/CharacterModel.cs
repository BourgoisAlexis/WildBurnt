using System;

[Serializable]
public struct CharacterModel {
    public EntityModel EntityModel;
    public int[] Inventory;
    public int[] Gears;


    public CharacterModel(EntityModel entityModel, int inventorySize = 5) {
        EntityModel = entityModel;
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

    public void UpdateInventory(CharacterModel updatedModel) {
        Array.Copy(updatedModel.Inventory, Inventory, Inventory.Length);
        Array.Copy(updatedModel.Gears, Gears, Gears.Length);
    }

    public StatModel GetStats() {
        StatModel result = new StatModel(EntityModel.StatModel);

        foreach (int i in Gears) {
            ItemModel item = DataLoader.Instance.LoadItemModel(i);
            result += item.StatModel;
        }

        return result;
    }
}
