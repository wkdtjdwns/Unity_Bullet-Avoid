using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public string rankName1;
    public float rankScore1;

    public string rankName2;
    public float rankScore2;

    public string rankName3;
    public float rankScore3;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    string path;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        path = Path.Combine(Application.dataPath, "database.json");
        JsonLoad();
    }

    public void JsonLoad()
    {
        SaveData saveData = new SaveData();

        if (!File.Exists(path))
        {
            Reset();
        }

        else
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                GameManager.Instance.rankName1 = saveData.rankName1;
                GameManager.Instance.rankScore1 = saveData.rankScore1;

                GameManager.Instance.rankName2 = saveData.rankName2;
                GameManager.Instance.rankScore2 = saveData.rankScore2;

                GameManager.Instance.rankName3 = saveData.rankName3;
                GameManager.Instance.rankScore3 = saveData.rankScore3;
            }
        }
    }

    public void JsonSave()
    {
        SaveData saveData = new SaveData();

        saveData.rankName1 = GameManager.Instance.rankName1;
        saveData.rankScore1 = GameManager.Instance.rankScore1;

        saveData.rankName2 = GameManager.Instance.rankName2;
        saveData.rankScore2 = GameManager.Instance.rankScore2;

        saveData.rankName3 = GameManager.Instance.rankName3;
        saveData.rankScore3 = GameManager.Instance.rankScore3;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

    public void Reset()
    {
        GameManager.Instance.rankName1 = "OOO";
        GameManager.Instance.rankScore1 = 0;

        GameManager.Instance.rankName2 = "OOO";
        GameManager.Instance.rankScore2 = 0;

        GameManager.Instance.rankName3 = "OOO";
        GameManager.Instance.rankScore3 = 0;

        JsonSave();
    }
}