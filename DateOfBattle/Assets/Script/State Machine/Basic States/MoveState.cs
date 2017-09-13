using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be the move state of the unit
/// </summary>
public class MoveState : GenericState {
    [Header("The value needed for move state")]
    [Tooltip("Different from Character stat move speed. This name is misleading as the lower the value, the slower it run so will investigate more on it if have time.")]
    public float m_animMoveSpeed = 0.3f;

    [Header("The debugging and linking values")]
    [Tooltip("The Tile to move towards to")]
    public TileScript moveToTile;
    [Tooltip("The position to move towards to")]
    public Vector3 moveTowardsPos;
    [Tooltip("To get the character stats. Linking is not required.")]
    public CharacterScript charStats;
    [SerializeField, Tooltip("The flag to wait for the path request manager to come back")]
    protected bool hasFinishedPath = false;
    [SerializeField, Tooltip("The waypoints that the unit needs to follow")]
    protected Vector3[] m_wayptToFollow;
    [SerializeField, Tooltip("The original position of the unit before moving to another position!")]
    protected Vector3 m_originalPos;

	// Use this for initialization
	protected virtual void Start () {
        // Making sure the name is the same.
        stateName = "MoveState";
        charStats = GetComponent<CharacterScript>();
    }
	
    /// <summary>
    /// To update the movement to this point! 
    /// </summary>
	public override IEnumerator updateState() {
        CameraMovement.Instance.MoveTowardsPosition(transform.position);
        int m_MoveCountDown = charStats.m_leftOverMoveSpeed;
        if (m_MoveCountDown == 0)
            yield break;
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
        int currentMoveIndex = 0;
        m_originalPos = transform.position;
        CameraMovement.Instance.StartFollowingTransfrom(transform);
        m_FSMOwner.m_animScript.setWalking(true);
        doWalkingDirectionAnim(m_wayptToFollow[currentMoveIndex]);
        // We will count down the movement speed and check to make sure the current index is not greater than the array
        while (m_MoveCountDown > 0 && currentMoveIndex < m_wayptToFollow.Length)
        {
            // If the unit has reached that point, then increment the index
            if (transform.position == m_wayptToFollow[currentMoveIndex])
            {
                ++currentMoveIndex;
                --m_MoveCountDown;
                if (m_MoveCountDown > 0 && currentMoveIndex < m_wayptToFollow.Length)
                {
                    // Doing it here so that the calculation for direction will only be called once!
                    doWalkingDirectionAnim(m_wayptToFollow[currentMoveIndex]);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, m_wayptToFollow[currentMoveIndex], m_animMoveSpeed);
            }
            yield return null;
        }
        charStats.m_leftOverMoveSpeed = m_MoveCountDown;
        CameraMovement.Instance.StopCamUpdateMovement();
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

    protected virtual void OnDisable()
    {
        // Need to stop the coroutine
        StopCoroutine("updateState");
        // Need to make sure moveToTile is null!
        moveToTile = null;
        hasFinishedPath = false;
        reportToGridNewPos();
        m_FSMOwner.m_animScript.setWalking();
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

    /// <summary>
    /// Meant to report back to the grid.
    /// Purpose is to make the original position to become walkable then the new position unwalkable!
    /// </summary>
    protected void reportToGridNewPos()
    {
        Node originalNode = Grid.Instance.NodeFromWorldPt(m_originalPos);
        originalNode.walkable = true;
        Node newNode = Grid.Instance.NodeFromWorldPt(transform.position);
        newNode.walkable = false;
    }

    /// <summary>
    /// The function that does the walking animation based on the direction!
    /// </summary>
    protected void doWalkingDirectionAnim(Vector3 otherPos)
    {
        Vector2 directionInWorldSpace2D = otherPos - transform.position;
        // If it is not going up or down!
        if (Mathf.Approximately(0, directionInWorldSpace2D.y))
        {
            // This means the unit is walking horizontally!
            if (directionInWorldSpace2D.x > 0)
            {
                // Then the unit is going to right!
                m_FSMOwner.m_animScript.setWalkDirection(3);
            }
            else
            {
                // The unit is going left
                m_FSMOwner.m_animScript.setWalkDirection(2);
            }
        }
        else
        {
            // The unit is walking vertically!
            if (directionInWorldSpace2D.y > 0)
            {
                // The unit is walking up!
                m_FSMOwner.m_animScript.setWalkDirection(0);
            }
            else
            {
                m_FSMOwner.m_animScript.setWalkDirection(1);
            }
        }
    }
}
