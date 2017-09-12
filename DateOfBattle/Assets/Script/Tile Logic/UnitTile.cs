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

    /// <summary>
    /// When it dies, trigger the "died" event at Observer System!
    /// For Player Unit, it should trigger "PlayerUnitDied".
    /// For Enemy Unit, it should trigger "EnemyUnitDied".
    /// </summary>
    private void OnDestroy()
    {
        // Need to report back to Grid.cs that the current node that the unit is occupying has been destroyed!
        Grid.Instance.NodeFromWorldPt(transform.position).walkable = true;
        // Trigger the event at ObserverSystem
        ObserverSystemScript.Instance.storeVariableInEvent(tag + "UnitDied", gameObject);
        // The tag will be doing it for us to check whether is it the player or enemy unit!
        ObserverSystemScript.Instance.TriggerEvent(tag + "UnitDied");
    }
}
