using UnityEngine;

public class PlayerLight : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		transform.up = Player.instance.GetComponent<PlayerInput>().pointDirection;
	}
}
