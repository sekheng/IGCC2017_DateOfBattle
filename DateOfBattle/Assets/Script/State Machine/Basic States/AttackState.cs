using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be the attack state of the unit
/// </summary>
public class AttackState : GenericState{
    [Header("Values and References required for Attack State")]
    [Tooltip("To delay the update by some time so the camera will stay a bit longer when unit is attacking each other")]
    public float m_DelayUpdateForCam = 0.5f;

    [Header("The debugging and linking values")]
    [Tooltip("To get the character stats. Linking is not required.")]
    public CharacterScript charStats;
    [Tooltip("The chara to attack towards to")]
    public CharacterScript targetChara;
    // Use this for initialization
    protected virtual void Start () {

        // Making sure the name is the same.
        stateName = "AttackState";
        charStats = GetComponent<CharacterScript>();
    }


    public override IEnumerator updateState()
    {

        if (!targetChara)
            yield break;

        int m_Range = charStats.m_Range;
        bool isOnRange = false;
        Vector3 directionFromPosToTarget = targetChara.transform.position - transform.position;

        //isOnRange = m_Range >  Vector3.Distance(transform.position, targetChara.transform.position);
        isOnRange = m_Range > directionFromPosToTarget.magnitude;
        if (isOnRange)
        {
            CamFocusOnUnitTarget(directionFromPosToTarget);
            // set direction and animate!
            doAnimationBasedOnDirection(directionFromPosToTarget);
            m_FSMOwner.m_animScript.setAttacking(true);
            charStats.Attack(targetChara);

            if(targetChara.IsDead())
            {
                //Debug.Log("it is die.");
                //TODO: for now, Destroy the game object of the Unit until there is a particle effect system to enhance the graphics
                Destroy(targetChara.gameObject);
            }
            yield return new WaitForSeconds(m_DelayUpdateForCam);
        }
        else
        {
            //

        }
        yield break;
    }

    public override void resetState()
    {
        m_FSMOwner.m_animScript.setAttacking(false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(charStats)
        {
            Gizmos.DrawWireSphere(transform.position, charStats.m_Range);
        }
    }
    /// <summary>
    /// To get the tile to move script
    /// </summary>
    /// <param name="argument">It should be the tile it needs to move to!</param>
    /// <returns>return true if casting is successful</returns>
    public override bool interactWithState(object argument)
    {
        targetChara = argument as CharacterScript;
        // If the object is there, then the casting is successful so return true
        if (targetChara)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Set the animation direction based on where it is attacking!
    /// </summary>
    /// <param name="directionFacing">The direction it is facing!</param>
    protected void doAnimationBasedOnDirection(Vector3 directionFacing)
    {
        // Need to normalize it otherwise the direction it is facing will be incorrect!
        directionFacing.Normalize();
        // Need to compare in terms of absolute otherwise it will be difficult to calculate!
        float absX = Mathf.Abs(directionFacing.x);
        float absY = Mathf.Abs(directionFacing.y);
        if (absX >= absY)
        {
            // Which means the unit is more likely to face horizontally to the target
            if (directionFacing.x > 0)
            {
                // It is facing right!!
                m_FSMOwner.m_animScript.setWalkDirection(3);
            }
            else
            {
                // It is facing left
                m_FSMOwner.m_animScript.setWalkDirection(2);
            }
        }
        else
        {
            // The unit is facing vertically to the target!
            if (directionFacing.y > 0)
            {
                // it is facing up!
                m_FSMOwner.m_animScript.setWalkDirection(0);
            }
            else
            {
                // It is facing down
                m_FSMOwner.m_animScript.setWalkDirection(1);
            }
        }
    }

    protected void CamFocusOnUnitTarget(Vector3 directionToTarget)
    {
        // We just need the direction to the target!
        CameraMovement.Instance.MoveTowardsPosition(transform.position + directionToTarget * 0.5f);
    }
}
