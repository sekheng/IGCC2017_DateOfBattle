using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Meant to test transitioning the hardcoded way
/// </summary>
public class testStateChange : MonoBehaviour {
    public UnitFSM unitStateMachine;

    public TileScript targetTile;

	// Use this for initialization
	void Start () {
        StartCoroutine(changeStateAfterStart());
	}
	
    IEnumerator changeStateAfterStart()
    {
        // Wait for 2 frames then start to change state because the state machine and the states has yet to be initialised!
        yield return null;
        yield return null;
        unitStateMachine.GetGenericState("MoveState").interactWithState(targetTile);
        unitStateMachine.ChangeCurrentState("MoveState");
        yield break;
    }
}
