using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum LevelState
    {
        Running,
        Finished,
    }

    [SerializeField] private RawImage fadeBackground;
    [SerializeField] private LevelController levelController;

    public static string FinishedLevelsFile => Application.persistentDataPath + "/finishedLevels.json";

    public event Action StoppedLevel;
    public event Action StartedLevel;
    public event Action EditingLevel;

    public SceneFader sceneFader;

    public static bool hasStartedGame;

    public LevelState CurrentLevelState { get; private set; }

    public static GameController gameController;
    private void Awake()
    {
        gameController = this;
    }

    void Start()
    {
        CurrentLevelState = LevelState.Running;
        levelController.FinishedLevel += LevelController_FinishedLevel;
        levelController.FailedLevel += LevelController_FailedLevel;

        fadeBackground.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        levelController.FinishedLevel -= LevelController_FinishedLevel;
        levelController.FailedLevel -= LevelController_FailedLevel;
    }

    private void LevelController_FailedLevel(LevelBase failedLevel)
    {
        foreach (Transform child in GameObject.Find("AddedObjects").transform)
            Destroy(child.gameObject);
    }

    private void LevelController_FinishedLevel(LevelBase finishedLevel)
    {
        string data = "";

        var finishedLevelInfo = new FinishedLevelInfo();

        if (!File.Exists(FinishedLevelsFile))
            File.Create(FinishedLevelsFile).Dispose();
        else
        {
            using StreamReader streamReader = new StreamReader(FinishedLevelsFile);
            data = File.ReadAllText(FinishedLevelsFile);
            finishedLevelInfo = JsonConvert.DeserializeObject<FinishedLevelInfo>(data);
        }

        var levelInfo = new LevelInfo
        {
            levelName = SceneManager.GetActiveScene().name,
            completed = true,
            stars = 3
        };

        if (finishedLevelInfo == null)
        {
            finishedLevelInfo = new FinishedLevelInfo();
            finishedLevelInfo.finishedLevels.Add(levelInfo.levelName, levelInfo);
        }
        else if (!finishedLevelInfo.finishedLevels.ContainsKey(levelInfo.levelName))
            finishedLevelInfo.finishedLevels.Add(levelInfo.levelName, levelInfo);
        else
            finishedLevelInfo.finishedLevels[levelInfo.levelName] = levelInfo;

        using StreamWriter streamWriter = new(FinishedLevelsFile, false);
        data = JsonConvert.SerializeObject(finishedLevelInfo);
        streamWriter.WriteLine(data);

        sceneFader.Play();
    }


    public void StartGame()
    {
        CurrentLevelState = LevelState.Running;
        hasStartedGame = true;
        StartedLevel?.Invoke();
    }

    public void ExitLevel()
    {
        SceneManager.LoadScene("LevelWorld");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
