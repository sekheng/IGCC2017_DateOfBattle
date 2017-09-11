using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The manager that controls the whole enemy units....hopefully!
/// </summary>
public class EnemyAIManager : MonoBehaviour {
    [Tooltip("The list of enemy unit gameobjects!")]
    public List<GameObject> m_enemyGOList;

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
    }

    public IEnumerator updateOwnUnits()
    {
        // We loop through the unit 1 by 1!
        foreach (GameObject enemyUnitGO in m_enemyGOList)
        {
            // Get the state machine!
            UnitFSM enemyUnitFSM = enemyUnitGO.GetComponent<UnitFSM>();
            // Interact with the state to inform it to move towards the player fortress!
            enemyUnitFSM.GetGenericState("MoveState").interactWithState("PLAYERFORTRESS");
            enemyUnitFSM.ChangeCurrentState("MoveState");
            // Wait till it finishes updating!
            //yield return enemyUnitFSM.updateStateCoroutine;
            //yield return null;
            while (enemyUnitFSM.updateStateCoroutine != null)
                yield return null;
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
    }

    /// <summary>
    /// The function to be called when an enemy unit died only!
    /// </summary>
    protected void enemyManagerUnitDied()
    {
        // there is no time to code a perfect system so looping it is fine!
        foreach (GameObject go in m_enemyGOList)
        {
            if (go == null)
            {
                m_enemyGOList.Remove(go);
                break;
            }
        }
        if (m_enemyGOList.Count <= 0)
            GameManager.Instance.playerWonDisplayAnimation();
    }
}
