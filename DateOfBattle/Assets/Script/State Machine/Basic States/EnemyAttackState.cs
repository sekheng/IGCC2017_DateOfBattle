using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : AttackState {
    [Header("Values for EnemyAttack state!")]
    [Tooltip("Wait for a bit of delay as the animation transition cannot catch up!")]
    public float m_attackAnimDelay = 0.5f;

    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator updateState()
    {
        Vector3 directionFromPositionToTarget = targetChara.transform.position - transform.position;
        // If the unit is in range, then attack!
        if (targetChara && charStats.m_Range >= directionFromPositionToTarget.magnitude)
        {
            // The quickest way to fix the moving then attack animation bug!
            // Without this, the unit after changing from "EnemyMove" to this state. It will not attempt any animation as it transits too fast.
            //yield return null;
            //yield return null;
            //yield return null;
            yield return new WaitForSeconds(m_attackAnimDelay);
            doAnimationBasedOnDirection(directionFromPositionToTarget);
            m_FSMOwner.m_animScript.setAttacking(true);
            // Dont need to reset attack animation as AttackState is resetting it for us!
            charStats.Attack(targetChara);
            if (targetChara.IsDead())
            {
                Destroy(targetChara.gameObject);
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
        yield return null;
        // For some reason, it sometimes refuse to reset state!
        resetState();
        yield break;
    }

    /// <summary>
    /// In order to reset the state!
    /// </summary>
    public override void resetState()
    {
        base.resetState();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (charStats)
        {
            Gizmos.DrawWireSphere(transform.position, charStats.m_Range);
        }
    }
#endif
}
