using System.IO;
using Svelto.ECS.Example.Survive;
using UnityEngine;

[ExecuteInEditMode]
public class WavesData : MonoBehaviour
{
    static private bool serializedOnce;

    void Awake()
    {
        if (serializedOnce == false)
        {
            SerializeData();
        }
    }
    public void SerializeData()
    {
        serializedOnce = true;
        var data = GetComponent<EnemyWavesDataSource>();
        JSonEnemyWaveData[] wavesdata = new JSonEnemyWaveData[data.wavesData.Length];

        for (int i = 0; i < data.wavesData.Length; i++)
            wavesdata[i] = new JSonEnemyWaveData(data.wavesData[i]);

        var json = JsonHelper.arrayToJson(wavesdata);

        Utility.Console.Log(json);

        File.WriteAllText(Application.persistentDataPath + "/EnemyWavesData.json", json);
    }
}