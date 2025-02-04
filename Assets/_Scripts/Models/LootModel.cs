public class LootModel {
    public ItemModel[] ItemModels { get; private set; }


    public LootModel() {
        ItemModels = new ItemModel[0];
    }


    public void AddLoots(ItemModel[] itemModels) {
        ItemModels = itemModels;
    }
}
