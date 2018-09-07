using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {
	[ColorUsage(false, true, 0, 8, 0.125f, 3)] public Color GlowColor;

	[ColorUsage(false, true, 0, 8, 0.125f, 3)] public Color TrailGlowColor;


	public Rigidbody2D rb2d;
	public AnimationCurve speedFalloff;
	public AnimationCurve timeUntilStopBasedOnVelocity;

	public GameObject HitParticle;

	public Material GlowMaterial;
	public Material TrailGlowMaterial;
	public Material LightMaterial;

	public float minGlow;
	public float maxGlow = 5.76f;

	public float minSpeedForDamage;
	float velocityAtHit;
	Vector2 lastKnownVelocity;

	float timeUntilStop = 0;

	TrailRenderer m_trail;

	// Use this for initialization
	void Start() {
		m_trail = GetComponentInChildren<TrailRenderer>();
	}

	// Update is called once per frame
	void Update() {
		timeUntilStop -= Time.deltaTime;
	}

	float m_maxStopTime;

	public void SendHit() {
		m_maxStopTime = timeUntilStopBasedOnVelocity.Evaluate(rb2d.velocity.magnitude);
		velocityAtHit = rb2d.velocity.magnitude;
		timeUntilStop = m_maxStopTime;

		AudioManager.instance.PlaySound(Audio.SwingHit);
	}

	void FixedUpdate() {
		Vector2 velocity = rb2d.velocity;
		velocity.Normalize();
		velocity *= speedFalloff.Evaluate(timeUntilStop / (m_maxStopTime + 0.001f)) * velocityAtHit;

		lastKnownVelocity = velocity;
		rb2d.velocity = velocity;

		SetColors(velocity);
	}

	void OnDisable() {
		SetColors(Vector2.zero);
	}

	void SetColors(Vector2 velocity) {
		float glowMag = Mathf.Clamp((velocity.magnitude - 6.5f) * 0.15f, 0.0f, 2.4f);
		glowMag *= glowMag;

		glowMag = Mathf.Clamp(glowMag, minGlow, maxGlow);

		GlowMaterial.SetColor("_EmissionColor", GlowColor * glowMag);
		TrailGlowMaterial.SetColor("_EmissionColor", TrailGlowColor * glowMag * 0.5f);

		var col = LightMaterial.color;
		col.a = glowMag * 0.005f;
		LightMaterial.color = col;

		m_trail.time = Mathf.Lerp(0, 0.125f, glowMag - 1.25f);
	}

	public void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Enemy" && lastKnownVelocity.magnitude >= minSpeedForDamage) {
			// Pierce the enemy
			var point = col.contacts[0].point;
			var hit = Instantiate(HitParticle, point, Quaternion.FromToRotation(Vector3.forward, col.relativeVelocity.normalized));

			var pSystem = hit.GetComponent<ParticleSystem>();
			var main = pSystem.main;
			main.startSpeed = col.relativeVelocity.magnitude;

			Destroy(hit, hit.GetComponent<ParticleSystem>().main.duration);
			col.gameObject.GetComponent<Enemy>().Damage();
		}
	}
}