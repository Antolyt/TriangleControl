using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveScore : MonoBehaviour {

    private static SaveScore saveScore;
    private static string directoryPath;
    private static string saveGamePath;        // Requieres set in Awake
    public static ScoreData scoreData;

    public static int currentScore;

    void Awake()
    {
        directoryPath = Application.persistentDataPath + "/Savegames";
        saveGamePath = directoryPath + "/scores.dat";
        Directory.CreateDirectory(directoryPath);

        Debug.Log(saveGamePath);

        // make singleton
        if (saveScore == null)
        {
            DontDestroyOnLoad(gameObject);
            saveScore = this;
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Save Game
    /// </summary>
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveGamePath);

        bf.Serialize(file, scoreData);
        file.Close();

        Debug.Log("File Saved");
    }

    /// <summary>
    /// Load Game
    /// </summary>
    public static void Load()
    {
        if (File.Exists(saveGamePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveGamePath, FileMode.Open);
            ScoreData sd = (ScoreData)bf.Deserialize(file);
            file.Close();

            scoreData = sd;
        }
        else
        {
            scoreData = new ScoreData();
        }
    }

    public static void AddPlayerToScoreTable(string name)
    {
        scoreData.scoreNames.Add(new ScoreName(Score.score, name));
        scoreData.scoreNames.Sort();
        if(scoreData.scoreNames.Count > 20)
            scoreData.scoreNames.RemoveAt(scoreData.scoreNames.Count - 1);
        Save();
    }
}

[Serializable]
public class ScoreData
{
    public List<ScoreName> scoreNames;

    public ScoreData()
    {
        scoreNames = new List<ScoreName>();
    }
}

[Serializable]
public struct ScoreName : IComparable<ScoreName>
{
    public int score;
    public string name;

    public ScoreName(int score, string name)
    {
        this.score = score;
        this.name = name;
    }

    public int CompareTo(ScoreName other)
    {
        return other.score.CompareTo(this.score);
    }
}