﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The manager that controls the whole enemy units....hopefully!
/// </summary>
public class EnemyAIManager : MonoBehaviour {
    [Tooltip("The list of enemy unit gameobjects!")]
    public List<GameObject> m_enemyGOList;
    [Tooltip("The amount of time for each unit to wait before making it's move")]
    public float m_amountOfWaitTime = 1.0f;
    // The coroutine request to wait for the amount of time
    protected WaitForSeconds timeForDelayAI;

    // The singleton for EnemyAIManager
    public static EnemyAIManager Instance
    {
        get; private set;
    }

    /// <summary>
    /// Setting up singleton!
    /// </summary>
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

    // Use this for initialization
    void Start () {
        // The simplest conversion!
        m_enemyGOList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        timeForDelayAI = new WaitForSeconds(m_amountOfWaitTime);
    }

    public IEnumerator updateOwnUnits()
    {
        // We loop through the unit 1 by 1!
        foreach (GameObject enemyUnitGO in m_enemyGOList)
        {
            // Wait for the some time before the AI make its move
            yield return timeForDelayAI;
            // Activate the unit range ring!
            CharacterScript charStat = enemyUnitGO.GetComponent<CharacterScript>();
            charStat.unitRangeRingGO.SetActive(true);
            // Get the state machine!
            UnitFSM enemyUnitFSM = enemyUnitGO.GetComponent<UnitFSM>();
            // Interact with the state to inform it to move towards the player fortress!
            enemyUnitFSM.GetGenericState("MoveState").interactWithState("PLAYERFORTRESS");
            enemyUnitFSM.ChangeCurrentState("MoveState");
            // Wait till it finishes updating!
            while (enemyUnitFSM.updateStateCoroutine != null)
                yield return null;
            charStat.unitRangeRingGO.SetActive(false);
        }
        // When everything is done, then switch the turn at GameManager!
        GameManager.Instance.isItPlayerTurn = true;
        yield break;
    }

    private void OnEnable()
    {
        ObserverSystemScript.Instance.SubscribeEvent("EnemyUnitDied", enemyManagerUnitDied);
    }

    private void OnDisable()
    {
        ObserverSystemScript.Instance.UnsubscribeEvent("EnemyUnitDied", enemyManagerUnitDied);
        StopCoroutine("updateOwnUnits");
    }

    /// <summary>
    /// The function to be called when an enemy unit died only!
    /// </summary>
    protected void enemyManagerUnitDied()
    {
        m_enemyGOList.Remove(ObserverSystemScript.Instance.GetStoredEventVariable<GameObject>("EnemyUnitDied"));
        ObserverSystemScript.Instance.removeTheEventVariableNextFrame("EnemyUnitDied");
        if (m_enemyGOList.Count <= 0)
            GameManager.Instance.playerWonDisplayAnimation();
    }
}
