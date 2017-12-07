using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage all the player mouvement (walk, run, jump, wall jump etc.)
/// </summary>
public class PlayerMovement : PhysicObject {
  // Player's component
  private Animator animator;
  public PlayerSound playerSound;

  // player's params
  public float walkingSpeed = 4f;
  public float runningSpeed = 7.0f;
  public float slidingSpeed = 0.1f;
  public float maxSlidingSpeed = 2f;
  public float jumpForce = 7.0f;
  public Vector2 wallJumpLeap = new Vector2(8, 12);
  public Vector2 climbWallLeap = new Vector2(12, 12);
  public Transform wallJumpCheck;
  public LayerMask wallJumpLayer;

  // private
  private Vector2 directionalInput;
  private Vector2 rawDirectionalInput;
  private bool isFacingRight = true;
  private float speed = 0f;
  private bool isWallJumping = false;
  private bool isWallClimbing = false;
  private const int wallJumpFrame = 4;
  private const int climbJumpFrame = 5;
  private int wallJumpFrameCounter;
  private bool isWallJumpingTakeOff = false;
  private bool isVelocityForWallJumping = false;
  private bool isNeedToSwitchDirection = false;
  private int wallJumpDirectionX;
  private float defaultSlidingSpeed;
  private const int unstickFrame = 20;
  private int unstickFrameCounter;
  private bool isStickToWall = false;

  void Awake() {
    animator = (Animator) GetComponent(typeof(Animator));
    playerSound = (PlayerSound) GetComponent(typeof(PlayerSound));
    defaultSlidingSpeed = slidingSpeed;
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

    // stick to the wall delay
    if (animator.GetBool("isSliding") && isStickToWall) {
      UnstickFromWallTimer();
    } else {
      // wall jumping velocity
      if (isWallJumping) {
        if (isWallClimbing) {
          ClimbWall();
        } else {
          WallJump();
        }
      } else {
        // player direction
        Flip();

        // default velocity
        ComputeDefaultTargetVelocity();
      }

    }

    // slide agains wall
    if (IsAbleToWallJump()) {
      SlideWall();
    } else {
      animator.SetBool("isSliding", false);
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
      playerSound.PlayJumpAudioClip();
      velocity.y = jumpForce;
    } else if (IsAbleToWallJump()) {

      // player will be flipped
      isNeedToSwitchDirection = true;

      // use to change velocity on the first frame of the jump
      isVelocityForWallJumping = true;

      isWallJumping = true;

      // does the player just want to leave the wall without make a long wall jump
      isWallJumpingTakeOff = (rawDirectionalInput.x == 0);

      // jump on the same wall
      if (rawDirectionalInput.x == (wallJumpDirectionX * -1)) {
        isWallClimbing = true;
        wallJumpFrameCounter = climbJumpFrame;
      } else {
        isWallClimbing = false;
        wallJumpFrameCounter = wallJumpFrame;
      }
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
    Vector2 leap = isWallJumpingTakeOff ? (wallJumpLeap / 2) : wallJumpLeap;
    float wallJumpLeapX = wallJumpDirectionX * leap.x;

    // inverse direction
    if (isNeedToSwitchDirection && !isWallClimbing) {
      isNeedToSwitchDirection = false;
      playerSound.PlayJumpAudioClip();
      InverseScaleX();
      isFacingRight = !isFacingRight;
    }

    // does velocity need to be changed to wallJumpLeapY value
    if (wallJumpFrameCounter > 0) {
      wallJumpFrameCounter--;
      velocity.y = wallJumpLeap.y;
    } else {
      DefaultVelocityEquation();
    }

    // new velocity for the next frame
    targetVelocity = new Vector2(wallJumpLeapX, Vector2.zero.y);

    // wall jumping is over
    if (isGrounded || IsCollidingWithWall()) {
      wallJumpFrameCounter = wallJumpFrame;
      isWallJumping = false;
      wallJumpDirectionX = 0;
    }
  }

  /// <summary>
  /// Let the player climb a wall
  /// </summary>
  public void ClimbWall() {

    // wall jump direction (if facing right, should wall jump to the left)
    float climbLeapX = wallJumpDirectionX * climbWallLeap.x;
    float climbLeapY = climbWallLeap.y;
    bool hasStopClimbing = false;

    if (wallJumpFrameCounter > 0) {
      wallJumpFrameCounter--;
      velocity.y = climbLeapY;
      targetVelocity = new Vector2(climbLeapX, Vector2.zero.y);
      Debug.Log(targetVelocity);
    } else {
      targetVelocity = new Vector2(-climbLeapX, Vector2.zero.y);
      hasStopClimbing = IsCollidingWithWall();
    }

    // wall jumping is over
    if (isGrounded || hasStopClimbing) {
      wallJumpFrameCounter = wallJumpFrame;
      isWallJumping = false;
      isWallClimbing = false;
      wallJumpDirectionX = 0;
    }
  }

  /// <summary>
  /// Make the player slowly slide a wall if his velocity is below 0.01
  /// </summary>
  public void SlideWall() {

    // player can be "stick" to the wall
    if (!animator.GetBool("isSliding")) {
      isStickToWall = true;
      unstickFrameCounter = unstickFrame;
    }

    animator.SetBool("isSliding", true);
    wallJumpDirectionX = isFacingRight ? -1 : 1;

    // make player slide
    if (animator.GetFloat("moveY") < 0.01f) {
      velocity.y = slidingSpeed * -1;
    }
  }

  /// <summary>
  /// Increase player's sliding speed 
  /// </summary>
  /// <param name="newSpeed"></param>
  public void IncreaseSlidingSpeed() {
    slidingSpeed = maxSlidingSpeed;
  }

  /// <summary>
  /// Reset default sliding speed
  /// </summary>
  public void ResetSlidingSpeed() {
    slidingSpeed = defaultSlidingSpeed;
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
  /// Sets the directional input.
  /// </summary>
  /// <returns>The directional input.</returns>
  /// <param name="input">input.</param>
  public void SetRawDirectionalInput(Vector2 input) {
    rawDirectionalInput = input;
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

  /// <summary>
  /// Stick player for x frames to know if he want to be unstick
  /// from the wall or take his direction for a wall jump
  /// </summary>
  private void UnstickFromWallTimer() {
    if (rawDirectionalInput.x == wallJumpDirectionX) {
      if (unstickFrameCounter > 0) {
        velocity.x = 0;
        unstickFrameCounter--;
      } else {
        unstickFrameCounter = unstickFrame;
        isStickToWall = false;
      }
    }
  }
}