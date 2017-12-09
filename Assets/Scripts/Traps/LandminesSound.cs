using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Traps (danger) sound
/// </summary>
public class LandminesSound : MonoBehaviour {

  public AudioClip explosionSound;

	public void PlayExplosionAudioClip() {
		SoundManager.instance.PlaySingle(explosionSound);
	}

}


