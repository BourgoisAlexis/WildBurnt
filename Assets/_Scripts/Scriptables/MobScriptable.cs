using UnityEngine;


[CreateAssetMenu(menuName = "WildBurnt/Mob", fileName = "New Mob")]
public class MobScriptable : ScriptableObject {
	public int Id;
	public StatModel StatModel;

	private void OnValidate() {
		Id = ParseID();
	}

	private int ParseID() {
		return int.Parse(name.Split('_')[0]);
	}
}
