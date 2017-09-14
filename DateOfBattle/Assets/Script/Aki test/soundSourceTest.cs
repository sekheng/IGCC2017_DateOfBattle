using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundSourceTest : MonoBehaviour {

    AudioManager manager;
	// Use this for initialization
	void Start () {
        manager = GetComponent<AudioManager>();

    }

    //example,don't use this.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            manager.PlaySE(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            manager.PlaySE(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            manager.PlaySE(2);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            manager.PlayMusic(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            manager.PlayMusic(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            manager.PlayMusic(2);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            manager.bgmLoop = manager.bgmLoop;
        }
    }

}
