using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BonusHealthSpawningData))]
public class SerializeBonusHealthDataInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BonusHealthSpawningData myScript = (BonusHealthSpawningData)target;
        if (GUILayout.Button("Export Json file"))
        {
            myScript.SerializeData();
        }
    }
}
