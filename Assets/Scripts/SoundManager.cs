using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
	public static SoundManager inst;

	[Header("References")]
	public AudioMixer mixer;
	public AudioSource audioMusicIntro;
	public AudioSource audioMusicLoop;
	public AudioSource audioEffect;

	void Awake() {
		// Singleton check
		if (SoundManager.inst == null) SoundManager.inst = this;
		else { Destroy(gameObject); return; }
		DontDestroyOnLoad(this);
	}

	void Start() {
		audioMusicIntro.Play();
	}

	void Update() {
		if (!audioMusicIntro.isPlaying && !audioMusicLoop.isPlaying)
			audioMusicLoop.Play();
	}

	protected Dictionary<AudioClip, float> lastClipPlay = new Dictionary<AudioClip, float>();
	public static void PlayOneShot(AudioClip clip, float pitchRange = 0, float volScale = 1) {
		if (inst == null) return;

		// Don't play the same audio clip over itself
		if (inst.lastClipPlay.ContainsKey(clip) && Time.time < inst.lastClipPlay[clip] + clip.length)
			return;
		inst.lastClipPlay[clip] = Time.time;

		inst.audioEffect.pitch = 1 + Random.Range(-pitchRange, pitchRange);
		inst.audioEffect.PlayOneShot(clip, volScale);
	}

	public static void PlayVariedOneShot(List<AudioClip> clips, float pitchRange = 0, float volScale = 1) {
		PlayOneShot(clips[Random.Range(0, clips.Count)], pitchRange, volScale);
	}

	public static void PlayOneShotStackable(AudioClip clip, float pitchRange = 0, float volScale = 1) {
		if (inst == null) return;
		inst.audioEffect.pitch = 1 + Random.Range(-pitchRange, pitchRange);
		inst.audioEffect.PlayOneShot(clip, volScale);
	}

	public enum SoundChannel {
		Music,
		Effects
	}
}