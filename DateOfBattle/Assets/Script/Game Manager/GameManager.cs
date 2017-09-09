using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will handle the entire game flow.
/// Unfortunately, no time to code proper game rules. LOL.
/// </summary>
public class GameManager : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        ObserverSystemScript.Instance.SubscribeEvent("PlayerFortressLost", playerLostFortress);
        ObserverSystemScript.Instance.SubscribeEvent("EnemyFortressLost", enemyLostFortress);
    }

    private void OnDisable()
    {
        ObserverSystemScript.Instance.UnsubscribeEvent("PlayerFortressLost", playerLostFortress);
        ObserverSystemScript.Instance.UnsubscribeEvent("EnemyFortressLost", enemyLostFortress);
    }

    protected void playerLostFortress()
    {

    }

    protected void enemyLostFortress()
    {

    }
}
