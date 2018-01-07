using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikes : MonoBehaviour {

  private Animator animator;
  public GameObject damageCollider;

  void Awake() {
    animator = GetComponent<Animator>();
  }

  /// <summary>
  /// Sent when another object enters a trigger collider attached to this
  /// object (2D physics only).
  /// </summary>
  /// <param name="other">The other Collider2D involved in this collision.</param>
  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      animator.SetTrigger("isTriggered");
    }
  }

  /// <summary>
  /// Active a box collider to damage player when pikes are out
  /// </summary>
  public void ActiveDamageCollider() {
    damageCollider.SetActive(true);
  }

  public void InactiveDamageCollider() {
    damageCollider.SetActive(false);
  }
}
