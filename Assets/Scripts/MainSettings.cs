using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainSettings : MonoBehaviour
{

    public static MainSettings Instance;

    public string Name;
    public int BestScore;
    private string SettingFilePath;
    [SerializeField] public TMP_InputField NameField;
    [SerializeField] public TextMeshProUGUI ScoreText;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            SettingFilePath = Application.persistentDataPath + "/gamesettings.json";
            LoadSettings();
            NameField.text = Name;
            UpdateScoreText();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void UpdateScoreText()
    {
        if (Name!=null && Name.Length > 0)
        {
            ScoreText.text = $"Best Score: {Name}:{BestScore}";
        }
        else
        {
            ScoreText.text = "Best Score";
        }
        
    }

    public void StartGame()
    {
        Name = NameField.text.Trim();
        SceneManager.LoadScene("main");
    }

    public void Quit()
    {
        SaveSettings();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void SaveSettings()
    {
        SaveData data = new SaveData();
        data.Name = Name;
        data.BestScore = BestScore;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SettingFilePath, json);
    }

    private void LoadSettings()
    {
        Name = "";
        if (File.Exists(SettingFilePath))
        {
            string json = File.ReadAllText(SettingFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Name = data.Name;
            BestScore = data.BestScore;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string Name;
        public int BestScore;
    }

}
