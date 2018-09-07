using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDieSystem : MonoBehaviour
{
    //private Vector3 checkpoint, gravreset;
    //private Quaternion rotationReset;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //RespawnHelper.Set(transform, transform);
        if (SceneManager.GetActiveScene().name == RespawnHelper.GetName)
        {
            rb.velocity = Vector3.zero;
            transform.position = RespawnHelper.GetPosition;
            transform.rotation = RespawnHelper.GetRotation;
            Physics.gravity = RespawnHelper.GetGravity;
        }

        SetCheckpoint(transform);
    }

    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.tag == "Checkpoint")
        {
            SetCheckpoint(trigger.transform);
        }

        if (trigger.tag == "Kill")
        {
            Die();
        }
    }

    void SetCheckpoint(Transform trigger)
    {
        RespawnHelper.Set(transform, trigger, SceneManager.GetActiveScene().name);
        //checkpoint = trigger.position;
        //gravreset = Physics.gravity;
        //rotationReset = transform.rotation;
    }

    void Die()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void NewLevel()
    {
        
    }
}
