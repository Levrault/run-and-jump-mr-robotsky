using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the player's health
/// </summary>
public class PlayerHealth : MonoBehaviour {
  public int health = 3;
  public bool godMode = false;
  private int currentHealth;
  private const int blinkFrame = 20;
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
  /// <param name="amount"></param>
  public void TakeDamage(int amount) {
    currentHealth = currentHealth - amount;
    hudController.UpdateHealthBar(currentHealth);
    if (currentHealth == 0) {
      PlayerManager.instance.KillPlayer(gameObject);
    } else {
      blinkCounter = 0;
      StartCoroutine(Blink());
    }

    if (godMode) {
      currentHealth = 3;
    }
  }

  /// <summary>
  /// When player collect item, he can regain
  /// health if he needs it
  /// </summary>
  public void RegainHealth() {
    if (currentHealth < 3 && currentHealth > 0) {
      currentHealth++;
      hudController.UpdateHealthBar(currentHealth);
    }
  }

  /// <summary>
  /// Make the player blink
  /// </summary>
  /// <returns></returns>
  IEnumerator Blink() {
    yield return new WaitForSeconds(Time.deltaTime);

    // show/hide for 4 frames
    spriteRenderer.enabled = (blinkCounter <= blinkFrame && blinkCounter%4 == 0);
    blinkCounter++;

    if (blinkCounter >= blinkFrame) {
      spriteRenderer.enabled = true;
      blinkCounter = 0;
    } else {
      StartCoroutine(Blink());
    }
  }
}
