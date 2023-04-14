using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    public LevelBase level;
    public InputField inputLevelName;

    public static string LevelDirectoryPath => "Levels";

    private bool hasSetLevel = false;

    void Start()
    {
        if (!Directory.Exists(LevelDirectoryPath))
            Directory.CreateDirectory(LevelDirectoryPath);

        level = new LevelBase();
        ItemAdder.AddedItem += ItemAdder_AddedItem;
        LevelComponentRemover.DestroyedObject += LevelComponentRemover_destroyedObject;


    }

    private void LevelComponentRemover_destroyedObject(GameObject destroyedGameObject)
    {
        foreach (var levelComponent in level.startingLevelComponents)
        {
            if (levelComponent.spawnedGameObject == destroyedGameObject)
            {
                level.startingLevelComponents.Remove(levelComponent);
                break;
            }
        }
    }

    private void ItemAdder_AddedItem(ComponentBehaviour componentBehaviour, GameObject addedGameObject)
    {
        var levelComponent = (LevelComponent)Activator.CreateInstance(
            componentBehaviour.levelComponent.GetType());
        levelComponent.spawnedGameObject = addedGameObject;
        levelComponent.typeName = levelComponent.GetType().Name;
        level.startingLevelComponents.Add(levelComponent);

        addedGameObject.GetComponent<ItemMover>().SetDragging(true);
    }

    public void Load()
    {
        level = Load(inputLevelName.text);
    }

    public static LevelBase Load(string levelFileName)
    {
        LevelBase level = null;

        GameObject gridWorld = GameObject.Find("GridWorld");
        if (gridWorld != null)
        {
            foreach (Transform child in gridWorld.transform)
                Destroy(child.gameObject);
        }

        var text = Resources.Load<TextAsset>(LevelLoader.LevelDirectoryPath + "/" + levelFileName);
        if (text != null)
        {
            level = JsonConvert.DeserializeObject<LevelBase>(text.text);
            for (int i = 0; i < level.startingLevelComponents.Count; i++)
            {
                Type componentType = Type.GetType(level.startingLevelComponents[i].typeName);
                Dictionary<string, object> args = level.startingLevelComponents[i].args;
                level.startingLevelComponents[i] = (LevelComponent)Activator.CreateInstance(componentType);
                level.startingLevelComponents[i].typeName = componentType.Name;
                level.startingLevelComponents[i].args = args;

                if (level.startingLevelComponents[i].Prefab == null)
                    level.startingLevelComponents[i].LoadPrefab();

                GameObject addedObject = Instantiate(level.startingLevelComponents[i].Prefab);
                addedObject.transform.SetParent(gridWorld.transform);
                level.startingLevelComponents[i].InstantiateWithArgs(level.startingLevelComponents[i].args);

                addedObject.transform.position = new Vector2(
                    level.startingLevelComponents[i].startingPosition.x,
                    level.startingLevelComponents[i].startingPosition.y);

                level.startingLevelComponents[i].spawnedGameObject = addedObject;
                var levelComponentSettings = level.startingLevelComponents[i].spawnedGameObject.AddComponent<LevelComponentSettings>();
                levelComponentSettings.settings = level.startingLevelComponents[i].args;

                if (componentType.Name.Contains("Miniature"))
                    level.miniatures.Add(level.startingLevelComponents[i]);
            }
        }

        return level;
    }

    public void Save()
    {
        if (!Directory.Exists(LevelDirectoryPath))
            Directory.CreateDirectory(LevelDirectoryPath);

        foreach (var levelComponent in level.startingLevelComponents)
        {
            levelComponent.startingPosition =
                new CustomPosition(levelComponent.spawnedGameObject.transform.position);

            if (level.lowestPosition.HasValue)
            {
                if (levelComponent.startingPosition.y < level.lowestPosition.Value)
                    level.lowestPosition = levelComponent.startingPosition.y;
            }
            else
                level.lowestPosition = levelComponent.startingPosition.y;

            if (level.highestPosition.HasValue)
            {
                if (levelComponent.startingPosition.y > level.highestPosition.Value)
                    level.highestPosition = levelComponent.startingPosition.y;
            }
            else
                level.highestPosition = levelComponent.startingPosition.y;

            levelComponent.args = levelComponent.spawnedGameObject.GetComponent<LevelComponentSettings>().settings;
        }

        bool editingExistingLevel = inputLevelName.text.Length > 0;
        if (!editingExistingLevel)
        {
            int currentLevel = Resources.LoadAll<TextAsset>(LevelLoader.LevelDirectoryPath).Length + 1;
            level.levelNumber = currentLevel;
            level.levelName = "level" + currentLevel;
        }
        else
            level.levelName = inputLevelName.text;
        string path = $"{Application.dataPath}/Resources/{LevelLoader.LevelDirectoryPath}/{level.levelName}.json";

        using var streamWriter = new StreamWriter(path, false);
        string data = JsonConvert.SerializeObject(level);
        streamWriter.WriteLine(data);
    }
}
