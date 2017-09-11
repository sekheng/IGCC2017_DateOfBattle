using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is used to replace the attack state.
/// In the event if player clicked on enemy unit and attack it.
/// </summary>
public class MoveAttackState : MoveState {

	// Use this for initialization
	protected override void Start () {
        charStats = GetComponent<CharacterScript>();
        stateName = "MoveAttackState";
	}



}
