using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public PlayerMovement movement;
    public PlayerHealth health;
    public PlayerInput input;
    public Score score;
    public ComboCounter combo;

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }
}
