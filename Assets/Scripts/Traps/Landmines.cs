using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LandMines script behavior. Landmines will
/// detect player and explode after X times
/// </summary>
public class Landmines : MonoBehaviour {

  public float timeBeforeExplosion = 1f;
  private Animator animator;

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
      StartCoroutine(Explode(timeBeforeExplosion));
    }
  }

  /// <summary>
  /// LandMine explode
  /// </summary>
  /// <param name="time"></param>
  /// <returns></returns>
  IEnumerator Explode(float time) {
    yield return new WaitForSeconds(time);
    animator.SetTrigger("triggerExplosion");
  }
}
