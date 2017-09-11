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
        isOnRange = m_Range >  Vector3.Distance(transform.position, targetChara.myTile.transform.position);

        if (isOnRange)
        {
            //
            
            charStats.Attack(targetChara);

            if(charStats.IsDead())
            {
                //Debug.Log("it is die.");
                //TODO: for now, Destroy the game object of the Unit until there is a particle effect system to enhance the graphics
                Destroy(charStats.gameObject);
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
        targetChara = argument as CharacterScript;
        // If the object is there, then the casting is successful so return true
        if (targetChara)
        {
            return true;
        }
        return false;
    }
}
