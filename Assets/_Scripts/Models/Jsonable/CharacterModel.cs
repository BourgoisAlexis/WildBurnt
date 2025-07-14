using System;

[Serializable]
public struct CharacterModel {
	public string Name;
	public StatModel StatModel;
	public int[] Inventory;
	public int[] Gears;

	public static CharacterModel Empty => new CharacterModel("Empty character", new StatModel(0, 0, 0, 0));


	public CharacterModel(string name, StatModel statModel, int inventorySize = 5) {
		Name = name;
		StatModel = statModel;
		Inventory = new int[5];
		Gears = new int[4];
		Array.Fill(Inventory, GameUtilsAndConsts.EMPTY_INT);
		Array.Fill(Gears, GameUtilsAndConsts.EMPTY_INT);
	}


	public void AddItemToInventory(int itemId) {
		for (int i = 0; i < Inventory.Length; i++) {
			if (Inventory[i] == GameUtilsAndConsts.EMPTY_INT) {
				Inventory[i] = itemId;
				return;
			}
		}
	}

	public void AddItemToGears(int inventoryIndex) {
		int value = Inventory[inventoryIndex];
		Inventory[inventoryIndex] = GameUtilsAndConsts.EMPTY_INT;
		for (int i = 0; i < Gears.Length; i++) {
			if (Gears[i] == GameUtilsAndConsts.EMPTY_INT) {
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
		StatModel result = StatModel;

		foreach (int i in Gears) {
			ItemModel item = DataLoader.Instance.LoadItemModel(i);
			result += item.StatModel;
		}

		return result;
	}
}
