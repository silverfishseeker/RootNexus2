using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemsMananger))]
public class ItemsManagerEditor : Editor {
    public override void OnInspectorGUI() {
        // metemos las cosas básicas por defecto
        DrawDefaultInspector();

        ItemsMananger ie = (ItemsMananger)target;

        if(GUILayout.Button("Comprobar IDs consistentes")) {
            if (ie.EnsureItemIDsConsistency())
                Debug.Log("Todo bien");
            else
                Debug.Log("MAL, algún ID sobra o falta");
        }
        GUILayout.Label ("Usar cuando se eliminan items:");
        if(GUILayout.Button("Limpiar IDs sobrantes en el registro")) {
            ie.ClearMissingIDsInRegister();
            Debug.Log("IDs limpiados");
        }

    }

}
