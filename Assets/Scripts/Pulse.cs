using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pulse : MonoBehaviour {
	public Vector3 scaleSecond;
	public float period;

	void Start() {
		transform.DOScale(scaleSecond, period).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
	}
}
