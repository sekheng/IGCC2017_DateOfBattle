﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextControl : MonoBehaviour {
    public TextMesh text;
    private Animator animator;
    private MeshRenderer textMesh;
    // private Animation animation;
    // Use this for initialization
    void Start () {

        //animation = GetComponent<Animation>();
        text = GetComponent<TextMesh>();
        animator = GetComponent<Animator>();
        textMesh = GetComponent<MeshRenderer>();
        textMesh.sortingOrder = 4;
    }

    public void PrintDamageValue(int damage,GameObject parent)
    {
        this.gameObject.transform.position = parent.transform.position;
        text.text = (damage.ToString());
        animator.SetTrigger("Damage");
        // animation.Play();
    }
}