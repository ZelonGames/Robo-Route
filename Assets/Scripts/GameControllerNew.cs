using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerNew : MonoBehaviour
{
    public enum LevelState
    {
        Running,
        Finished,
    }

    [SerializeField] private LevelControllerNew levelController;

    public static string FinishedLevelsFile => Application.persistentDataPath + "/finishedLevels.json";

    public event Action StoppedLevel;
    public event Action StartedLevel;
    public event Action EditingLevel;

    public static bool hasStartedGame;

    public LevelState CurrentLevelState { get; private set; }

    public static GameControllerNew gameController;
    private void Awake()
    {
        gameController = this;
    }

    void Start()
    {
        CurrentLevelState = LevelState.Running;
        levelController.FinishedLevel += LevelController_FinishedLevel;
        levelController.FailedLevel += LevelController_FailedLevel;
    }

    private void LevelController_FailedLevel(LevelBase failedLevel)
    {
        foreach (Transform child in GameObject.Find("AddedObjects").transform)
            Destroy(child.gameObject);
    }

    private void LevelController_FinishedLevel(LevelBase finishedLevel)
    {
        if (finishedLevel == null)
        {
            SceneManager.LoadScene("LevelMenu");
            return;
        }

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
            levelName = finishedLevel.levelName,
            levelNumber = finishedLevel.levelNumber,
            cleared = true,
            stars = 3
        };

        if (finishedLevelInfo == null)
        {
            finishedLevelInfo = new FinishedLevelInfo();
            finishedLevelInfo.finishedLevels.Add(levelInfo.levelNumber, levelInfo);
        }
        else if (!finishedLevelInfo.finishedLevels.ContainsKey(levelInfo.levelNumber))
            finishedLevelInfo.finishedLevels.Add(levelInfo.levelNumber, levelInfo);
        else
            finishedLevelInfo.finishedLevels[levelInfo.levelNumber] = levelInfo;

        using StreamWriter streamWriter = new StreamWriter(FinishedLevelsFile, false);
        data = JsonConvert.SerializeObject(finishedLevelInfo);
        streamWriter.WriteLine(data);

        SceneManager.LoadScene("LevelMenu");
    }

    public void StartGame()
    {
        CurrentLevelState = LevelState.Running;
        hasStartedGame = true;
        StartedLevel?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
