using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Update HUD of the player
/// </summary>
public class HUDController : MonoBehaviour {
	public Text scoreText;
	public GameObject healthBar;
	private int score = 0;

	/// <summary>
	/// Update player score
	/// </summary>
	/// <param name="Player"></param>
	/// <param name="amount"></param>
	public void UpdateScore(int amount) {
		score += amount;
		scoreText.text = score.ToString("000000");
	}

	/// <summary>
	/// Update player health bar
	/// </summary>
	/// <param name="health"></param>
	public void UpdateHealthBar(int health) {
		healthBar.GetComponent<Animator>().SetInteger("health", health);
	}
}
