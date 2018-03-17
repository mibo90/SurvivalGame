using System.IO;
using Svelto.ECS.Example.Survive;
using UnityEngine;

[ExecuteInEditMode]
public class BonusAmmoSpawningData : MonoBehaviour
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
        var data = GetComponents<BonusSpawnDataSource>();
        JSonBonusSpawnData[] spawningdata = new JSonBonusSpawnData[data.Length];

        for (int i = 0; i < data.Length; i++)
            spawningdata[i] = new JSonBonusSpawnData(data[i].spawnData);

        var json = JsonHelper.arrayToJson(spawningdata);

        Utility.Console.Log(json);

        File.WriteAllText(Application.persistentDataPath + "/BonusAmmoSpawningData.json", json);
    }
}