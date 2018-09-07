using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public Image[] hearts;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        int health = Player.instance.health.health;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i >= health)
            {
                hearts[i].enabled = false;
            }
        }
    }
}