using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The basic state machine of the units!
/// </summary>
public class UnitFSM : MonoBehaviour {
    [SerializeField, Tooltip("This is for all the states that the unit has!")]
    protected GenericState[] m_AllGenericStates;
    [Tooltip("The current state that this FSM is in!")]
    public GenericState currentState;
    // To keep track of the update state
    protected Coroutine updateStateCoroutine;

	// Use this for initialization
	void Awake () {
        m_AllGenericStates = GetComponentsInChildren<GenericState>();
        // Then loop through all the state and make sure the states can recognise their owner
        foreach (GenericState state in m_AllGenericStates)
            state.m_FSMOwner = this;
    }

    public bool ChangeCurrentState(string stateName)
    {
        if (currentState == null || currentState.stateName != stateName)
        {
            // Need to stop the update of the state!
            if (updateStateCoroutine != null)
            {
                StopCoroutine(updateStateCoroutine);
            }
            // the current state is still active, make it inactive and reset current state
            if (currentState != null)
            {
                currentState.resetState();
            }
            // We loop it because no time to code faster search for states
            foreach (GenericState state in m_AllGenericStates)
            {
                if (state.stateName == stateName)
                {
                    // then set the state and begin the update!
                    state.enabled = true;
                    currentState = state;
                    updateStateCoroutine = StartCoroutine(UpdatingStates());
                    break;
                }
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// The coroutine to update the state update!
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdatingStates()
    {
        Coroutine stateCoroutine = StartCoroutine(currentState.updateState());
        yield return stateCoroutine;
        // reset the value once the update is done!
        if (currentState)
        {
            currentState.resetState();
            currentState = null;
        }
        yield break;
    }

    public GenericState GetGenericState(string stateName)
    {
        foreach (GenericState state in m_AllGenericStates)
        {
            if (state.stateName == stateName)
            {
                return state;
            }
        }
        return null;
    }
}
