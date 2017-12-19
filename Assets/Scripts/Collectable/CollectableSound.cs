using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSound : MonoBehaviour {

  public AudioClip collectedSound;

	public void PlayCollectedAudioClip() {
		SoundManager.instance.PlaySingle(collectedSound);
	}
}
