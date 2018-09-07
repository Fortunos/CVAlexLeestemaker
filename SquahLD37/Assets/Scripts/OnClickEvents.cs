using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickEvents : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene(2);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }
}