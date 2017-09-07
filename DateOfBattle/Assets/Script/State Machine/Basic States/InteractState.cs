using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The state to interact with unit.
/// </summary>
public class InteractState : GenericState {
    protected UnitTile unitTile;
    [Tooltip("the instruction to check which direction!")]
    public string theDirection;

	// Use this for initialization
	void Start () {
        // Setting up the name and set the component to be inactive
        stateName = "InteractState";
        unitTile = GetComponent<UnitTile>();
	}

    public override IEnumerator updateState()
    {
        UnitTile otherUnit;
        switch (theDirection)
        {
            case "up":
                otherUnit = unitTile.m_upTile.GetUnitTile();
                unitTile.interactWithTile(otherUnit);
                break;
            case "down":
                otherUnit = unitTile.m_downTile.GetUnitTile();
                unitTile.interactWithTile(otherUnit);
                break;
            case "left":
                otherUnit = unitTile.m_leftTile.GetUnitTile();
                unitTile.interactWithTile(otherUnit);
                break;
            case "right":
                otherUnit = unitTile.m_rightTile.GetUnitTile();
                unitTile.interactWithTile(otherUnit);
                break;
            default:
                break;
        }
        yield break;
    }

    public override void resetState()
    {
        theDirection = null;
    }

    public override bool interactWithState(object argument)
    {
        string strArg = argument as string;
        // if successful conversion! that will be the direction
        if (strArg != null)
        {
            theDirection = strArg;
        }
        return false;
    }
}
