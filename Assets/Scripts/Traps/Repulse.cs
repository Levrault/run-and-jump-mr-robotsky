using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulse : MonoBehaviour {

  public Vector2 repulseForce = new Vector2(4,4);
  public int frame = 4;

  /// <summary>
  /// Sent when another object enters a trigger collider attached to this
  /// object (2D physics only).
  /// </summary>
  /// <param name="other">The other Collider2D involved in this collision.</param>
  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      other.gameObject.GetComponent<PlayerMovement>().AddForce(repulseForce, frame);
    }
  }
}
