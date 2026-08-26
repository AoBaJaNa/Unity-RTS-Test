using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestManager))]
public class TestManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TestManager manager = (TestManager)target;

        GUILayout.Space(15);

        if(GUILayout.Button("À¯´Ö ½ºÆù(Spawn)", GUILayout.Height(30)))
        {
            manager.SpawnUnit();
        }

        GUILayout.Space(5);
        
        if(GUILayout.Button("À¯´Ö »èÁ¦(Clear)", GUILayout.Height(30)))
        {
            manager.ClearUnits();
        }
    }
}
