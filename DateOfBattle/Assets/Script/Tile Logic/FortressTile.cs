using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script meant for the fortress!
/// </summary>
public class FortressTile : TileScript {
    /// <summary>
    /// It means the script is destroyed! If so, trigger the fortressdestroyed event!
    /// For Player fortress it is "PlayerFortressLost".
    /// For Enemy fortress it is "EnemyFortressLost".
    /// </summary>
    private void OnDestroy()
    {
        // There is no need to check whether this is an enemy or not since the gameobject tag will be doing it for us!
        ObserverSystemScript.Instance.TriggerEvent(tag + "Lost");
    }
}
