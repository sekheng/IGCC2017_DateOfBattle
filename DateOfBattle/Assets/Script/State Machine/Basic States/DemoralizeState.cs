using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In the event that the character got demoralized by too much.
/// Then it will do something against the player's action!
/// </summary>
public class DemoralizeState : GenericState {
    [Header("The values needed for this state")]
    [SerializeField, Tooltip("The tag name of the base that this unit needs to run to! This should be used by the plyer unit.")]
    protected string fortressTagName = "PlayerFortress";

    [Header("Debugging References")]
    [SerializeField, Tooltip("The character status. Linking will be done automatically.")]
    protected CharacterScript charStat;
    [SerializeField, Tooltip("The Fortress tile. So that the unit can retreat to!")]
    protected Transform fortressTileTransform;

    // Use this for initialization
    void Start () {
        // Do the linking of the tag!
        charStat = GetComponent<CharacterScript>();
        fortressTileTransform = GameObject.FindGameObjectWithTag(fortressTagName).transform;
        stateName = "DemoralizeState";
    }

    public override IEnumerator updateState()
    {
        // TODO: write the definition when the unit got demoralized.
        
        yield break;
    }

    public override void resetState()
    {
        
    }

    public override bool interactWithState(object argument)
    {
        if (charStat.m_Motivation >= 50)
        {
            return true;
        }
        else if (charStat.m_Motivation < 25)
        {
            // Set the damage to the maximum hp!
            charStat.m_AttackDamage = charStat.m_MaximumHp;
            // and suicide!
            charStat.Attack(charStat);
            if (charStat.IsDead())
            {
                Destroy(gameObject);
            }
            return false;
        }
        // No time to do complicated stuff
        //if (charStat.m_characterCharis == (CharacterScript.CHARACTER_CHARACTERISTIC)(argument))
        //{
        //    return true;
        //}
        return false;
    }
}
