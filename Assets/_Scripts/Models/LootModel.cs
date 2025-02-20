public class LootModel {
    public int[] ItemIds { get; private set; }
    public WeightedGenerator WeightedGenerator { get; private set; }

    private int _numberofItems = 4;
    //Trouver une meilleure solution pour la length des items possibles


    public LootModel() {
        ItemIds = new int[0];
        WeightedGenerator = new WeightedGenerator(_numberofItems, 1);
    }


    public void AddLoots(int[] itemIds) {
        ItemIds = itemIds;
    }

    public int TakeLoot(int index) {
        int result = ItemIds[index];
        ItemIds[index] = GameUtilsAndConsts.EMPTY_ITEM;

        return result;
    }


    public int[] CreateItemSet() {
        int rowSize = UnityEngine.Random.Range(2, 6);
        int[] array = WeightedGenerator.GenerateArray(rowSize);

        return array;
    }
}
