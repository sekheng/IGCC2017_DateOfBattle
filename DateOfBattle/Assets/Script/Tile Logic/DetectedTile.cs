using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectedTile : MonoBehaviour {
    ///// <summary>
    ///// The dictionary / hashset / C++ map / lookup table to store the current gameobjects
    ///// </summary>
    //protected Dictionary<string, GameObject> m_dictNameGO = new Dictionary<string, GameObject>();
    [Tooltip("All of the tile it has collided with so far")]
    public List<TileScript> allColliderTile = new List<TileScript>();

    void OnTriggerEnter2D(Collider2D other)
    {
        allColliderTile.Add(other.gameObject.GetComponent<TileScript>());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        allColliderTile.Remove(other.gameObject.GetComponent<TileScript>());
    }
}
