using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The generic state so that no instantiating will be allowed!
/// </summary>
public abstract class GenericState : MonoBehaviour {
    [Header("The Generic State References. They should already be assigned in codes!")]
    [Tooltip("The state name for identification! No need to put anything inside as it should be done at the State Start")]
    public string stateName;
    [Tooltip("The FSM owner for callback purpose. There is no need to link as UnitFSM Start is doing it!")]
    public UnitFSM m_FSMOwner;

    /// <summary>
    /// Every state needs to implement this.
    /// No need for update as UnitFSM.cs will be the only one to update it!
    /// The use of coroutine so that the update can be stopped anytime.
    /// </summary>
    public abstract IEnumerator updateState();
    /// <summary>
    /// All state needs to have the ability to reset state!
    /// </summary>
    public abstract void resetState();

    /// <summary>
    /// For interacting with other states!
    /// </summary>
    /// <param name="argument"></param>
    /// <returns>Return false since there is nothing to interact with a generic state!</returns>
    public virtual bool interactWithState(object argument)
    {
        return false;
    }
}
