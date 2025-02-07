using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataLoader))]
public class DataLoaderEditor : Editor {
    public override void OnInspectorGUI() {
        if (GUILayout.Button("Update Datas")) {
            DataLoader loader = target as DataLoader;
            loader.UpdateDatas();
        }

        base.OnInspectorGUI();
    }
}
