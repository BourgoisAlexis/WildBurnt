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
        ItemModels[index] = new ItemModel(GameUtilsAndConsts.EMPTY_ITEM, GameUtilsAndConsts.EMPTY_ITEM);
        return ItemModels[index];
    }

    public ItemModel[] CreateItemSet() {
        int rowSize = UnityEngine.Random.Range(2, 6);
        int[] array = WeightedGenerator.GenerateArray(rowSize);
        ItemModel[] result = new ItemModel[rowSize];

        for (int i = 0; i < rowSize; i++)
            result[i] = new ItemModel(array[i], i);

        return result;
    }
}
