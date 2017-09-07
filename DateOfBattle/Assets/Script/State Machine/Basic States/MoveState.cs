using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be the move state of the unit
/// </summary>
public class MoveState : GenericState {
    [Tooltip("The Tile to move towards to")]
    public TileScript moveToTile;

	// Use this for initialization
	void Start () {
        // Making sure the name is the same.
        stateName = "MoveState";
	}
	
    /// <summary>
    /// To update the movement to this point! 
    /// </summary>
	public override IEnumerator updateState() {
        //TODO: Use A* Search!!! 
        //TODO: Remove the line below
        m_FSMOwner.GetGenericState("InteractState").interactWithState("down");
        m_FSMOwner.ChangeCurrentState("InteractState");

        while (moveToTile)
        {
            yield return null;
        }
        yield break;
	}

    private void OnDisable()
    {
        // Need to make sure moveToTile is null!
        moveToTile = null;
    }

    public override void resetState()
    {
        moveToTile = null;
    }


}
