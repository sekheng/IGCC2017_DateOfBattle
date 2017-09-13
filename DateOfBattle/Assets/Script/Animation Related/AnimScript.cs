using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Meant to be a simper API class to be called by other scripts.
/// handles the animation.
/// </summary>
public class AnimScript : MonoBehaviour {
    [Header("Debugging References and Values. Dont touch it!")]
    [SerializeField, Tooltip("The direction when it is currently walking! so that it will not repeatedly call the API of the animator.")]
    protected int m_walkingDirection = 0;
    [SerializeField, Tooltip("To check whether is it curently walking or not")]
    protected bool m_isItWalking = false;
    [SerializeField, Tooltip("The animator of the unit. Linking is already done in codes!")]
    protected Animator m_AnimController;

    // To set the animation parameter
    public const string animParamIntDirectionName = "direction";
    public const string animParambWalkingName = "isWalk";

    // Use this for initialization
    void Start () {
        m_AnimController = GetComponent<Animator>();
    }
	
    /// <summary>
    /// To set the walk direction.
    /// </summary>
    /// <param name="whatDirection">0 means walking up. 1 means walking down. 2 means walking left. 3 means walking right.</param>
    public void setWalkDirection(int whatDirection)
    {
        if (m_walkingDirection != whatDirection)
        {
            m_walkingDirection = whatDirection;
            m_AnimController.SetInteger(animParamIntDirectionName, whatDirection);
        }
    }

    /// <summary>
    /// Set is it walking or not! The parameter is default to false.
    /// </summary>
    /// <param name="isWalking">If true, it will animate walking.</param>
    public void setWalking(bool isWalking = false)
    {
        if (m_isItWalking != isWalking)
        {
            m_isItWalking = isWalking;
            m_AnimController.SetBool(animParambWalkingName, isWalking);
        }
    }
}
