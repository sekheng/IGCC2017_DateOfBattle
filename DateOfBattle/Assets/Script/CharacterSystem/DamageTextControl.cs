using System.Collections;
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
        if (damage < 0)
        {
            text.color = Color.green;
        }
        else
        {
            text.color = Color.red;
        }
        this.gameObject.transform.position = parent.transform.position;
        text.text = (Mathf.Abs(damage).ToString());
        // Just play the animation state name, at Base Layer, Starting from 0!
        animator.Play("Hopping", 0, 0);
        //animator.SetTrigger("Damage");
        // animation.Play();
    }
}
