using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerAnimation animation;
    public Vector2 pointDirection;
    public bool usingController;

	private bool isSwinging;

    public void Start()
    {

    }

    public void Update()
    {
        setControllerType();
        setDirection();

	    if (PlayerAnimation.instance.globalCooldown <= 0 && Input.GetAxis("Swing") <= 0.1)
		    isSwinging = false;
		
	    if (Input.GetAxis("Swing") > 0.1f && !isSwinging)
	    {
		    Swing();
		    isSwinging = true;
	    }
    }

    private void Swing()
    {
        animation.UseAbility("swing");
	    AudioManager.instance.PlaySound(Audio.Swing);
	    CameraManager.instance.OnSwing(transform.up);
    }

    private void setControllerType()
    {
        if (Input.GetButtonDown("ControllerSwing") || Input.GetAxis("ControllerSwing") > 0.1f)
        {
            // Swing done with controller, get direction from right stick
            usingController = true;
        }
        else if (Input.GetButtonDown("MouseSwing"))
        {
            // Swing not done by controller = done by mouse, get direction from mouse
            usingController = false;
        }
    }

    private void setDirection()
    {
        if (usingController)
        {
            float xInput = Input.GetAxis("AimHor");
            float yInput = Input.GetAxis("AimVer");

	        if (Mathf.Approximately(xInput, 0) && Mathf.Approximately(yInput, 0))
	        {
		        xInput = Input.GetAxis("Horizontal");
		        yInput = Input.GetAxis("Vertical");
	        }

            Vector2 inputDirection = new Vector2(xInput, yInput);

            if (inputDirection != Vector2.zero)
            {
                // If the stick is sitting in the center, don't change from the last direction
                pointDirection = inputDirection;
            }
        }
        else
        {
            Vector3 vec1 = gameObject.transform.position;
            Vector3 vec2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointDirection = vec2 - vec1;
        }

	    Vector3 scale = gameObject.transform.localScale;
	    if (Mathf.Sign(scale.x) != Mathf.Sign(pointDirection.x))
	    {
		    gameObject.transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
	    }
    }

	
}