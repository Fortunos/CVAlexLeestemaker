using UnityEngine;
using UnityEngine.UI;

public class ComboCounter : MonoBehaviour {
	public float ComboTime = 5;
	public float SecPerDecrease = 1;
	public int Threshold1;
	public int Threshold2;
	public Text UI;

	public int currentCombo;
	float lastUpped;
	float lastDecreased;
	int currentMusicLayer, formerMusicLayer;

	void Start() {
		currentCombo = 0;
		currentMusicLayer = 0;
		formerMusicLayer = 0;
		lastUpped = Time.time;
		lastDecreased = Time.time;
		UI.text = "Combo: 0X";
	}

	public void upCombo() {
		currentCombo++;
		lastUpped = Time.time;
		ComboUpdate();
	}

	void Update() {
		if (currentCombo > 0 && Time.time - lastUpped > ComboTime) {
			if (Time.time - lastDecreased > SecPerDecrease) {
				lastDecreased = Time.time;
				if (currentCombo >= Threshold2) {
					currentCombo -= 3;
				} else if (currentCombo >= Threshold1) {
					currentCombo -= 2;
				} else {
					currentCombo -= 1;
				}
				ComboUpdate();
			}
		}
	}

	void ComboUpdate() {
		UI.text = "Combo: " + currentCombo + "X";

		if (currentCombo >= Threshold2) {
			currentMusicLayer = 2;
		} else if (currentCombo >= Threshold1) {
			currentMusicLayer = 1;
		} else {
			currentMusicLayer = 0;
		}

		if (formerMusicLayer != currentMusicLayer) {
			AudioManager.instance.SwitchBase(currentMusicLayer);
			formerMusicLayer = currentMusicLayer;
		}
	}

	public float DiscoShit {
		get {
			if (currentCombo >= Threshold2) {
				return 1;
			} else {
				return currentCombo / (float) Threshold2;
			}
		}
	}
}