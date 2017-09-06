using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileInteract
{
    bool interactWithTile(ITileInteract other);
}

/// <summary>
/// Containing the tile information
/// </summary>
public class TileScript : MonoBehaviour, ITileInteract
{
    public bool canBeAccessed = true;

    public bool interactWithTile(ITileInteract other)
    {
        // means nothing happen so return true
        return true;
    }
}
