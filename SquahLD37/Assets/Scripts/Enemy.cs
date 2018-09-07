using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public int health = 1;
    public int pointsOnKill = 1;
	public Animator animator;
	public Rigidbody2D rb2d;

	public EnemyMovement movement;

	public FSMSystem fsm;

	public bool isDead;

    private float invulnTime = .1f;
    private float lastHitTime = 0f;

	// Use this for initialization
	void Awake () {
		MakeFSM();
	}
	
	// Update is called once per frame
	public virtual void Update ()
	{
		if (isDead)
		{
			rb2d.velocity *= 0.9f;
			return;
		}
		if (movement)
			movement.DoStep();
	}

	public virtual void Damage(int damage = 1)
	{
	    if (Time.time - lastHitTime > invulnTime)
	    {
	        health -= damage;
	        if (health <= 0)
	        {
	            Die();
	        }
	        lastHitTime = Time.time;
	    }
	}

	public virtual void Die()
	{
        //Destroy(gameObject);
        Player.instance.score.GiveScore(pointsOnKill);
        animator.Play("death");
		isDead = true;
		gameObject.layer = LayerMask.NameToLayer("NonInteractive");
        Player.instance.combo.upCombo();

		GetComponent<VelocityFlip>().enabled = false;
		Destroy(rb2d);
		Destroy(GetComponent<Collider2D>());
		Destroy(gameObject, 4.0f);

		var childColliders = GetComponentsInChildren<Collider2D>();
		foreach (var childCollider in childColliders) {
			Destroy(childCollider);
		}

		enabled = false;
	}

	public virtual void MakeFSM()
	{
		//Debug.LogError("Your class does not implement the MakeFSM function! It won't do anything now.");
		//http://wiki.unity3d.com/index.php?title=Finite_State_Machine
	}

	public void SetTransition(Transition t) { fsm.PerformTransition(t); }
}
