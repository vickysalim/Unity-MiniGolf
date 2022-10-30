using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderManager : MonoBehaviour
{
    // Load
    public static void Load(string sceneName)
    {
        SceneLoader.Load(sceneName);
    }

    // Progress Load
    public static void ProgressLoad(string sceneName)
    {
        SceneLoader.ProgressLoad(sceneName);
    }

    // Reload Stage
    public static void ReloadStage()
    {
        SceneLoader.ReloadStage();
    }

    // Load Next Level
    public static void LoadNextStage()
    {
        SceneLoader.LoadNextStage();
    }
}
