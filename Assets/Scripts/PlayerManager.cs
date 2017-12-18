using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager players stats throught the game
/// </summary>
public class PlayerManager : MonoBehaviour {

  public static PlayerManager instance = null;
  public int playersInGame = 1;

  void Awake() {
    if (instance == null) {
      instance = this;
    } else if (instance != this) {
      Destroy(gameObject);
    }
  }


  /// <summary>
  /// Damage a player
  /// </summary>
  /// <param name="player"></param>
  /// <param name="amount"></param>
  public void DamagePlayer(GameObject player, int amount) {
    player.GetComponent<PlayerHealth>().TakeDamage(amount);
  }

  /// <summary>
  /// Player's health is equal to zero
  /// </summary>
  /// <param name="player"></param>
  public void KillPlayer(GameObject player) {
    playersInGame--;
    player.GetComponent<Animator>().SetTrigger("isDeath");
  }
}
