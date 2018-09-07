using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    public float rotationSpeed;

    //private Vector3 velocity;
    private float rotateStart;
    private bool rotating = false;
    private Quaternion from, lastTo;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == RespawnHelper.GetName)
        {
            transform.rotation = RespawnHelper.GetRotation;
        }
    }

    void LateUpdate()
    {
        var playerPosition = player.transform.position;
        var pos = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
        transform.position = pos + (transform.rotation * offset);

        //transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, 0.3f);
        if (transform.rotation != player.transform.rotation)
        {
            if (!rotating)
            {
                rotateStart = Time.time;
                from = transform.rotation;
                lastTo = player.transform.rotation;
                rotating = true;
            }
            else if (player.transform.rotation != lastTo)
            {
                from = transform.rotation;
                lastTo = player.transform.rotation;
                rotateStart = Time.time;
            }
            //Debug.Log(transform.rotation + " is niet " + player.transform.rotation);
            transform.rotation = Quaternion.Lerp(from, player.transform.rotation, (Time.time - rotateStart) * rotationSpeed);
        }
        else
        {
            //Debug.Log("Goed gewerkt pik");
            rotating = false;
        }
    }
}
