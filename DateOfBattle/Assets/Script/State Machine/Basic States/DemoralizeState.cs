﻿using System.Collections;
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
}
