using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {
	public float minDistToAccelerateTowards = 0.5f;
	public float thrust = 1;
	private float lastCollisionTime;
	public float speed;

	private Rigidbody2D _rb;
	private Vector2 _dest = Vector2.zero;

	void Start() {
		_rb = GetComponent<Rigidbody2D>();
	}

	void Update() {
		if (Input.GetMouseButton(0))
			_dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		else
			_dest = Vector2.zero;
	}

	void FixedUpdate() {
		if (_dest == Vector2.zero) return;
		var dist = _dest - (Vector2)transform.position;
		if (dist.magnitude < minDistToAccelerateTowards) return;
		_rb.AddForce(dist.normalized * thrust);
		speed = _rb.velocity.magnitude;
	}

	void OnCollisionEnter2D(Collision2D col) {
		Debug.Log($"Ship collided with {col.gameObject.name} at speed {speed}");
		GameManager.camControl.Shake(speed / 18, 0.05f);
		lastCollisionTime = Time.time;
	}
}
