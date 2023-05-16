using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private SceneFader sceneFaderIn;
    private FinishedLevelInfo finishedLevelInfo = null;

    public static string LastPlayedLevelFile => Application.persistentDataPath + "/lastPlayedLevel.txt";
    public static string LevelDirectoryPath => "Levels";

    void Start()
    {
        if (!File.Exists(GameController.FinishedLevelsFile))
            File.Create(GameController.FinishedLevelsFile).Dispose();

        ButtonLevel.Clicked += OnLoadLevel;

        string data = File.ReadAllText(GameController.FinishedLevelsFile);
        finishedLevelInfo = JsonConvert.DeserializeObject<FinishedLevelInfo>(data);

        var tutorial = GameObject.Find("Tutorial").GetComponent<ButtonLevel>();

        UpdateLevelStatusRecursive(tutorial);

        if (!File.Exists(LastPlayedLevelFile))
            return;

        try
        {
            string lastPlayedLevel = File.ReadLines(LastPlayedLevelFile).First();
            Vector2 lastPlayedLevelPosition = GameObject.Find(lastPlayedLevel).transform.position;
            camera.transform.position = new Vector3(lastPlayedLevelPosition.x, lastPlayedLevelPosition.y, camera.transform.position.z);
        }
        catch
        {

        }
    }

    private void OnDestroy()
    {
        ButtonLevel.Clicked -= OnLoadLevel;
    }

    private void OnLoadLevel(string levelToLoad)
    {
        sceneFaderIn.sceneName = levelToLoad;
        sceneFaderIn.Play();
    }

    private void UpdateLevelStatusRecursive(ButtonLevel currentButton)
    {
        if (finishedLevelInfo.finishedLevels.TryGetValue(currentButton.gameObject.name, out var levelInfo))
        {
            currentButton.Complete();
            foreach (ButtonLevel buttonLevel in currentButton.unlockingLevels)
            {
                buttonLevel.Unlock();
                UpdateLevelStatusRecursive(buttonLevel);
            }
        }
    }
}
