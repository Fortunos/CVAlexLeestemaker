using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleter : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.tag == "Finish")
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                //Go to the next scene
                currentSceneIndex += 1;
                SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
            }
            else
            {
                //Game Complete!
            }
        }
    }
}
