using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera movement during the battle!
/// </summary>
public class CameraMovement : MonoBehaviour {
    [Header("Values for the variables")]
    [Tooltip("The time to animate the camera movement from 1 point to another")]
    public float m_AnimMovement = 0.3f;
    [Tooltip("The free cam movement speed. Different from AnimMovement")]
    public float m_FreeCamMoveSpeed = 1.5f;
    
    [Header("Debugging References")]
    [SerializeField, Tooltip("The flag to indicate whether it has finished animating movement")]
    protected bool m_hasFinishedMoving = true;
    /// <summary>
    /// Just a getter to only access whether this camera has finished moving
    /// </summary>
    public bool hasFinishedMoving
    {
        get
        {
            return m_hasFinishedMoving;
        }
    }
    /// <summary>
    /// The coroutine update of camera free movement
    /// </summary>
    public Coroutine m_updateMovementCoroutine;

    // The singleton for camera movement
    public static CameraMovement Instance
    {
        get; private set;
    }

    /// <summary>
    /// Setting up the singleton
    /// </summary>
    private void Awake()
    {
        if (Instance)
            Destroy(this);
        else
            Instance = this;
    }

    /// <summary>
    /// Move towards that position of the camera!
    /// </summary>
    /// <param name="pointPos"></param>
    public void MoveTowardsPosition(Vector2 pointPos)
    {
        if (m_hasFinishedMoving)
        {
            // Stop free movement
            StopCamUpdateMovement();
            // using LeanTween to be lazy in coding!
            LeanTween.move(gameObject, pointPos, m_AnimMovement).setOnComplete(MovementAnimComplete);
            m_hasFinishedMoving = false;
        }
    }

    /// <summary>
    /// LeanTween will call this event when it is done!
    /// </summary>
    protected void MovementAnimComplete()
    {
        m_hasFinishedMoving = true;
    }

    /// <summary>
    /// The API to begin the free movement of the camera!
    /// </summary>
    public void BeginCamFreeMovement()
    {
        StopCamUpdateMovement();
        m_updateMovementCoroutine = StartCoroutine(FreeMovementUpdate());
    }

    /// <summary>
    /// The API to stop the free movement of camera and other camera coroutines!
    /// </summary>
    public void StopCamUpdateMovement()
    {
        // If the coroutine exists, stop the coroutine
        if (m_updateMovementCoroutine != null)
        {
            StopCoroutine(m_updateMovementCoroutine);
            m_updateMovementCoroutine = null;
        }
    }

    /// <summary>
    /// The update of movement of the camera
    /// </summary>
    /// <returns></returns>
    protected IEnumerator FreeMovementUpdate()
    {
        while (true)
        {
            #region MOUSE_CONTROL
            if (Input.GetMouseButton(1))
            {
                // Too lazy to optimize so this will do. lol!
                transform.position += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * m_FreeCamMoveSpeed;
            }
            #endregion
            yield return null;
        }
    }

    protected IEnumerator FollowTransformPos(Transform objectTransfrom)
    {
        WaitForEndOfFrame endOfFrameRequest = new WaitForEndOfFrame();
        while (true)
        {
            // Wait till the end of frame similar to LateUpdate but it is right after all of the rendering!
            yield return endOfFrameRequest;
            Vector3 objTransfromToCam = new Vector3(objectTransfrom.position.x, objectTransfrom.position.y, transform.position.z);
            // Then we will just do lerping to make it seems smooth!
            transform.position = Vector3.Lerp(transform.position, objTransfromToCam, 0.2f);
            // Then wait till next frame!
            yield return null;
        }
    }

    public void StartFollowingTransfrom(Transform objectTransfrom)
    {
        StopCamUpdateMovement();
        m_updateMovementCoroutine = StartCoroutine(FollowTransformPos(objectTransfrom));
    }
}
