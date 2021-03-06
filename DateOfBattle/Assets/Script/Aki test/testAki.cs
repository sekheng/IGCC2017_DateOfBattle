﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Meant to test transitioning the hardcoded way
/// </summary>
public class testAki : MonoBehaviour
{
    public UnitFSM unitStateMachine;
    
    public CharacterScript targetChara;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(changeStateAfterStart());
    }

    IEnumerator changeStateAfterStart()
    {
        yield return null;
        yield return null;
        unitStateMachine.GetGenericState("AttackState").interactWithState(targetChara);
        unitStateMachine.ChangeCurrentState("AttackState");
        yield break;
    }
}
