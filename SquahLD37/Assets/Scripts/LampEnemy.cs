using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampEnemy : Enemy
{
	public float preferredDistanceFromPlayer;
	public float spawnTime;
	public float throwDelay;
	public GameObject projectilePrefab;
	public float projectileSpeed;
	public float projectileDistance;

	public GameObject Light;

	private WalkToPlayerState walk;
	private ThrowState throwProjectile;
	
	public override void Update ()
	{
		base.Update();
		if (isDead)
			return;

		walk.Update(Time.deltaTime);
		throwProjectile.Update(Time.deltaTime);
		
		fsm.CurrentState.Reason(Player.instance.gameObject, gameObject);
		fsm.CurrentState.Act(Player.instance.gameObject, gameObject);
	}

	public override void MakeFSM()
	{
		walk = new WalkToPlayerState(preferredDistanceFromPlayer, gameObject);
		walk.AddTransition(Transition.ThrowTransition, StateID.Walk);

		throwProjectile = new ThrowState(spawnTime, throwDelay, projectilePrefab, projectileSpeed, projectileDistance, gameObject);
		throwProjectile.AddTransition(Transition.WalkTransition, StateID.Throw);

		fsm = new FSMSystem();
		fsm.AddState(walk, StateID.Throw);
		fsm.AddState(throwProjectile, StateID.Walk);
	}

	public override void Die() {
		base.Die();

		Light.SetActive(false);
	}

	private class WalkToPlayerState : FSMState
	{
		public float preferredDistanceToPlayer;
		
		private GameObject go;

		public WalkToPlayerState(float _preferredDistanceToPlayer, GameObject _go)
		{
			go = _go;
			Restart(_preferredDistanceToPlayer);
		}

		public override void Reason(GameObject player, GameObject npc)
		{
			if (inRangeCondition(player, npc))
			{
				LampEnemy enemy = npc.GetComponent<LampEnemy>();
				enemy.throwProjectile.Restart(enemy.spawnTime, enemy.throwDelay, enemy.projectilePrefab, enemy.projectileSpeed, enemy.projectileDistance);
				enemy.SetTransition(Transition.ThrowTransition);
			}
		}

		public override void Act(GameObject player, GameObject npc)
		{
			if (npc.GetComponent<Enemy>().isDead)
				return;

			if (!inRangeCondition(player, npc))
			{
				npc.GetComponent<EnemyMovement>().enabled = true;
			}
		}

		public void Update(float timeElapsed)
		{
			//Nothing
		}

		public void Restart(float _preferredDistanceToPlayer)
		{
			if (go.GetComponent<Enemy>().isDead)
				return;

			preferredDistanceToPlayer = _preferredDistanceToPlayer;
			
			go.GetComponent<LampEnemy>().animator.Play("walk");
		}

		private bool inRangeCondition(GameObject player, GameObject npc)
		{
			return (player.transform.position - npc.transform.position).magnitude < preferredDistanceToPlayer;
		}
	}
	
	private class ThrowState : FSMState
	{
		private float spawnTime;
		private float timeUntilSpawn;
		private float throwTime;
		private float timeUntilThrow;

		private bool hasSpawned;

		private GameObject projectilePrefab;
		private float projectileSpeed;
		private float projectileDistance;

		private GameObject go;

		public ThrowState(float _spawnTime, float _throwTime, GameObject _projectilePrefab, float _projectileSpeed, float _projectileDistance, GameObject _go)
		{
			go = _go;
			Restart(_spawnTime, _throwTime, _projectilePrefab, _projectileSpeed, _projectileDistance);
		}

		public override void Reason(GameObject player, GameObject npc)
		{
			if (timeUntilThrow <= 0)
			{
				LampEnemy enemy = npc.GetComponent<LampEnemy>();
				enemy.walk.Restart(enemy.preferredDistanceFromPlayer);
				enemy.SetTransition(Transition.WalkTransition);
			}
		}

		public override void Act(GameObject player, GameObject npc)
		{
			if (npc.GetComponent<Enemy>().isDead)
				return;
			npc.GetComponent<EnemyMovement>().enabled = false;
			npc.GetComponent<Rigidbody2D>().velocity *= 0.5f;

			if (timeUntilSpawn <= 0 && !hasSpawned)
			{
				GameObject projectile = Instantiate(projectilePrefab, npc.transform.position, Quaternion.identity);
				projectile.GetComponent<LampProjectile>().Initialize(player.gameObject.transform.position - npc.gameObject.transform.position, projectileSpeed, projectileDistance);
				hasSpawned = true;
			}
		}

		public void Update(float timeElapsed)
		{
			timeUntilThrow -= timeElapsed;
			timeUntilSpawn -= timeElapsed;

			
		}

		public void Restart(float _spawnTime, float _throwTime, GameObject _projectilePrefab, float _projectileSpeed, float _projectileDistance)
		{
			if (go.GetComponent<Enemy>().isDead)
				return;

			spawnTime = _spawnTime;
			throwTime = _throwTime;
			projectilePrefab = _projectilePrefab;
			projectileSpeed = _projectileSpeed;
			projectileDistance = _projectileDistance;

			timeUntilThrow = spawnTime;
			timeUntilSpawn = throwTime;

			hasSpawned = false;

			go.GetComponent<LampEnemy>().animator.Play("throw");
		}
	}
}
