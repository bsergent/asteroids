using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {
	public Asteroid prefabAsteroid;
	public UnityEngine.UI.Image imgTimer;
	public Transform target;
	public float spawnWarmup = 3;
	public float spawnPeriod = 10;
	public int asteroidsSpawned = 0;

	[Header("Asteroid Properties")]
	public float accelerationBase = 0.7f;
	public float accelerationIncrement = 0.05f;
	public float sizeBase = 1;
	public float sizeIncrement = 0.1f;
	public float massBase = 3.6f;
	public float massIncrement = 0.24f;

	private float lastSpawn;

	void Awake() {
		lastSpawn = Time.time - spawnPeriod + spawnWarmup;
		imgTimer.fillAmount = (Time.time - lastSpawn) / spawnPeriod;
	}

	void Update() {
		imgTimer.fillAmount = (Time.time - lastSpawn) / spawnPeriod;
		if (Time.time > lastSpawn + spawnPeriod)
			Spawn();
	}

	void Spawn() {
		lastSpawn = Time.time;

		float acceleration = accelerationBase + asteroidsSpawned * accelerationIncrement;
		float size = sizeBase + asteroidsSpawned * sizeIncrement;
		float mass = massBase + asteroidsSpawned * massIncrement;

		// Get random point on edge of screen
		float camHeightHalf = Camera.main.orthographicSize;
		float camWidthHalf = Camera.main.aspect * camHeightHalf;
		var pos = Vector2.zero;
		if (Random.value > 0.5f) {
			pos.x = Mathf.Sign(Random.value - 0.5f);
			pos.y = Random.value - 0.5f;
		} else {
			pos.x = Random.value - 0.5f;
			pos.y = Mathf.Sign(Random.value - 0.5f);
		}
		pos.x *= camWidthHalf;
		pos.y *= camHeightHalf;
		pos += (pos - (Vector2)target.position).normalized * size;

		// Spawn an asteroid and hurl it toward earth
		var ast = GameObject.Instantiate<Asteroid>(prefabAsteroid, pos, Quaternion.identity);
		ast.Hurl(target, acceleration, size, mass);

		asteroidsSpawned++;
	}
}
