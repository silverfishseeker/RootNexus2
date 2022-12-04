using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameStateEngine))]
public class GameEngineEditor : Editor {
    public override void OnInspectorGUI() {
        // metemos las cosas básicas por defecto
        DrawDefaultInspector();

        // Item item = (Item)target;

        GUILayout.Label ("Establece el dataFolder por defecto:");
        if(GUILayout.Button("SetDefaultDataFolder")) {
            GameStaticAccess.SetDefaultDataFolder();
        }
        EditorGUILayout.LabelField("DataFolder: ", GameStaticAccess.DataFolder);

    }

}
