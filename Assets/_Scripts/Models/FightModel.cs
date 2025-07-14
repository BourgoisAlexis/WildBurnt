using System.Linq;

public class FightModel {
	public WeightedGenerator WeightedGenerator { get; private set; }
	public CharacterModel[] Players { get; private set; }
	public CharacterModel[] Mobs { get; private set; }


	public FightModel() {
	}


	public void CreateBoard(CharacterModel[] players, CharacterModel[] mobs) {
		Players = players;
		Mobs = mobs;
	}


	public CharacterModel[] CreateMobSet() {
		if (WeightedGenerator == null)
			WeightedGenerator = new WeightedGenerator(DataLoader.Instance.MobScriptables.Length, 1);

		int rowSize = UnityEngine.Random.Range(2, 6);
		int[] array = WeightedGenerator.GenerateArray(rowSize);

		CharacterModel[] result = array.Select(x => DataLoader.Instance.LoadMobModel(x)).ToArray();

		return result;
	}
}

