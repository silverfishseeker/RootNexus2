using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor {
    public override void OnInspectorGUI() {
        // metemos las cosas básicas por defecto
        DrawDefaultInspector();
        Item item = (Item)target;
        EditorGUILayout.LabelField("ID: ", item.ID);
        if (item.IsntID) {
            GUILayout.Label ("Pulsa el botón al crear nuevo item");
            if(GUILayout.Button("Generar ID")) {
                if(item.GetNewID())
                    EditorUtility.SetDirty (item);
                    // Marcarlo sucio significa que tiene cambios pendientes, así podemos guardar el ID
                else
                    Debug.Log("No se pudo generar el ID del item "+item);
            }
        }

    }

}
