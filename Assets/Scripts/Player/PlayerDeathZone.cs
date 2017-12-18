using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kill player when he reach the death zone limit
/// </summary>
public class PlayerDeathZone : MonoBehaviour {
  public float limitY;
  private PlayerHealth playerHealth;

  void Awake() {
    playerHealth = GetComponent<PlayerHealth>();
  }

  void Update() {
    if (transform.position.y < limitY) {
      playerHealth.InstantKill();
    }
  }

  void OnDrawGizmos() {
    Gizmos.color = Color.red;
    Vector3 from = new Vector3(transform.position.x - 3, limitY, transform.position.z);
    Vector3 to = new Vector3(transform.position.x + 3, limitY, transform.position.z);
    Gizmos.DrawLine(from, to);
  }
}
