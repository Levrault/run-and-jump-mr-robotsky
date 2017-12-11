using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damage Player every X times
/// </summary>
public class LoopDamage : MonoBehaviour {
  public float loopingTimer = 1f;
  private bool isLoopingDamage = false;
  private GameObject player;

  /// <summary>
  /// Sent when another object enters a trigger collider attached to this
  /// object (2D physics only).
  /// </summary>
  /// <param name="other">The other Collider2D involved in this cllision.</param>
  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      player = other.gameObject;
      isLoopingDamage = true;
      StartCoroutine(DamageTimer(loopingTimer));
    }
  }

  /// <summary>
  /// Sent when another object leaves a trigger collider attached to
  /// this object (2D physics only).
  /// </summary>
  /// <param name="other">The other Collider2D involved in this collision.</param>
  void OnTriggerExit2D(Collider2D other) {
    if (other.tag == "Player") {
      isLoopingDamage = false;
    }
  }

  /// <summary>
  /// Damage player every second he stays in the traps
  /// </summary>
  /// <param name="time"></param>
  /// <returns></returns>
  IEnumerator DamageTimer(float time) {
    PlayerManager.instance.DamagePlayer(player);
    yield return new WaitForSeconds(time);

    if (isLoopingDamage) {
      StartCoroutine(DamageTimer(loopingTimer));
    }
  }
}
