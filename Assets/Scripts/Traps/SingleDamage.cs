using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make a single damage
/// </summary>
public class SingleDamage : MonoBehaviour {

  public int amount = 1;

  /// <summary>
  /// Sent when another object enters a trigger collider attached to this
  /// object (2D physics only).
  /// </summary>
  /// <param name="other">The other Collider2D involved in this collision.</param>
  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      PlayerManager.instance.DamagePlayer(other.gameObject, amount);
    }
  }

}
