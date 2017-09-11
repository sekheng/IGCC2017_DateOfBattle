﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : AttackState {
    public override IEnumerator updateState()
    {
        // If the unit is in range, then attack!
        if (charStats.m_Range >= Vector3.Distance(transform.position, targetChara.transform.position))
        {
            charStats.Attack(targetChara);
            if (charStats.IsDead())
            {
                Destroy(charStats.gameObject);
                // Tell the move state that we have defeated the unit!
                m_FSMOwner.GetGenericState("MoveState").interactWithState("DEFEAT_UNIT");
            }
        }
        else
        {
            // We have to inform the previous state the target tile is out of range
            targetChara = null;
            // Change back to move state and start moving!
            m_FSMOwner.GetGenericState("MoveState").interactWithState("OUT_OF_RANGE");
            m_FSMOwner.ChangeCurrentState("MoveState");
        }
        yield break;
    }
}