using System.IO;
using Svelto.ECS.Example.Survive;
using UnityEngine;

[ExecuteInEditMode]
public class BonusHealthSpawningData : MonoBehaviour
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
        var data = GetComponents<BonusHealthSpawnDataSource>();
        JSonBonusHealthSpawnData[] spawningdata = new JSonBonusHealthSpawnData[data.Length];

        for (int i = 0; i < data.Length; i++)
            spawningdata[i] = new JSonBonusHealthSpawnData(data[i].spawnData);

        var json = JsonHelper.arrayToJson(spawningdata);

        Utility.Console.Log(json);

        File.WriteAllText(Application.persistentDataPath + "/BonusHealthSpawningData.json", json);
    }
}