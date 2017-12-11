﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage input 
/// </summary>
public class InputController : MonoBehaviour {
  public PlayerMovement playerMovement;

  void Awake() {
    playerMovement = (PlayerMovement) GetComponent(typeof(PlayerMovement));
  }

  void Update() {
    SetPlayerDirectionalInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    SetPlayerRawDirectionalInput(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    ControlPlayerMovement();
  }

  /// <summary>
  /// Sets the player mouvement directional input.
  /// </summary>
  /// <returns>The player directional input.</returns>
  /// <param name="horizontal">Horizontal.</param>
  /// <param name="vertical">Vertical.</param>
  void SetPlayerDirectionalInput(float horizontal, float vertical) {
    playerMovement.SetDirectionalInput(new Vector2(horizontal, vertical));
  }

  /// <summary>
  /// Sets the player mouvement raw directional input.
  /// </summary>
  /// <returns>The player directional input.</returns>
  /// <param name="horizontal">Raw Horizontal.</param>
  /// <param name="vertical">Raw Vertical.</param>
  void SetPlayerRawDirectionalInput(float horizontal, float vertical) {
    playerMovement.SetRawDirectionalInput(new Vector2(horizontal, vertical));
  }

  /// <summary>
  /// Control the player's movement with the axis input
  /// </summary>
  void ControlPlayerMovement() {

    // moving
    if (Input.GetAxis("Horizontal") != 0) {
      // running or walking
      if (Input.GetButton("Run")) {
        playerMovement.Run();
      } else {
        playerMovement.Walk();
      }
    } else if (playerMovement.IsGrounded()) {
      // idle
      playerMovement.Idle();
    }

    // jumping
    if (Input.GetButtonDown("Jump")) {
      playerMovement.Jump();
    } else if (Input.GetButtonUp("Jump")) {
      playerMovement.JumpTakeOff();
    }

    // faster sliding
    if (playerMovement.IsAbleToWallJump()) {
      if (Input.GetAxis("Vertical") < 0) {
        playerMovement.IncreaseSlidingSpeed();
      } else {
        playerMovement.ResetSlidingSpeed();
      }
    }
  }
}