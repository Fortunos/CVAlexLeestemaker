using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoolEnemy : Enemy
{

	public float standStillWaitTime;
	public AnimationCurve speedDuringJump;
	public float maxSpeedDuringJump;
	public float jumpDuration;

	private StandStillState still;
	private JumpToPlayerState jump;

	// Use this for initialization
	//void Awake () {
		
	//}
	
	// Update is called once per frame
	public override void Update ()
	{
		base.Update();
		if (isDead)
			return;

		still.Update(Time.deltaTime);
		jump.Update(Time.deltaTime);
		
		fsm.CurrentState.Reason(Player.instance.gameObject, gameObject);
		fsm.CurrentState.Act(Player.instance.gameObject, gameObject);
	}

	public override void MakeFSM()
	{
		still = new StandStillState(gameObject.transform.position, standStillWaitTime, gameObject);
		still.AddTransition(Transition.JumpTransition, StateID.Still);
		
		jump = new JumpToPlayerState(transform.position, Player.instance.transform.position, speedDuringJump, maxSpeedDuringJump, jumpDuration, gameObject);
		jump.AddTransition(Transition.StillTransition, StateID.Jump);

		fsm = new FSMSystem();
		fsm.AddState(still, StateID.Jump);
		fsm.AddState(jump, StateID.Still);
	}

	private class StandStillState : FSMState
	{
		public float timeUntilMove;
		private GameObject go;

		public StandStillState(Vector3 position, float _waitTimeUntilMove, GameObject _go)
		{
			go = _go;
			Restart(position, _waitTimeUntilMove);
		}

		public override void Reason(GameObject player, GameObject npc)
		{
			if (timeUntilMove <= 0)
			{
				StoolEnemy enemy = npc.GetComponent<StoolEnemy>();
				enemy.jump.Restart(npc.transform.position, player.transform.position, enemy.speedDuringJump, enemy.maxSpeedDuringJump, enemy.jumpDuration);
				enemy.SetTransition(Transition.JumpTransition);
			}
		}

		public override void Act(GameObject player, GameObject npc)
		{
			if (npc.GetComponent<Enemy>().isDead)
				return;
			npc.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}

		public void Update(float timeElapsed)
		{
			timeUntilMove -= timeElapsed;
		}

		public void Restart(Vector3 position, float _waitTimeUntilMove)
		{
			if (go.GetComponent<Enemy>().isDead)
				return;

			timeUntilMove = _waitTimeUntilMove;
			go.GetComponent<StoolEnemy>().animator.Play("idle");
		}
	}

	private class JumpToPlayerState : FSMState
	{
		Vector3 startPosition;
		Vector3 targetPosition;
		private AnimationCurve speedDuringJump;
		public float jumpProgress;
		public float jumpDuration;
		private float maxSpeedDuringJump;
		private float timeProgress;

		private GameObject go;
		
		Vector3 direction
		{
			get { return (targetPosition - startPosition).normalized; }
		}

		public JumpToPlayerState(Vector3 position, Vector3 _targetPosition, AnimationCurve _speedDuringJump, float _maxSpeedDuringJump, float _jumpDuration, GameObject _go)
		{
			go = _go;
			Restart(position, _targetPosition, _speedDuringJump, _maxSpeedDuringJump, _jumpDuration);
		}

		public override void Reason(GameObject player, GameObject npc)
		{
			if (jumpProgress >= 1)
			{
				StoolEnemy enemy = npc.GetComponent<StoolEnemy>();
				enemy.still.Restart(npc.transform.position, enemy.standStillWaitTime);
				enemy.SetTransition(Transition.StillTransition);
			}
		}

		public override void Act(GameObject player, GameObject npc)
		{
			if (npc.GetComponent<Enemy>().isDead)
				return;

			Vector3 velocity = speedDuringJump.Evaluate(jumpProgress) * maxSpeedDuringJump * direction;
			npc.GetComponent<Rigidbody2D>().velocity = velocity;
		}

		public void Update(float timeElapsed)
		{
			timeProgress += timeElapsed;
			jumpProgress = timeProgress / jumpDuration;
		}

		public void Restart(Vector3 position, Vector3 _targetPosition, AnimationCurve _speedDuringJump, float _maxSpeedDuringJump, float _jumpDuration)
		{
			if (go.GetComponent<Enemy>().isDead)
				return;

			startPosition = position;
			targetPosition = _targetPosition;
			speedDuringJump = _speedDuringJump;
			maxSpeedDuringJump = _maxSpeedDuringJump;
			jumpDuration = _jumpDuration;

			jumpProgress = 0;
			timeProgress = 0;
			
			go.GetComponent<StoolEnemy>().animator.Play("jump");
		}
	}
}
