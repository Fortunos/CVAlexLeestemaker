using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	public static PlayerAnimation instance;

	public Rigidbody2D rb2d;
	public float minVelocityForRunAnimation;
	private bool isMoving;

	public Animator animator;

	public float globalCooldown;
	public List<Ability> abilityCooldowns;

	public void Start()
	{
		instance = this;
	}

	public bool UseAbility(string name)
	{
		if (globalCooldown > 0)
			return false;
		PlayAnimation(name);
		return true;
	}

	private void PlayAnimation(string anim)
	{
		animator.Play(anim);
		globalCooldown = abilityCooldowns.Find(a => a.name == anim).cooldown;
	}

	public void Update()
	{
		globalCooldown -= Time.deltaTime;
		bool _isMoving = Mathf.Abs(rb2d.velocity.magnitude) > minVelocityForRunAnimation;
		animator.SetBool("isMoving", _isMoving);
		if (isMoving != _isMoving)
		{
			isMoving = _isMoving;
			if (isMoving)
			{
				animator.Play("walk");
			}
			else
			{
				animator.Play("idle");
			}
		}
	}

	[Serializable]
	public struct Ability
	{
		public string name;
		public float cooldown;
	}
}
