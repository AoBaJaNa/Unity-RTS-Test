using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraMovement))]
public class CameraMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraMovement cameraMovement = (CameraMovement)target;

        base.OnInspectorGUI();

        if(GUILayout.Button("Camera Initialize",GUILayout.Height(20)))
        {
            cameraMovement.CameraInitialized();
        }
    }
}
