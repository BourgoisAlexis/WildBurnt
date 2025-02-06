public class LootModel {
    public ItemModel[] ItemModels { get; private set; }
    public WeightedGenerator WeightedGenerator { get; private set; }

    private int _itemNumber = 20;


    public LootModel() {
        ItemModels = new ItemModel[0];
        WeightedGenerator = new WeightedGenerator(_itemNumber, 1);
    }


    public void AddLoots(ItemModel[] itemModels) {
        ItemModels = itemModels;
    }

    public ItemModel TakeLoot(int index) {
        ItemModel result = ItemModels[index];
        ItemModels[index] = new ItemModel(GameUtilsAndConsts.EMPTY_ITEM, new StatModel(0, 0, 0, 0));
        return result;
    }


    public ItemModel[] CreateItemSet() {
        int rowSize = UnityEngine.Random.Range(2, 6);
        int[] array = WeightedGenerator.GenerateArray(rowSize);
        ItemModel[] result = new ItemModel[rowSize];

        for (int i = 0; i < rowSize; i++)
            result[i] = CreateRandomItem(array[i]);

        return result;
    }

    private ItemModel CreateRandomItem(int i) {
        int r = UnityEngine.Random.Range(0, 10);
        StatModel statModel = new StatModel(0, 0, r, 0);

        NetworkUtilsAndConsts.Log($"-Generating random loots atm-");

        return new ItemModel(i, statModel);
    }
}
