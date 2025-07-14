using UnityEngine;


[CreateAssetMenu(menuName = "WildBurnt/Item", fileName = "New Item")]
public class ItemScriptable : ScriptableObject {
	public int Id;
	public StatModel StatModel;

	private void OnValidate() {
		Id = ParseID();
	}

	private int ParseID() {
		return int.Parse(name.Split('_')[0]);
	}
}
