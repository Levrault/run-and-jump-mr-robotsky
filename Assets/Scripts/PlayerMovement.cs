using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage all the player mouvement (walk, run, jump, wall jump etc.)
/// </summary>
public class PlayerMovement : MonoBehaviour {
	// Player's component
	private Rigidbody2D rigidBody2D;
	private Animator animator;
	private SpriteRenderer spriteRenderer;

	// player's params
	public float speed = 6.0f;
	
	// private
	private Vector3 velocity;

	void Awake() {
		rigidBody2D = (Rigidbody2D) GetComponent(typeof(Rigidbody2D));
		animator = (Animator) GetComponent(typeof(Animator));
		spriteRenderer = (SpriteRenderer) GetComponent(typeof(SpriteRenderer));
	}

	void Update() {
		CalculVelocity();
	}

	void CalculVelocity() {
		Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), 0);
		Debug.Log(movement);
		rigidBody2D.velocity = movement * speed;
	}
}