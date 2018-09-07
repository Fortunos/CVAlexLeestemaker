using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampProjectile : MonoBehaviour
{
	private Vector3 direction;
	private float speed;
	private float distance;

	private Vector3 startPosition;

	public Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ((gameObject.transform.position - startPosition).magnitude > distance)
			Destroy(gameObject);
	}

	public void Initialize(Vector3 _direction, float _speed, float _distance)
	{
		direction = _direction;
		speed = _speed;
		distance = _distance;

		startPosition = gameObject.transform.position;
		rb2d.velocity = direction.normalized*speed;
		float angle = Mathf.Atan2(direction.y, direction.x) * 180f / Mathf.PI;
		transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		string tag = col.gameObject.tag;
		if (tag == "Wall")
			Destroy(gameObject);
		if (tag == "Player")
			Destroy(gameObject);
	}
}
