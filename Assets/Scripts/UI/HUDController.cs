using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Update HUD of the player
/// </summary>
public class HUDController : MonoBehaviour {
	public GameObject score;
	public GameObject healthBar;

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
