using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
  public PlayerMovement playerMovement;

  void Awake() {
    playerMovement = (PlayerMovement) GetComponent(typeof(PlayerMovement));
  }

  void Update() {
    SetPlayerDirectionalInput(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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
  /// Control the player's movement with the axis input
  /// </summary>
  private void ControlPlayerMovement() {

    // idle
    if (!Input.anyKey) {
      playerMovement.Idle();
    }

    // moving
    if (Input.GetAxisRaw("Horizontal") != 0) {
      // running or walking
      if (Input.GetButton("Run")) {
        playerMovement.Run();
      } else {
        playerMovement.Walk();
      }
    }

    // jumping
    if (Input.GetButtonDown("Jump")) {
      if (playerMovement.IsGrounded()) {
        playerMovement.Jump();
      }
    }
  }
}