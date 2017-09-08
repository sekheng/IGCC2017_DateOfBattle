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
        yield return null;
        yield return null;
        unitStateMachine.GetGenericState("MoveState").interactWithState(targetTile);
        unitStateMachine.ChangeCurrentState("MoveState");
        yield break;
    }
}
