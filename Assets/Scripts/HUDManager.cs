using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Update HUD of the player
/// </summary>
public class HUDManager : MonoBehaviour {
  public static HUDManager instance = null;
	public GameObject score;
	public GameObject healthBar;

  void Awake() {
    if (instance == null) {
      instance = this;
    } else if (instance != this) {
      Destroy(gameObject);
    }
  }

	/// <summary>
	/// Update player score
	/// </summary>
	/// <param name="Player"></param>
	/// <param name="score"></param>
	public void UpdateScore(GameObject Player, int score) {

	}

	/// <summary>
	/// Update player health bar
	/// </summary>
	/// <param name="health"></param>
	public void UpdateHealthBar(int health) {
		healthBar.GetComponent<Animator>().SetInteger("health", health);
	}
}
