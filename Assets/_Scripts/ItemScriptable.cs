using UnityEngine;


[CreateAssetMenu(menuName = "WildBurnt/Item", fileName = "New Item")]
public class ItemScriptable : ScriptableObject {
    public int Id;
    public StatModel StatModel;

    private void OnValidate() {
        Id = ParseID();
    }

    private int ParseID() {
        int result = 0;
        string baseName = name;
        string[] parts = baseName.Split('_');

        int.TryParse(parts[0], out result);

        return result;
    }
}
