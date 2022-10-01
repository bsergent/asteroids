using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
	public Transform gravityWell;
	public float gravityAcceleration = 1;
	public float size {
		get => transform.localScale.x;
		set => transform.localScale = Vector3.one * value;
	}

	private Rigidbody2D _rb;
	private bool _seen = false;

	void Awake() {
		_rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		if (gravityWell == null) return;
		Vector2 direction = (gravityWell.position - transform.position).normalized;
		_rb.velocity += Vector2.one * direction * gravityAcceleration * Time.deltaTime;
	}

	public void Hurl(Transform target, float acceleration, float size, float mass) {
		this.gravityWell = target;
		this.size = size;
		this.gravityAcceleration = acceleration;
		_rb.mass = mass;
	}

	void OnBecameVisible() { // Includes scene camera
		_seen = true;
	}

	void OnBecameInvisible() { // Includes scene camera
		if (!_seen) return;
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D col) {
		// Debug.Log($"Collided with {col.otherCollider.gameObject.name}");
		if (col.gameObject.CompareTag("Earth")) {
			// Damage earth
			GameManager.health--;
			// Destory self
			Destroy(gameObject);
		}
	}
}
