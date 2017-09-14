using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTheLevel : MonoBehaviour {
    public void LoadLevel(string levelName)
    {
        PlayerSceneManager.Instance.LoadThenTransitScene(levelName);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitTheGame();
        }
    }
}
