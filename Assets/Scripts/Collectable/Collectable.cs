using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collectable, increase health and player score
/// </summary>
public class Collectable : MonoBehaviour {

  public int amount = 100;
  private Animator animator;
	private CollectableSound collectableSound;

  void Start() {
		animator = GetComponent<Animator>();
		collectableSound = GetComponent<CollectableSound>();
  }

  /// <summary>
  /// Sent when another object enters a trigger collider attached to this
  /// object (2D physics only).
  /// </summary>
  /// <param name="other">The other Collider2D involved in this collision.</param>
  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      PlayerManager.instance.IncreasePlayerScore(other.gameObject, amount);
      collectableSound.PlayCollectedAudioClip();
			animator.SetTrigger("isCollected");
    }
  }
}
