using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTile : TileScript
{
    [Header("Unit references")]
    [Tooltip("The trigger that is above this unt")]
    public DetectedTile m_upTile;
    [Tooltip("The trigger that is below this unit")]
    public DetectedTile m_downTile;
    [Tooltip("The trigger that is left of this unit")]
    public DetectedTile m_leftTile;
    [Tooltip("The trigger that is right of this unit")]
    public DetectedTile m_rightTile;

    public bool interactWithTile(ITileInteract other)
    {
        // When interacting with other tile!
        if (other is UnitTile)
        {
            // TODO: Do something!
            print("Peace on you: " + ((other as UnitTile).name));
        }
        return false;
    }

    private void Start()
    {
        // Because unit has occupied the current space!
        canBeAccessed = false;
    }

    public void MoveUp()
    {
        
    }

    public void MoveDown()
    {

    }

    public void MoveLeft()
    {

    }

    public void MoveRight()
    {

    }
}
