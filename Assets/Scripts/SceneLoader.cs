using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static string sceneToLoad;
    public static string SceneToLoad { get => sceneToLoad; }

    // Load
    public static void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Progress Load
    public static void ProgressLoad(string sceneName)
    {
        sceneToLoad = sceneName;
        SceneManager.LoadScene("LoadingProgress");
    }

    // Reload Stage
    public static void ReloadStage()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        ProgressLoad(currentScene);
    }

    // Load Next Level
    public static void LoadNextStage()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int nextLevel = int.Parse(currentSceneName.Split("Stage")[1]) + 1;
        string nextSceneName = "Stage" + nextLevel;

        if(SceneUtility.GetBuildIndexByScenePath(nextSceneName) == -1)
        {
            Debug.LogError(nextSceneName + " does not exists");
            return;
        }

        ProgressLoad(nextSceneName);
    }
}
