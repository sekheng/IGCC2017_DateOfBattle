using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The class that will handle scene management
/// </summary>
public class PlayerSceneManager : MonoBehaviour {
    [Header("Values and References needed for Player Scene Manager")]
    [Tooltip("The loading scene name")]
    public string m_loadingSceneName = "LoadingScene";

    // The singleton of this class!
    public static PlayerSceneManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// It will straight away transit to that scene.
    /// </summary>
    /// <param name="sceneName">The scene name which needs to be similar to the name in Unity scene!</param>
    public void TransitToOtherScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// It will go to the loading scene before transiting to other scene so that it might look smooth!
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadThenTransitScene(string sceneName)
    {
    }

    /// <summary>
    /// Unload the scene name!
    /// </summary>
    /// <param name="sceneName">The scene name to be unloaded</param>
    public void UnloadScene(string sceneName)
    {

    }
}
