using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityFlip : MonoBehaviour
{
	public Rigidbody2D rb2d;
	public bool X, Y;
	public float minVelocityForFlip = 0.1f;

	private float originalScaleX;
	private float originalScaleY;


	// Use this for initialization
	void Start ()
	{
		originalScaleX = transform.localScale.x;
		originalScaleY = transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 scale = transform.localScale;
		Vector3 velocity = rb2d.velocity;
		if (X)
		{
			float signX = Mathf.Sign(velocity.x);
			transform.localScale = new Vector3(originalScaleX*signX, scale.y, scale.z);
		}
		if (Y)
		{
			float signY = Mathf.Sign(velocity.y);
			transform.localScale = new Vector3(scale.x, originalScaleY*signY, scale.z);
		}
	}
}
