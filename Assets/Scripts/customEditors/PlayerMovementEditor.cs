using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;


[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor {
    

    public override void OnInspectorGUI() {
        // metemos las cosas básicas por defecto
        DrawDefaultInspector();

        PlayerMovement pmTarget = target as PlayerMovement;

        pmTarget.fakeJumpBuffer = EditorGUILayout.Slider("Jump Buffer", pmTarget.jumpBuffer, 0, pmTarget.jumpBufferMax)+1;

    }

}
