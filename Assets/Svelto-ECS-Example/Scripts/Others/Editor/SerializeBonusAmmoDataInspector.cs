using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BonusAmmoSpawningData))]
public class SerializeBonusAmmoDataInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BonusAmmoSpawningData myScript = (BonusAmmoSpawningData)target;
        if (GUILayout.Button("Export Json file"))
        {
            myScript.SerializeData();
        }
    }
}
