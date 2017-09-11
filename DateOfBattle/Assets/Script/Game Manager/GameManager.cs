using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will handle the entire game flow.
/// Unfortunately, no time to code proper game rules. LOL.
/// </summary>
public class GameManager : MonoBehaviour {
    [Header("The References to call and link in Unity!")]
    [Tooltip("The gameobject of the screen that display the intro!")]
    public GameObject m_introDisplayGO;
    [Tooltip("The gameobject of the screen that display player won!")]
    public GameObject m_wonDisplayGO;
    [Tooltip("The gameobject of the screen that display player lost!")]
    public GameObject m_lostDisplayGO;

    [Header("Debugging References and they will be automatically linked inside codes. Do not touch!")]
    [Tooltip("How many player units are there!")]
    public int m_PlayerUnitsLeft;
    [Tooltip("How many enemy units are there!")]
    public int m_EnemyUnitsLeft;
    
	// Use this for initialization
	void Start () {
        // When the game starts, just get the number of units for each side!
        m_PlayerUnitsLeft = GameObject.FindGameObjectsWithTag("Player").Length;
        m_EnemyUnitsLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
	}

    private void OnEnable()
    {
        ObserverSystemScript.Instance.SubscribeEvent("PlayerFortressLost", playerLostFortress);
        ObserverSystemScript.Instance.SubscribeEvent("EnemyFortressLost", enemyLostFortress);
        ObserverSystemScript.Instance.SubscribeEvent("PlayerUnitDied", playerUnitDied);
        ObserverSystemScript.Instance.SubscribeEvent("EnemyUnitDied", enemyUnitDied);
    }

    private void OnDisable()
    {
        ObserverSystemScript.Instance.UnsubscribeEvent("PlayerFortressLost", playerLostFortress);
        ObserverSystemScript.Instance.UnsubscribeEvent("EnemyFortressLost", enemyLostFortress);
        ObserverSystemScript.Instance.UnsubscribeEvent("PlayerUnitDied", playerUnitDied);
        ObserverSystemScript.Instance.UnsubscribeEvent("EnemyUnitDied", enemyUnitDied);
    }

    /// <summary>
    /// The API to be used by ObserverSystem when a player unit died!
    /// </summary>
    protected void playerUnitDied()
    {
        --m_PlayerUnitsLeft;
        if (m_PlayerUnitsLeft <= 0)
        {
            // If no more player units, do something!
            m_lostDisplayGO.SetActive(true);
        }
    }

    /// <summary>
    /// The API to be used by ObserverSystem when an enemy unit died!
    /// </summary>
    protected void enemyUnitDied()
    {
        --m_EnemyUnitsLeft;
        if (m_EnemyUnitsLeft <= 0)
        {
            // if no more enemy units, do something!
            m_wonDisplayGO.SetActive(true);
        }
    }

    /// <summary>
    /// Display the losing screen and restart everything if necessary!
    /// There is no need to know if there are any fortress there since other scripts will be calling it!
    /// </summary>
    protected void playerLostFortress()
    {
        m_lostDisplayGO.SetActive(true);
    }

    /// <summary>
    /// Display the winning screen and proceed to the next level if there is any!
    /// There is no need to know if there are any fortress there since other scripts will be calling it!
    /// </summary>
    protected void enemyLostFortress()
    {
        m_wonDisplayGO.SetActive(true);
    }
}
