using UnityEngine;

public class BallHit : MonoBehaviour
{
    public float strength;
	public float range;
	public GameObject ball; // This is really dirty

    private bool forceDisable;

    public float TimeStoppedBaseTime;
    public float TimePauseDelay;

    float timeStopped;
    float TimeStoppedTime;

    bool timeStop;
    bool pauseChargeUp;

    Rigidbody2D m_lastBallCollider;

    void OnEnable()
    {
        Time.timeScale = 1.0f;
    }

    public void Update()
    {
        // Delay the start of the timepause
        if (pauseChargeUp && timeStopped - Time.realtimeSinceStartup + TimePauseDelay < 0)
        {
            Time.timeScale = 0f;
            pauseChargeUp = false;
            timeStop = true;
            timeStopped = Time.realtimeSinceStartup;
        }

        if (timeStopped - Time.realtimeSinceStartup + TimeStoppedTime < 0 && timeStop)
        {
            Time.timeScale = 1.0f;
            timeStop = false;

	        Vector2 dirNormalized;


	        if (Player.instance.input.usingController) {
		        dirNormalized = Player.instance.input.pointDirection.normalized;
	        } else {
		        Vector3 vec1 = m_lastBallCollider.transform.position;
		        Vector3 vec2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		        dirNormalized = ((Vector2) vec2 - (Vector2) vec1).normalized;
	        }

	        float velocity = m_lastBallCollider.velocity.magnitude;
            velocity += strength;

            m_lastBallCollider.velocity = dirNormalized * velocity;
            m_lastBallCollider.GetComponent<BallMovement>().SendHit();

			

            AudioManager.instance.PlaySound(Audio.SwingHit);
        }
		
        if (PlayerAnimation.instance.globalCooldown > 0 && !forceDisable)
        {
            Hit();
        }
        else if (PlayerAnimation.instance.globalCooldown <= 0)
        {
            forceDisable = false;
        }
    }

	private void Hit()
	{
		if ((ball.transform.position - transform.position).magnitude > range)
			return;

		Rigidbody2D rb2d = ball.GetComponent<Rigidbody2D>();

		float velocity = rb2d.velocity.magnitude;
		CameraManager.instance.OnBallHit(velocity);
        forceDisable = true;

        // Used to stop time on hit
		float scale = Mathf.Clamp(rb2d.velocity.magnitude / 10, 0, 1);
		m_lastBallCollider = rb2d;

        pauseChargeUp = true;
        timeStopped = Time.realtimeSinceStartup;
        TimeStoppedTime = TimeStoppedBaseTime * scale;
	}
}
