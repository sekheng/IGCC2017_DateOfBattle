using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testUnitScript : MonoBehaviour {
    UnitTile theUnitProperties;
    Rigidbody2D rb2D;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    const float animSpeed = 5.0f;
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.UpArrow))
        {
            rb2D.MovePosition(rb2D.position + animSpeed * Time.deltaTime * Vector2.up);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.position += Time.deltaTime * 5.0f * Vector3.left;
            rb2D.MovePosition(rb2D.position + animSpeed * Time.deltaTime * Vector2.left);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb2D.MovePosition(rb2D.position + animSpeed * Time.deltaTime * Vector2.down);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb2D.MovePosition(rb2D.position + animSpeed * Time.deltaTime * Vector2.right);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        print("Collided with: " + other.gameObject.name);
    }
}
