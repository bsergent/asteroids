using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class CameraController : MonoBehaviour {
	public Volume volDamage;
	[Header("Zoom-out")]
	public float orthoSizeStart = 5;
	public float orthoSizeEnd = 10;
	public float orthoSizeDuration = 180;

	private Camera _cam;
	private PixelPerfectCamera _camPix;
	private Vector3 _initPos;
	private float _shakeStartTime;
	private float _shakeDuration;
	private float _shakeMagnitude;

	void Awake() {
		_cam = GetComponent<Camera>();
		_camPix = GetComponent<PixelPerfectCamera>();
	}

	void Start() {
		if (_camPix != null && _camPix.enabled) {
			_camPix.refResolutionX = GetCameraReferenceWidth();
			_camPix.refResolutionY = GetCameraReferenceWidth();
		} else {
			_cam.orthographicSize = Mathf.Lerp(orthoSizeStart, orthoSizeEnd, GameManager.timeSinceStart / orthoSizeDuration);
		}
	}

	void Update() {
		if (GameManager.paused) return;
		if (_camPix != null && _camPix.enabled) {
			_camPix.refResolutionX = GetCameraReferenceWidth();
			_camPix.refResolutionY = GetCameraReferenceWidth();
		} else {
			_cam.orthographicSize = Mathf.Lerp(orthoSizeStart, orthoSizeEnd, GameManager.timeSinceStart / orthoSizeDuration);
		}
	}

	int GetCameraReferenceWidth() {
		return Mathf.FloorToInt(
			Mathf.Lerp(orthoSizeStart, orthoSizeEnd, GameManager.timeSinceStart / orthoSizeDuration)
			* _camPix.assetsPPU) * 2;
	}

	public void Shake(float magnitude, float duration) {
		transform.DOShakePosition(duration, magnitude);
	}

	public void ChromaticAbborate(float duration) {
		DOTween.Punch(() => volDamage.weight * Vector3.one, val => volDamage.weight = val.x, Vector3.right, duration);
		// DOTween.To(val => volDamage.weight = val, 0, 1, duration).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InFlash);
	}
}
