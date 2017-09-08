using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be the move state of the unit
/// </summary>
public class MoveState : GenericState {
    [Header("The value needed for move state")]
    [Tooltip("The animating movement speed. Different from Character stat move speed.")]
    public float m_animMoveSpeed = 5.0f;

    [Header("The debugging and linking values")]
    [Tooltip("The Tile to move towards to")]
    public TileScript moveToTile;
    [Tooltip("The position to move towards to")]
    public Vector3 moveTowardsPos;
    [Tooltip("To get the character stats. Linking is not required.")]
    public CharacterScript charStats;
    [SerializeField, Tooltip("The flag to wait for the path request manager to come back")]
    bool hasFinishedPath = false;
    [SerializeField, Tooltip("The waypoints that the unit needs to follow")]
    Vector3[] m_wayptToFollow;

	// Use this for initialization
	void Start () {
        // Making sure the name is the same.
        stateName = "MoveState";
        charStats = GetComponent<CharacterScript>();
    }
	
    /// <summary>
    /// To update the movement to this point! 
    /// </summary>
	public override IEnumerator updateState() {
        if (moveToTile)
        // Request the movement from PathRequestManager
            PathRequestManager.RequestPath(transform.position, moveToTile.transform.position, OnPathFound);
        else
        {
            PathRequestManager.RequestPath(transform.position, moveTowardsPos, OnPathFound);
        }
        // Wait till it has find the path!
        while (!hasFinishedPath)
            yield return null;
        int m_MoveCountDown = charStats.m_MoveSpeed, currentMoveIndex = 0;
        // We will count down the movement speed and check to make sure the current index is not greater than the array
        while (m_MoveCountDown > 0 && currentMoveIndex < m_wayptToFollow.Length)
        {
            // If the unit has reached that point, then increment the index
            if (transform.position == m_wayptToFollow[currentMoveIndex])
            {
                ++currentMoveIndex;
                --m_MoveCountDown;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, m_wayptToFollow[currentMoveIndex], m_animMoveSpeed);
            }
            yield return null;
        }
        yield break;
	}

    protected void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        // If cannot find any path, then stop the current coroutine
        if (!pathSuccessful)
            StopCoroutine("updateState");
        else
        {
            m_wayptToFollow = newPath;
            hasFinishedPath = true;
        }
    }

    private void OnDisable()
    {
        // Need to stop the coroutine
        StopCoroutine("updateState");
        // Need to make sure moveToTile is null!
        moveToTile = null;
        hasFinishedPath = false;
    }

    public override void resetState()
    {
        // Call OnDisable since it has the function to reset it anyway
        OnDisable();
    }

    /// <summary>
    /// To get the tile to move script
    /// </summary>
    /// <param name="argument">It should be the tile it needs to move to!</param>
    /// <returns>return true if casting is successful</returns>
    public override bool interactWithState(object argument)
    {
        // If it is just position
        if (argument is Vector2)
        {
            Vector2 movePos = (Vector2)argument;
            // So that the z-position will remain
            moveTowardsPos = new Vector3(movePos.x, movePos.y, transform.position.z);
            moveToTile = null;
            return true;
        }
        moveToTile = argument as TileScript;
        // If the object is there, then the casting is successful so return true
        if (moveToTile)
        {
            return true;
        }
        return false;
    }
}
