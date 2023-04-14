using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Canvas canvas;
    public Button buttonLevel;

    private FinishedLevelInfo finishedLevelInfo = null;

    public static string LevelDirectoryPath => "Levels";

    void Start()
    {
        TextAsset[] levels = Resources.LoadAll<TextAsset>(LevelDirectoryPath);

        if (File.Exists(GameController.FinishedLevelsFile))
        {
            string data = File.ReadAllText(GameController.FinishedLevelsFile);
            finishedLevelInfo = JsonConvert.DeserializeObject<FinishedLevelInfo>(data);
        }

        foreach (var level in levels)
        {
            LevelBase convertedLevel = JsonConvert.DeserializeObject<LevelBase>(level.text);
            Button button = Instantiate(this.buttonLevel);
            button.transform.SetParent(canvas.transform, false);
            
            var buttonLevel = button.GetComponent<ButtonLevel>();
            buttonLevel.levelNumber = convertedLevel.levelNumber;

            if (finishedLevelInfo != null && finishedLevelInfo.finishedLevels.ContainsKey(buttonLevel.levelNumber))
                button.GetComponent<Image>().color = Color.green;
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
