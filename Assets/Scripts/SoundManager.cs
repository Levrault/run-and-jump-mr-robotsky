using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

  public AudioSource efxSource;
  public AudioSource musicSource;
  public static SoundManager instance = null;

  public float lowPitchRange = .95f;
  public float highPitchRange = 1.05f;

  void Awake() {
    if (instance == null) {
      instance = this;
    } else if (instance != this) {
      Destroy(gameObject);
    }

    DontDestroyOnLoad(gameObject);
  }

  /// <summary>
  ///	Play single audioclip
  /// </summary>
  /// <param name="clip"></param>
  public void PlaySingle(AudioClip clip) {
    efxSource.PlayOneShot(clip);
  }

	/// <summary>
	/// Play ramdom clips. 
	/// Randomize help our sound to not
	/// feel repetitive and annoying
	/// </summary>
	/// <param name="clips"></param>
  public void RandomizeSfx(params AudioClip[] clips) {
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		efxSource.pitch = randomPitch;
		efxSource.clip = clips[randomIndex];
		efxSource.Play();

  }
}
