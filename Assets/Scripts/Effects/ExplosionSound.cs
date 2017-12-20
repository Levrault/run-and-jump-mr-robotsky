using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Traps (danger) sound
/// </summary>
public class ExplosionSound : MonoBehaviour {

  public AudioClip explosionSound;
  public AudioClip tickSound;

	public void PlayExplosionAudioClip() {
		SoundManager.instance.PlaySingle(explosionSound);
	}

	public void PlayTickAudioClip() {
		SoundManager.instance.PlaySingle(tickSound);
	}

}


