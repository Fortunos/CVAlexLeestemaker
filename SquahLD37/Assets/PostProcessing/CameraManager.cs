using System.Collections;
using System.Linq;
using DG.Tweening;
using DynamicLight2D;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraManager : MonoBehaviour {
	public static CameraManager instance;

	public Color MinVignetteColor;
	public Color MaxVignetteColor;

	public float MinVignetteIntensity;
	public float MaxVignetteIntensity;

	public Gradient DiscoGradient;



	public DynamicLight[] Lights;
	Material[] m_lightColors;
	Material[] m_fixtureGlow;
	Color[] m_colors;

	Camera m_camera;



	private PostProcessingBehaviour m_manager;
	float m_currentCombo;


	void OnEnable() {
		instance = this;
		m_camera = GetComponent<Camera>();
		m_manager = GetComponent<PostProcessingBehaviour>();

		m_lightColors = new Material[Lights.Length];
		m_fixtureGlow = new Material[Lights.Length];

		for (int i = 0; i < Lights.Length; i++) {
			var dynamicLight = Lights[i];
			dynamicLight.LightMaterial = Instantiate(dynamicLight.LightMaterial);
			m_lightColors[i] = dynamicLight.LightMaterial;

			var fixture = dynamicLight.transform.Find("Fixture");
			m_fixtureGlow[i] = fixture.GetComponent<Renderer>().material;
		}

		StartCoroutine(SetDiscoColors());
	}

	void Update() {
		var pl = Player.instance;

		float healthFrac = 1 - pl.health.GetFractionOfTotalHealth();
		SetForFrac(healthFrac);
		SetCombo(Player.instance.combo.DiscoShit);
	}

	void OnDisable() {
		foreach (var mat in m_lightColors) {
			Destroy(mat);
		}

		SetForFrac(0);
		SetCombo(0);
	}

	void SetForFrac(float healthFrac) {
		var vignetteSettings = m_manager.profile.vignette.settings;
		vignetteSettings.intensity = Mathf.Lerp(MinVignetteIntensity, MaxVignetteIntensity, healthFrac);
		vignetteSettings.color = Color.Lerp(MinVignetteColor, MaxVignetteColor, healthFrac);

		m_manager.profile.vignette.settings = vignetteSettings;
	}

	public void OnDamage() {
		m_camera.DOShakePosition(0.1f, 0.1f).OnComplete(() => m_camera.transform.position = new Vector3(0, 0, -5));
	}

	IEnumerator SetDiscoColors() {
		m_colors = new Color[Lights.Length];

		while (true) {
			for (int i = 0; i < Lights.Length; ++i) {
				m_colors[i] = DiscoGradient.Evaluate(Random.value);
			}

			yield return new WaitForSeconds(1.0f);
		}
	}


	public void SetCombo(float combo) {
		m_currentCombo = Mathf.Clamp01(Mathf.Lerp(m_currentCombo, combo, Time.deltaTime));


		var colorGrade = m_manager.profile.colorGrading;
		var basicSetting = colorGrade.settings;
		basicSetting.basic.postExposure = Mathf.Lerp(-0.7f, -2.0f, combo);
		colorGrade.settings = basicSetting;

		for (int i = 0; i < Lights.Length; ++i) {
			Color glowColor = m_colors[i] * combo;
			m_lightColors[i].color = glowColor * 0.5f;
			m_fixtureGlow[i].SetColor("_EmissionColor", glowColor);
		}
	}

	public void OnSwing(Vector3 dir) {
		
	}

	public void OnBallHit(float velocity) {
		m_camera.DOShakePosition(0.05f, 0.003f * velocity).OnComplete(() => m_camera.transform.position = new Vector3(0, 0, -5));


		//DOTween.Punch(() => transform.position, vec => transform.position = vec, -dir,5.2f)
		//.OnComplete(() => transform.position = new Vector3(0, 0, -5));
	}
}