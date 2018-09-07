using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float InvulnFramesTime = .5f;

    public int maximumHealth;
    public int health;

    public float lastHit;


	public float GetFractionOfTotalHealth() {
		return (float) health / maximumHealth;
	}

	// Use this for initialization
    void Start()
    {
        //Set health to maximum to start with
        health = maximumHealth;
    }
	
    public void Damage(int x = 1)
    {
        if (Time.time - lastHit > InvulnFramesTime)
        {
            AudioManager.instance.PlaySound(Audio.GetHit);
            AudioManager.instance.PlaySound(Audio.MonsterAttack);
            CameraManager.instance.OnDamage();
            UIUpdate();
            health -= x;
            if (health <= 0)
            {
                Death();
            }
            UIUpdate();

            lastHit = Time.time;

	        StartCoroutine(PlayerFlicker());
        }
    }

	IEnumerator PlayerFlicker() {
		var rend = GetComponent<SpriteRenderer>();
		Color off = new Color(1, 1, 1, 0.2f);
		Color on = Color.white;

		while (Time.time - lastHit < InvulnFramesTime) {
			rend.color = off;
			yield return new WaitForSeconds(0.15f);
			rend.color = on;
			yield return new WaitForSeconds(0.15f);
		}

		rend.color = on;
	}

	public void Heal(int x)
    {
        health += Mathf.Clamp(health + x, 0, maximumHealth);
        UIUpdate();
    }

    public void Death()
    {
        //DEATH
	    AudioManager.instance.PlaySound(Audio.Death);

        Player.instance.score.SetHighscore();

        SceneManager.LoadScene(0);
    }

    private void UIUpdate()
    {

    }
}