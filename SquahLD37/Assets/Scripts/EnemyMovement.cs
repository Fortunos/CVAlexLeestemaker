using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float speed;
    private Player player;

	public bool enabled = true;

    // Use this for initialization
    void Start()
    {
        player = Player.instance;
    }

    // Update is called once per frame
    public void DoStep()
    {
	    if (!enabled)
		    return;

        // Get positions of player and self
        Vector3 playerPos = player.gameObject.transform.position;
        Vector3 pos = gameObject.transform.position;
        // Calculate direction needed to travel in to get to the player
        Vector2 dir = new Vector2(playerPos.x - pos.x, playerPos.y - pos.y).normalized;

        rb2d.velocity = dir * speed;
    }
}