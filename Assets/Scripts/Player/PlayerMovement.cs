using System.Collections;
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
    // sprite direction
    SetSpriteRendererDirection();

    // animation state
    animator.SetFloat("moveX", speed);
    animator.SetFloat("moveY", velocity.y);
    animator.SetBool("isGrounded", isGrounded);

    // velocity for the next frame
    targetVelocity = new Vector2(directionalInput.x, Vector2.zero.y) * speed;
  }

  /// <summary>
  /// Set player to idle
  /// </summary>
  public void Idle() {
    speed = 0f;
  }

  /// <summary>
  /// Set player to walking
  /// </summary>
  public void Walk() {
    speed = walkingSpeed;
  }

  /// <summary>
  /// Set player to run (increase his speed)
  /// </summary>
  public void Run() {
    speed = runningSpeed;
  }

  /// <summary>
  /// Set the player to jump
  /// </summary>
  public void Jump() {
    if (IsGrounded()) {
      velocity.y = jumpForce;
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