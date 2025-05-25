public class LootModel {
    public int[] ItemIds { get; private set; }
    public WeightedGenerator WeightedGenerator { get; private set; }

    private const int ITEM_TYPE_NUMBER = 4;


    public LootModel() {
        ItemIds = new int[0];
        WeightedGenerator = new WeightedGenerator(ITEM_TYPE_NUMBER, 1);
    }


    public void AddLoots(int[] itemIds) {
        ItemIds = itemIds;
    }

    public int RemoveLoot(int index) {
        int result = ItemIds[index];
        ItemIds[index] = GameUtilsAndConsts.EMPTY_INT;

        return result;
    }


    public int[] CreateItemSet() {
        int rowSize = UnityEngine.Random.Range(2, 6);
        int[] array = WeightedGenerator.GenerateArray(rowSize);

        return array;
    }
}
