using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoAIManager : MonoBehaviour {

    public static PlayerAutoAIManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance)
            Destroy(this);
        else
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	

}
