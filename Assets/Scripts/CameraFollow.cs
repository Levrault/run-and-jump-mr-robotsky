using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Like in old super mario bros game level, the camera
/// will always goes to the right. We can also activate a
/// test mode where the camera follow the player 
/// </summary>
public class CameraFollow : MonoBehaviour {

  // Distance between player and camera in horizontal direction
  public float xOffset = 0f;

  // Distance between player and camera in vertical direction
  public float yOffset = 0f;

  // follow the player if debugmode is on
  public bool debugMode = false;

  // how fast the camera goes to the right
  public float speed = 2f;

  // Reference to the player's transform.
  public Transform player;

  void Update() {
    if (!debugMode) {
      InGameMode();
    }
  }

  void LateUpdate() {
    if (debugMode) {
      DebugMode();
    }
  }

  /// <summary>
  /// Follow player
  /// </summary>
  private void DebugMode() {
    float positionX = player.transform.position.x + xOffset;
    float positionY = transform.position.y + yOffset;
    transform.position = new Vector3(positionX, positionY, -10);
  }

  /// <summary>
  /// Always goes to the right
  /// </summary>
  private void InGameMode() {
    transform.Translate(Vector3.right * Time.deltaTime * speed, Camera.main.transform);
  }
}
