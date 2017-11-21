﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage all the player mouvement (walk, run, jump, wall jump etc.)
/// </summary>
public class PlayerMovement : PhysicObject {
  // Player's component
  private Animator animator;
  private SpriteRenderer spriteRenderer;

  // player's params
  public float walkingSpeed = 4f;
  public float runningSpeed = 7.0f;
  public float jumpForce = 7.0f;
  public Transform groundCheck;
  public LayerMask groundLayer;

  // private
  private Vector2 directionalInput;
  private bool isFlippedRight = true;
  private float speed = 0f;

  void Awake() {
    animator = (Animator) GetComponent(typeof(Animator));
    spriteRenderer = (SpriteRenderer) GetComponent(typeof(SpriteRenderer));
  }

  /// <summary>
  /// Custom velocity for the player
  /// </summary>
  protected override void ComputeVelocity() {
    SetSpriteRendererDirection();
    targetVelocity = new Vector2(directionalInput.x, Vector2.zero.y) * speed;
  }

  /// <summary>
  /// Set player to idle
  /// </summary>
  public void Idle() {
    if (IsGrounded()) {
      speed = 0f;
      animator.SetBool("isIdle", true);
      animator.Play("PlayerIdle");
    }
  }

  /// <summary>
  /// Set player to walking
  /// </summary>
  public void Walk() {
    speed = walkingSpeed;

    animator.SetBool("isRunning", false);
    animator.SetBool("isWalking", true);
    if (IsGrounded()) {
      animator.Play("PlayerWalk");
    }
  }

  /// <summary>
  /// Set player to run (increase his speed)
  /// </summary>
  public void Run() {
    speed = runningSpeed;

    animator.SetBool("isWalking", false);
    animator.SetBool("isRunning", true);
    if (IsGrounded()) {
      animator.Play("PlayerRun");
    }
  }

  /// <summary>
  /// Set player to fall
  /// </summary>
  public void Fall() {
    if (!animator.GetBool("isFalling")) {
      ResetPlayerStateMachine();
      animator.SetBool("isFalling", true);
      animator.Play("PlayerFall");
    }
  }

  /// <summary>
  /// Set the player to jump
  /// </summary>
  public void Jump() {
    if (IsGrounded()) {
      ResetPlayerStateMachine();
      animator.SetBool("isJumping", true);
      velocity.y = jumpForce;
      animator.Play("PlayerJump");
    }
  }

  /// <summary>
  /// Make smaller jump if jump button is release earlier
  /// </summary>
  public void JumpTakeOff() {
    if (velocity.y > 0) {
      velocity.y = velocity.y * 0.5f;
    }
  }

  /// <summary>
  /// Reset all the player's state machie to false
  /// </summary>
  public void ResetPlayerStateMachine() {
    animator.SetBool("isIdle", false);
    animator.SetBool("isWalking", false);
    animator.SetBool("isRunning", false);
    animator.SetBool("isJumping", false);
  }

  /// <summary>
  /// Set spriteRenderer flipping direction when
  /// directionInput change
  /// </summary>
  public void SetSpriteRendererDirection() {
    if (directionalInput.x < 0 && isFlippedRight) {
      spriteRenderer.flipX = true;
      isFlippedRight = false;
    } else if (!isFlippedRight && directionalInput.x > 0) {
      spriteRenderer.flipX = false;
      isFlippedRight = true;
    }
  }

  /// <summary>
  /// Sets the directional input.
  /// </summary>
  /// <returns>The directional input.</returns>
  /// <param name="input">input.</param>
  public void SetDirectionalInput(Vector2 input) {
    directionalInput = input;
  }

  /// <summary>
  /// Does the player hit the ground ?
  /// </summary>
  /// <returns>bool</returns>
  public bool IsGrounded() {
    return isGrounded;
  }

}