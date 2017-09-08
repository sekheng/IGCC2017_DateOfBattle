﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To be used at selection of unit during battle
/// </summary>
public class PlayerBattleMouse : MonoBehaviour {
    [Header("Debugging References")]
    [SerializeField, Tooltip("This is to know which Tile the player has clicked on. Do not mess with it!")]
    protected TileScript playerClickedTile;

    // Update is called once per frame
    void Update () {
        // If player pressed the mouse button down during the 1 frame
        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse to world coordinate. so simper.
            Vector2 mouseWorldCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Use overlap point since there is no raycast2D from MouseWorld Coordinate to collider
            RaycastHit2D hit2D = Physics2D.Linecast(mouseWorldCoord, mouseWorldCoord);
            if (!hit2D)
            { 
                if (playerClickedTile && playerClickedTile.GetComponent<UnitFSM>().GetGenericState("MoveState").interactWithState(mouseWorldCoord))
                {
                    playerClickedTile.GetComponent<UnitFSM>().ChangeCurrentState("MoveState");
                    playerClickedTile = null;
                }
                return;
            }
            TileScript theSelectedTile = hit2D.collider.GetComponent<TileScript>();
            if (theSelectedTile)
            {
                // TODO: this need more changes to it!
                // If player clicked tile happens to belong to the player, then can move the tile!
                if (playerClickedTile && playerClickedTile.tag == "Player" && theSelectedTile != playerClickedTile && theSelectedTile.tag != "Unwalkable")
                {
                    // If the interaction is successful, then change the state
                    if (playerClickedTile.GetComponent<UnitFSM>().GetGenericState("MoveState").interactWithState(theSelectedTile))
                    {
                        playerClickedTile.GetComponent<UnitFSM>().ChangeCurrentState("MoveState");
                        playerClickedTile = null;
                    }
                }
                else
                {
                    playerClickedTile = theSelectedTile;
                    ObserverSystemScript.Instance.TriggerEvent("PlayerClickedUnit");
                }
            }
        }
	}

#if UNITY_EDITOR
    /// <summary>
    /// To visualize what the player has clicked upon!
    /// </summary>
    private void OnDrawGizmos()
    {
        if (playerClickedTile)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(playerClickedTile.transform.position, 1);
        }
    }
#endif
}