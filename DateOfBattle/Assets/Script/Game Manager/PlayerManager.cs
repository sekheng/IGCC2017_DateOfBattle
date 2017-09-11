using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    [Tooltip("The list of player unit gameobjects!")]
    public List<GameObject> m_playerGOList;

    // The singleton for PlayerManager!
    public static PlayerManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        // Need to make sure only 1 PlayerManager exists!
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start () {
		
	}

    public IEnumerator updateOwnUnits()
    {
        yield break;
    }

    private void OnEnable()
    {
        ObserverSystemScript.Instance.SubscribeEvent("PlayerUnitDied", playerManagerUnitDied);
    }

    private void OnDisable()
    {
        ObserverSystemScript.Instance.UnsubscribeEvent("PlayerUnitDied", playerManagerUnitDied);
    }

    /// <summary>
    /// The function to be called when a player unit died!
    /// </summary>
    protected void playerManagerUnitDied()
    {
        // there is no time to code a perfect system so looping it is fine!
        foreach (GameObject go in m_playerGOList)
        {
            if (go == null)
            {
                m_playerGOList.Remove(go);
                break;
            }
        }
    }
}
