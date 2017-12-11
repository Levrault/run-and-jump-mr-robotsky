using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the player's health
/// </summary>
public class PlayerHealth : MonoBehaviour {
  public int health = 3;
  private int currentHealth;

  void Start() {
    currentHealth = health;
  }

  /// <summary>
  /// When player take damage, his life reduce of 1 point
  /// </summary>
  public void TakeDamage() {
    currentHealth--;
    if (currentHealth == 0) {
      PlayerManager.instance.KillPlayer(gameObject);
    }
  }

  /// <summary>
  /// When player collect item, he can regain
  /// health if he needs it
  /// </summary>
  public void RegainHealth() {
    if (currentHealth < 3 && currentHealth > 0) {
      currentHealth++;
    }
  }
}
