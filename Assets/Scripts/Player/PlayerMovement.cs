using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage all the player mouvement (walk, run, jump, wall jump etc.)
/// </summary>
public class PlayerMovement : PhysicObject {
  // Player's component
  private Animator animator;

  // player's params
  public float walkingSpeed = 4f;
  public float runningSpeed = 7.0f;
  public float jumpForce = 7.0f;
  public Vector2 wallJumpLeap = new Vector2(18, 17);
  public Transform wallJumpCheck;
  public LayerMask wallJumpLayer;

  // private
  private Vector2 directionalInput;
  private bool isFacingRight = true;
  private float speed = 0f;
  private bool isWallJumping = false;
  private bool isVelocityForWallJumping = false;
  private bool isNeedToSwitchDirection = false;
  private int wallJumpDirectionX;

  void Awake() {
    animator = (Animator) GetComponent(typeof(Animator));
  }

  void OnDrawGizmos() {
    // IsCollidingWithWall()
    Gizmos.color = Color.red;
    Vector2 position = wallJumpCheck.position;
    Gizmos.DrawWireSphere(new Vector3(position.x, position.y, 0), 0.2f);

  }

  /// <summary>
  /// Compute velocity every frame
  /// </summary>
  protected override void ComputeVelocity() {

    // animation state
    animator.SetFloat("moveX", speed);
    animator.SetFloat("moveY", velocity.y);
    animator.SetBool("isGrounded", isGrounded);

    // default velocity
    ComputeDefaultTargetVelocity();

    // player direction
    if (!isWallJumping) {
      Flip();
    }

    // slide agains wall
    if (!isGrounded) {
      SlideWall();
    }

    // wall jumping velocity
    if (isWallJumping) {
      WallJump();
    }
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
  /// Set player to run (by increasing speed)
  /// </summary>
  public void Run() {
    speed = runningSpeed;
  }

  /// <summary>
  /// Set the player to jump
  /// </summary>
  public void Jump() {
    if (isGrounded) {
      velocity.y = jumpForce;
    } else if (IsAbleToWallJump()) {
      isNeedToSwitchDirection = true;
      isVelocityForWallJumping = true;
      isWallJumping = true;
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
  /// Make the player do wall jumping
  /// </summary>
  public void WallJump() {

    // wall jump direction (if facing right, should wall jump to the left)
    float wallJumpLeapX = wallJumpDirectionX * wallJumpLeap.x;

    // change player direction
    if (isNeedToSwitchDirection) {
      InverseScaleX();
      isFacingRight = !isFacingRight;
      isNeedToSwitchDirection = false;
    }

    // does velocity need to be changed to wallJumpLeapY value
    if (velocity.y != wallJumpLeap.y && isVelocityForWallJumping) {
      velocity.y = wallJumpLeap.y;
      isVelocityForWallJumping = false;
    } else {
      DefaultVelocityEquation();
    }

    // new velocity for the next frame
    targetVelocity = new Vector2(wallJumpLeapX, Vector2.zero.y);

    if (isGrounded || IsCollidingWithWall()) {
      isWallJumping = false;
      wallJumpDirectionX = 0;
    }
  }

  /// <summary>
  /// Make the player slowly slide a wall if his velocity is below 0.01
  /// </summary>
  public void SlideWall() {
    animator.SetBool("isSliding", IsAbleToWallJump());

    if (animator.GetBool("isSliding")) {
      wallJumpDirectionX = isFacingRight ? -1 : 1;
      if (animator.GetFloat("moveY") < 0.01f) {
        velocity.y = Time.deltaTime;
      }
    } 
  }

  /// <summary>
  /// Does the player touch a wall to execute a wall jump ?
  /// Does the player touch the ground and a wall (and isn't already walljumping)
  /// </summary>
  /// <returns>does the wall is touched</returns>
  public bool IsAbleToWallJump() {
    return !isGrounded && IsCollidingWithWall() && !isWallJumping;
  }

  /// <summary>
  /// Flip player left/right. Based on horizontal input direction.
  /// </summary>
  public void Flip() {
    if (directionalInput.x < 0 && isFacingRight) {
      isFacingRight = false;
      InverseScaleX();
    } else if (!isFacingRight && directionalInput.x > 0) {
      isFacingRight = true;
      InverseScaleX();
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

  /// <summary>
  /// Inverse x axes of localScale
  /// </summary>
  private void InverseScaleX() {
    Vector3 scale = transform.localScale;
    scale.x *= -1;
    transform.localScale = scale;
  }

  /// <summary>
  /// Set default targetVelocity (Vector2 (directionalInput.x, Vector2.zero.y) * speed)
  /// </summary>
  private void ComputeDefaultTargetVelocity() {
    targetVelocity = new Vector2(directionalInput.x, Vector2.zero.y) * speed;
  }

  /// <summary>
  /// Does the player has collide with a wall ?
  /// </summary>
  /// <returns>bool</returns>
  private bool IsCollidingWithWall() {
    return Physics2D.OverlapCircle(wallJumpCheck.position, 0.2f, wallJumpLayer) ? true : false;
  }
}