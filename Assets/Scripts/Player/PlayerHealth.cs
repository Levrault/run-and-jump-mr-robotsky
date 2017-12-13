using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the player's health
/// </summary>
public class PlayerHealth : MonoBehaviour {
  public int health = 3;
  private int currentHealth;
  private const int blinkFrame = 4;
  private int blinkCounter = 0;
  private SpriteRenderer spriteRenderer;
  private HUDController hudController;

  void Start() {
    currentHealth = health;
    spriteRenderer = GetComponent<SpriteRenderer>();
    hudController = GetComponent<HUDController>();
  }

  /// <summary>
  /// When player take damage, his life reduce of 1 point
  /// </summary>
  public void TakeDamage() {
    currentHealth--;
    hudController.UpdateHealthBar(currentHealth);
    if (currentHealth == 0) {
      PlayerManager.instance.KillPlayer(gameObject);
    } else {
      blinkCounter = 0;
      StartCoroutine(Blink());
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

  /// <summary>
  /// Make the player blink
  /// </summary>
  /// <returns></returns>
  IEnumerator Blink() {
    yield return new WaitForSeconds(.125f);

    if (blinkCounter <= blinkFrame) {
      spriteRenderer.enabled = !spriteRenderer.enabled;
      blinkCounter++;
    } else {
      spriteRenderer.enabled = true;
    }

    StartCoroutine(Blink());
  }
}
