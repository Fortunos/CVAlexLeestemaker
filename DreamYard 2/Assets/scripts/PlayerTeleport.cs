using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    Transform transform;
    PlayerMovement movement;

    // Use this for initialization
    void Start()
    {
        transform = GetComponent<Transform>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// On colliding with a trigger, teleport if UP is pressed
    /// </summary>
    /// <param name="trigger"></param>
    void OnTriggerStay(Collider trigger)
    {
        //If we're aren't pressing teleport, return
        if (!Input.GetButtonDown("Teleport")) return;
        //If we're not on the ground, return
        if (!movement.IsOnGround()) return;

        //Get the teleporter
        var teleporter = trigger.gameObject.GetComponent<Teleporter>();
        if (teleporter == null) return;

        //Get what we need to go to (this can be null, in which case we don't teleport)
        var goTo = teleporter.linkedTo;
        if (goTo == null) return;

        //Teleport!
        transform.position = new Vector3(goTo.GetComponent<Transform>().position.x, goTo.GetComponent<Transform>().position.y, 0);
        StartCoroutine(movement.ResetCanFlip());
    }
}
