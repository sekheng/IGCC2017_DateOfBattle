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
    [Tooltip("The amount of time to wait then it starts to find the nearest unit")]
    public float m_findOtherUnitTime = 0.1f;

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
        // If the Enemy AI is not controlling it, dont update it!
        if (!EnemyAIManager.Instance.enabled)
            yield break;
        // Move the camera to this unit transform
        CameraMovement.Instance.MoveTowardsPosition(transform.position);
        // if it is attacking to begin with, then change to attack state
        if (isItAttacking)
        {
            m_FSMOwner.ChangeCurrentState("AttackState");
            yield break;
        }
        // Check for nearby enemy 1st!
        CharacterScript nearbyUnit = checkForUnitsInRange();
        if (nearbyUnit)
        {
            isItAttacking = true;
            m_FSMOwner.GetGenericState("AttackState").interactWithState(nearbyUnit);
            m_FSMOwner.ChangeCurrentState("AttackState");
            // Need to tell the Grid about the newly occupied grid!
            yield break;
        }


        if (moveToTile)
            // Request the movement from PathRequestManager
            PathRequestManager.RequestPath(transform.position, moveToTile.transform.position, OnPathFound);
        else
        {
            PathRequestManager.RequestPath(transform.position, moveTowardsPos, OnPathFound);
        }
        float timeCounter2nd = 0;
        // Wait till it has find the path!
        while (!hasFinishedPath)
        {
            m_timeCounter += Time.deltaTime;
            timeCounter2nd += Time.deltaTime;
            if (timeCounter2nd > m_findOtherUnitTime)
            {
                // Then find the nearest unit!
                float nearestDist = float.MaxValue;
                GameObject nearestUnit = null;
                // Have to hardcode to be like this!
                foreach (GameObject go in PlayerManager.Instance.m_playerGOList)
                {
                    if (Vector2.Distance(go.transform.position, transform.position) < nearestDist)
                    {
                        nearestDist = Vector2.Distance(go.transform.position, transform.position);
                        nearestUnit = go;
                    }
                }
                // If there is the nearest unit!
                if (nearestUnit)
                {
                    PathRequestManager.RequestPath(transform.position, nearestUnit.transform.position, OnPathFound);
                }
                // reset the time counter
                timeCounter2nd = 0;
            }
            else if (m_timeCounter > m_HowManySecond)
                yield break;
            yield return null;
        }
        int m_MoveCountDown = charStats.m_MoveSpeed, currentMoveIndex = 0;
        // Dont forget to include the original position
        m_originalPos = transform.position;
        // Then tell the camera to follow this unit!
        CameraMovement.Instance.StartFollowingTransfrom(transform);
        m_FSMOwner.m_animScript.setWalking(true);
        doWalkingDirectionAnim(m_wayptToFollow[currentMoveIndex]);
        // We will count down the movement speed and check to make sure the current index is not greater than the array
        while (m_MoveCountDown > 0 && currentMoveIndex < m_wayptToFollow.Length)
        {
            // If the unit has reached that point, then increment the index
            if (transform.position == m_wayptToFollow[currentMoveIndex])
            {
                ++currentMoveIndex;
                --m_MoveCountDown;
                nearbyUnit = checkForUnitsInRange();
                if (nearbyUnit)
                {
                    isItAttacking = true;
                    m_FSMOwner.GetGenericState("AttackState").interactWithState(nearbyUnit);
                    m_FSMOwner.ChangeCurrentState("AttackState");
                    // Need to tell the Grid about the newly occupied grid!
                    yield break;
                }
                else if (m_MoveCountDown > 0 && currentMoveIndex < m_wayptToFollow.Length)
                {
                    // Then keep moving!
                    doWalkingDirectionAnim(m_wayptToFollow[currentMoveIndex]);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, m_wayptToFollow[currentMoveIndex], m_animMoveSpeed);
                //LeanTween.move(gameObject, m_wayptToFollow[currentMoveIndex], m_animMoveSpeed);
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

    /// <summary>
    /// Check for any player units or fortress is nearby!
    /// </summary>
    /// <returns>Return any nearby units. If none, check whether the fortress is nearby. If not, return nothing</returns>
    protected CharacterScript checkForUnitsInRange()
    {
        foreach (GameObject playerUnitGO in PlayerManager.Instance.m_playerGOList)
        {
            if (charStats.m_Range >= Vector3.Distance(transform.position, playerUnitGO.transform.position))
            {
                return playerUnitGO.GetComponent<CharacterScript>();
            }
        }
        if (charStats.m_Range >= Vector3.Distance(transform.position, playerFortressTile.transform.position))
        {
            return playerFortressTile.GetComponent<CharacterScript>();
        }
        return null;
    }

    public override void resetState()
    {
        base.resetState();
        m_FSMOwner.m_animScript.setWalking(false);
        m_timeCounter = 0;
    }


}
