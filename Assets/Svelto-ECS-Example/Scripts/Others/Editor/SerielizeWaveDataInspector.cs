using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WavesData))]
public class SerializeWaveDataInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WavesData myScript = (WavesData)target;
        if (GUILayout.Button("Export Json file"))
        {
            myScript.SerializeData();
        }
    }
}