using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the player's health
/// </summary>
public class PlayerHealth : MonoBehaviour {
  // active godmode for debug
  public bool godMode = false;
  public int health = 3;
  private int currentHealth;
  private PlayerSound playerSound;

  // blink animation params
  private const int blinkFrame = 20;
  private int blinkCounter = 0;
  private SpriteRenderer spriteRenderer;

  // updata player's HUD
  private HUDController hudController;

  // let the player be invulnerable after being hit
  private bool isInvulnerable;

  void Start() {
    currentHealth = health;
    spriteRenderer = GetComponent<SpriteRenderer>();
    hudController = GetComponent<HUDController>();
    playerSound = GetComponent<PlayerSound>();
  }

  /// <summary>
  /// When player take damage, his life reduce of 1 point
  /// </summary>
  /// <param name="amount"></param>
  public void TakeDamage(int amount) {

    // can be hit
    if (!isInvulnerable) {
      currentHealth = currentHealth - amount;
      hudController.UpdateHealthBar(currentHealth);
      playerSound.PlayerGetttingHurtAudioClip();
    }

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
    spriteRenderer.enabled = (blinkCounter <= blinkFrame && blinkCounter % 4 == 0);
    blinkCounter++;
    isInvulnerable = true;

    if (blinkCounter >= blinkFrame) {
      spriteRenderer.enabled = true;
      blinkCounter = 0;
      isInvulnerable = false;
    } else {
      StartCoroutine(Blink());
    }
  }
}
