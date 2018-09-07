using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Fjördtunos made this
    public Rigidbody2D rb2d;
    public float speed;


    void FixedUpdate()
    {
        //float xInput = normalizeInput(Input.GetAxis("Horizontal"));
        //float yInput = normalizeInput(Input.GetAxis("Vertical"));

	    float xInput = Input.GetAxis("Horizontal");
	    float yInput = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(xInput, yInput);
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        rb2d.velocity = direction * speed;
    }
}