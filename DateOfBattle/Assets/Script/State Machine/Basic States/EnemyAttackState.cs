using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : AttackState {
    public override IEnumerator updateState()
    {
        if (charStats.m_Range >= Vector3.Distance(transform.position, targetToTile.transform.position))
        {

        }
        else
        {
            // We have to inform the previous state the target tile is out of range
            targetToTile = null;
            m_FSMOwner.GetGenericState("MoveState").interactWithState("OUT_OF_RANGE");
            m_FSMOwner.ChangeCurrentState("MoveState");
        }
        yield break;
    }
}
