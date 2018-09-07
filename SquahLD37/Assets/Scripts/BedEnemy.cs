using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedEnemy : Enemy
{
	public float prepareRunAnimationTime;
	public float walkSpeed;
	public float runSpeed;
	public float stunTime;
	public float approximateThreshold;

	private TargetPlayerState target;
	private AnimateState animate;
	private RunState charge;

	public bool hasHitWall;

	public override void Update ()
	{
		base.Update();
		if (isDead)
			return;

		target.Update(Time.deltaTime);
		animate.Update(Time.deltaTime);
		charge.Update(Time.deltaTime);
		
		fsm.CurrentState.Reason(Player.instance.gameObject, gameObject);
		fsm.CurrentState.Act(Player.instance.gameObject, gameObject);
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Wall")
			hasHitWall = true;
	}

	public override void MakeFSM()
	{
		target = new TargetPlayerState(approximateThreshold, walkSpeed, gameObject);
		target.AddTransition(Transition.AnimateTransition, StateID.Charge);

		animate = new AnimateState(prepareRunAnimationTime, Vector3.zero, gameObject);
		animate.AddTransition(Transition.ChargeTransition, StateID.Walk);

		charge = new RunState(Vector3.zero, runSpeed, stunTime, gameObject);
		charge.AddTransition(Transition.WalkTransition, StateID.Animate);
			

		fsm = new FSMSystem();
		fsm.AddState(target, StateID.Animate);
		fsm.AddState(animate, StateID.Charge);
		fsm.AddState(charge, StateID.Walk);
	}
	
	private class TargetPlayerState : FSMState
	{
		private float approximateThreshold;
		private float walkSpeed;

		private GameObject go;

		public TargetPlayerState(float _approximateThreshold, float _walkSpeed, GameObject _go)
		{
			go = _go;
			Restart(_approximateThreshold, _walkSpeed);
		}

		public override void Reason(GameObject player, GameObject npc)
		{
			Vector3 offset = npc.transform.position - player.transform.position;
			if (Mathf.Abs(offset.x) < approximateThreshold)
			{
				Vector3 direction = offset.y > 0 ? Vector3.down : Vector3.up;
				BedEnemy enemy = npc.GetComponent<BedEnemy>();
				enemy.animate.Restart(enemy.prepareRunAnimationTime, direction);
				enemy.SetTransition(Transition.AnimateTransition);
			}
			if (Mathf.Abs(offset.y) < approximateThreshold)
			{
				Vector3 direction = offset.x > 0 ? Vector3.left : Vector3.right;
				BedEnemy enemy = npc.GetComponent<BedEnemy>();
				enemy.animate.Restart(enemy.prepareRunAnimationTime, direction);
				enemy.SetTransition(Transition.AnimateTransition);
			}
		}

		public override void Act(GameObject player, GameObject npc)
		{
			if (npc.GetComponent<Enemy>().isDead)
				return;
			BedEnemy enemy = npc.GetComponent<BedEnemy>();
			enemy.hasHitWall = false;

			Vector3 offset = npc.transform.position - player.transform.position;
			Vector3 direction = Vector3.zero;
			if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
			{
				direction = offset.y > 0 ? Vector3.down : Vector3.up;
			}
			else
			{
				direction = offset.x > 0 ? Vector3.left : Vector3.right;
			}

			Rigidbody2D rb2d = npc.GetComponent<Rigidbody2D>();
			rb2d.velocity = direction * walkSpeed;

		}

		public void Update(float timeElapsed)
		{
			//Nothing
		}

		public void Restart(float _approximateThreshold, float _walkSpeed)
		{
			if (go.GetComponent<Enemy>().isDead)
				return;

			approximateThreshold = _approximateThreshold;
			walkSpeed =_walkSpeed;

			go.GetComponent<BedEnemy>().animator.Play("walk");
		}
	}
	
	private class AnimateState : FSMState
	{
		private float animateTime;
		private float timeUntilRun;
		private Vector3 direction;

		private GameObject go;

		public AnimateState(float _animateTime, Vector3 _direction, GameObject _go)
		{
			go = _go;
			Restart(_animateTime, _direction);
		}

		public override void Reason(GameObject player, GameObject npc)
		{
			if (timeUntilRun <= 0)
			{
				BedEnemy enemy = npc.GetComponent<BedEnemy>();
				enemy.charge.Restart(direction, enemy.runSpeed, enemy.stunTime);
				enemy.SetTransition(Transition.ChargeTransition);
			}
		}

		public override void Act(GameObject player, GameObject npc)
		{
			if (npc.GetComponent<Enemy>().isDead)
				return;
			BedEnemy enemy = npc.GetComponent<BedEnemy>();
			enemy.hasHitWall = false;

			Rigidbody2D rb2d = npc.GetComponent<Rigidbody2D>();
			rb2d.velocity *= 0.5f;
		}

		public void Update(float timeElapsed)
		{
			timeUntilRun -= timeElapsed;
		}

		public void Restart(float _animateTime, Vector3 _direction)
		{
			if (go.GetComponent<Enemy>().isDead)
				return;

			animateTime = _animateTime;
			timeUntilRun = animateTime;
			direction = _direction;
			
			go.GetComponent<BedEnemy>().animator.Play("preCharge");
		}
	}

	private class RunState : FSMState
	{
		private Vector3 direction;
		private float speed;
		private float stunTime;
		private float timeStunnedRemaining;
		private bool hasHitWall;
		//private bool playedStunned;

		private GameObject go;

		public RunState(Vector3 _direction, float _speed, float _stunTime, GameObject _go)
		{
			go = _go;
			Restart(_direction, _speed, _stunTime);
		}

		public override void Reason(GameObject player, GameObject npc)
		{
			BedEnemy enemy = npc.GetComponent<BedEnemy>();
			if (hasHitWall && timeStunnedRemaining <= 0)
			{
				enemy.target.Restart(enemy.approximateThreshold, enemy.walkSpeed);
				enemy.SetTransition(Transition.WalkTransition);
			}
		}

		public override void Act(GameObject player, GameObject npc)
		{
			if (npc.GetComponent<Enemy>().isDead)
				return;

			Rigidbody2D rb2d = npc.GetComponent<Rigidbody2D>();
			BedEnemy enemy = npc.GetComponent<BedEnemy>();

			if (enemy.hasHitWall && !hasHitWall)
			{
				hasHitWall = true;
				go.GetComponent<BedEnemy>().animator.Play("stunned");
				timeStunnedRemaining = stunTime;
			}
			if (hasHitWall)
			{
				rb2d.velocity *= 0.5f;
			}
			else
			{
				rb2d.velocity = direction * speed;
			}
		}

		public void Update(float timeElapsed)
		{
			timeStunnedRemaining -= timeElapsed;
		}

		public void Restart(Vector3 _direction, float _speed, float _stunTime)
		{
			if (go.GetComponent<Enemy>().isDead)
				return;

			direction = _direction;
			speed = _speed;
			stunTime = _stunTime;
			hasHitWall = false;
			//playedStunned = false;
			
			go.GetComponent<BedEnemy>().animator.Play("charge");
		}
	}
}
