using System.Linq;

public class FightModel {
    public WeightedGenerator WeightedGenerator { get; private set; }
    public CharacterModel[] Players { get; private set; }
    public CharacterModel[] Mobs { get; private set; }

    private const int MOB_TYPE_NUMBER = 1;


    public FightModel() {
        WeightedGenerator = new WeightedGenerator(MOB_TYPE_NUMBER, 1);
    }


    public void CreateBoard(CharacterModel[] players, CharacterModel[] mobs) {
        Players = players;
        Mobs = mobs;
    }


    public CharacterModel[] CreateMobSet() {
        int rowSize = UnityEngine.Random.Range(2, 6);
        int[] array = WeightedGenerator.GenerateArray(rowSize);

        CharacterModel[] result = array.Select(x => new CharacterModel(new StatModel(x, x, x, x))).ToArray();

        return result;
    }
}

