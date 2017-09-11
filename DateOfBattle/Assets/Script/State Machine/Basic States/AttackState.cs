using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be the attack state of the unit
/// </summary>
public class AttackState : GenericState{

    [Tooltip("To get the character stats. Linking is not required.")]
    public CharacterScript charStats;
    [Header("The debugging and linking values")]
    [Tooltip("The Tile to attack towards to")]
    public TileScript targetToTile;
    // Use this for initialization
    protected virtual void Start () {

        // Making sure the name is the same.
        stateName = "AttackState";
        charStats = GetComponent<CharacterScript>();
    }


    public override IEnumerator updateState()
    {

        if (!targetToTile)
            yield break;

        int m_Range = charStats.m_Range;
        bool isOnRange = false;
        isOnRange = m_Range >  Vector3.Distance(transform.position, targetToTile.transform.position);

        if (isOnRange)
        {
            //
            charStats.Damage(charStats.m_AttackDamage, charStats.m_AttackType);
            charStats.Attack();

            if(charStats.IsDead())
            {
                Debug.Log("it is die.");
            }
        }
        else
        {
            //

        }
        yield break;
    }

    public override void resetState()
    {
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
        targetToTile = argument as TileScript;
        // If the object is there, then the casting is successful so return true
        if (targetToTile)
        {
            return true;
        }
        return false;
    }
}
