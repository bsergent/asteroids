using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	public static CameraController camControl => Instance._camControl;
	public static bool paused => Instance._paused;
	public static float timeSinceStart => Time.time - Instance._startTime;

	[SerializeField] private CameraController _camControl;
	public AsteroidSpawner spawner;
	public GameObject earth;
	public TMPro.TextMeshPro lblHealth;
	public TMPro.TextMeshProUGUI lblScore;
	public TMPro.TextMeshProUGUI lblGameOver;

	private float _startTime;
	private int _highscore;
	private bool _paused = true;

	[SerializeField] private int _health = 10;
	public static int health {
		get => Instance._health;
		set {
			// Pulse number on change
			if (health != value) {
				Instance.lblHealth.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
				Instance.earth.transform.DOShakePosition(0.1f, 0.05f);
				camControl.ChromaticAbborate(0.2f);
			}
			Instance._health = value;
			Instance.lblHealth.text = value.ToString();
			if (health == 1)
				Instance.lblHealth.color = Color.red;

			if (health <= 0)
				Instance.GameOver();
		}
	}

	void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);

		_highscore = PlayerPrefs.GetInt("highscore", 0);
		UpdateScore();

		_startTime = Time.time;
		health = health;
		spawner.gameObject.SetActive(false);
	}

	void Update() {
		// Reload scene if game over and press space
		if (Input.GetKeyDown(KeyCode.Space) && health <= 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		// Unpause when first clicking
		if (Input.GetMouseButtonDown(0) && paused && health > 0) {
			spawner.gameObject.SetActive(true);
			_startTime = Time.time;
			_paused = false;
		}
		if (paused) return;

		// Zoom out camera and update score
		if (health > 0)
			UpdateScore();
	}

	void UpdateScore() {
		string scoreText = $"<mspace=0.7em>{Time.time - _startTime:00}";
		if (_highscore > 0)
			scoreText += $"\n<alpha=#15>{_highscore:00}</color>";
		scoreText += "</mspace>";
		lblScore.text = scoreText;
	}

	void GameOver() {
		_paused = true;
		lblHealth.enabled = false;
		earth.GetComponent<SpriteRenderer>().enabled = false;
		earth.GetComponent<Collider2D>().enabled = false;
		earth.transform.Find("Smoke Particles").GetComponent<ParticleSystem>().Play();

		lblGameOver.gameObject.SetActive(true);
		if (Time.time - _startTime > _highscore)
			PlayerPrefs.SetInt("highscore", Mathf.FloorToInt(Time.time - _startTime));
	}

	public static void FreezeFrame(float duration) {
		Instance.StartCoroutine(Instance.CoFreezeFrame(duration));
	}

	IEnumerator CoFreezeFrame(float duration) {
		float prevTimeScale = Time.timeScale;
		Time.timeScale = 0.25f;
		yield return new WaitForSecondsRealtime(duration);
		Time.timeScale = prevTimeScale;
	}
}
