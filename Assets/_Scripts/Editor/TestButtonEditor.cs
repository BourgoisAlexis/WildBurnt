using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestButton))]
public class TestButtonEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Test")) {
            TestButton b = target as TestButton;
            b.Test();
        }

        if (GUILayout.Button("Reset")) {
            TestButton b = target as TestButton;
            b.ResetTest();
        }
    }
}
