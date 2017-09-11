﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The move state meant for the enemy unit!
/// </summary>
public class EnemyMoveState : MoveState {
    [Header("Enemy Move State Reference")]
    [Tooltip("The player fortress tile")]
    public GameObject playerFortressTile;
    [Tooltip("Has begin attacked. To keep track whether is it attacking or not!")]
    public bool isItAttacking = false;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        // The name of the state which is "MoveState" can remain the same so as to avoid confusion
        // And find the player fortress!
        if (!playerFortressTile)
            playerFortressTile = GameObject.FindGameObjectWithTag("PlayerFortress");
    }

    public override IEnumerator updateState()
    {
        // if it is attacking to begin with, then change to attack state
        if (isItAttacking)
        {
            m_FSMOwner.ChangeCurrentState("AttackState");
        }

        if (moveToTile)
            // Request the movement from PathRequestManager
            PathRequestManager.RequestPath(transform.position, moveToTile.transform.position, OnPathFound);
        else
        {
            PathRequestManager.RequestPath(transform.position, moveTowardsPos, OnPathFound);
        }
        // Wait till it has find the path!
        while (!hasFinishedPath)
            yield return null;
        int m_MoveCountDown = charStats.m_MoveSpeed, currentMoveIndex = 0;
        // We will count down the movement speed and check to make sure the current index is not greater than the array
        while (m_MoveCountDown > 0 && currentMoveIndex < m_wayptToFollow.Length)
        {
            // If the unit has reached that point, then increment the index
            if (transform.position == m_wayptToFollow[currentMoveIndex])
            {
                ++currentMoveIndex;
                --m_MoveCountDown;
                yield return null;
                // Then check for nearby player units and fortress!
                foreach (GameObject playerUnitGO in PlayerManager.Instance.m_playerGOList)
                {
                    if (charStats.m_Range >= Vector3.Distance(transform.position, playerUnitGO.transform.position))
                    {
                        // Then we can attack! Change to attack state! and send the tile information!
                        m_FSMOwner.GetGenericState("AttackState").interactWithState(playerUnitGO.GetComponent<TileScript>());
                        m_FSMOwner.ChangeCurrentState("AttackState");
                        yield break;
                    }
                }
                // Looping and range is too small, check for fortress distance!
                if (charStats.m_Range >= Vector3.Distance(transform.position, playerFortressTile.transform.position))
                {
                    // Then we can attack! Change to attack state!
                    m_FSMOwner.GetGenericState("AttackState").interactWithState(playerFortressTile.GetComponent<TileScript>());
                    m_FSMOwner.ChangeCurrentState("AttackState");
                    yield break;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, m_wayptToFollow[currentMoveIndex], m_animMoveSpeed);
            }
            yield return null;
        }
        yield break;
    }

    public override bool interactWithState(object argument)
    {
        // If the argument is a string, then it is a message!
        if (argument is string)
        {
            switch (argument as string)
            {
                case "PLAYERFORTRESS":
                    // Then it means move towards the player fortress!
                    moveTowardsPos = playerFortressTile.transform.position;
                    return true;
                case "OUT_OF_RANGE":
                    isItAttacking = false;
                    return true;
                case "DEFEAT_UNIT":
                    isItAttacking = false;
                    return true;
                default:
                    break;
            }
        }
        // If it not what enemy move state wants, then check the parent interact with state!
        return base.interactWithState(argument);
    }
}
