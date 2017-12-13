using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage input 
/// </summary>
public class InputController : MonoBehaviour {

  public string playerSuffix = "P1";

  private PlayerMovement playerMovement;
  private string horizontalAxes, verticalAxes, jumpButton, runButton;

  void Awake() {
    playerMovement = GetComponent<PlayerMovement>();

    horizontalAxes = "Horizontal_" + playerSuffix;
    verticalAxes = "Vertical_" + playerSuffix;
    jumpButton = "Jump_" + playerSuffix;
    runButton = "Run_" + playerSuffix;

  }

  void Update() {
    SetPlayerDirectionalInput(Input.GetAxis(horizontalAxes), Input.GetAxis(verticalAxes));
    SetPlayerRawDirectionalInput(Input.GetAxisRaw(horizontalAxes), Input.GetAxisRaw(verticalAxes));
    ControlPlayerMovement();
  }

  /// <summary>
  /// Sets the player mouvement directional input.
  /// </summary>
  /// <returns>The player directional input.</returns>
  /// <param name="horizontal">Horizontal.</param>
  /// <param name="vertical">Vertical.</param>
  private void SetPlayerDirectionalInput(float horizontal, float vertical) {
    playerMovement.SetDirectionalInput(new Vector2(horizontal, vertical));
  }

  /// <summary>
  /// Sets the player mouvement raw directional input.
  /// </summary>
  /// <returns>The player directional input.</returns>
  /// <param name="horizontal">Raw Horizontal.</param>
  /// <param name="vertical">Raw Vertical.</param>
  private void SetPlayerRawDirectionalInput(float horizontal, float vertical) {
    playerMovement.SetRawDirectionalInput(new Vector2(horizontal, vertical));
  }

  /// <summary>
  /// Control the player's movement with the axis input
  /// </summary>
  private void ControlPlayerMovement() {

    // moving
    if (Input.GetAxis(horizontalAxes) != 0) {
      // running or walking
      if (Input.GetButton(runButton)) {
        playerMovement.Run();
      } else {
        playerMovement.Walk();
      }
    } else if (playerMovement.IsGrounded()) {
      // idle
      playerMovement.Idle();
    }

    // jumping
    if (Input.GetButtonDown(jumpButton)) {
      playerMovement.Jump();
    } else if (Input.GetButtonUp(jumpButton)) {
      playerMovement.JumpTakeOff();
    }

    // faster sliding
    if (playerMovement.IsAbleToWallJump()) {
      if (Input.GetAxis(verticalAxes) < 0) {
        playerMovement.IncreaseSlidingSpeed();
      } else {
        playerMovement.ResetSlidingSpeed();
      }
    }
  }
}