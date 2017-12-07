using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Like in old super mario bros game level, the camera
/// will always goes to the right. Camera can also follow
/// a target is followTarget mode is activated 
/// </summary>
public class CameraFollow : MonoBehaviour {

  // Distance between player and camera in horizontal direction
  public Vector3 offset = Vector2.zero;

  // dampening effect
  public float smoothing = 2f;

  // follow a target 
  public bool isFollowingTarget = false;

  // how fast the camera goes to the right
  public float speed = 2f;

  // Reference to the target's transform.
  public Transform target;

  private float lowY;

  void Start() {
    offset = transform.position + target.position;
    lowY = transform.position.y;
  }

  void Update() {
    if (!isFollowingTarget) {
      AutomaticMode();
    }
  }

  void LateUpdate() {
    if (isFollowingTarget) {
      FollowMode();
    }
  }

  /// <summary>
  /// Follow player
  /// </summary>
  private void FollowMode() {
    Vector3 targetCameraPosition = target.position + offset;
    Vector3 CameraPosition = new Vector3(transform.position.x, lowY, transform.position.y);
    transform.position = Vector3.Lerp(CameraPosition, targetCameraPosition, smoothing * Time.deltaTime);

    if (transform.position.y != lowY) {
      transform.position = new Vector3(transform.position.x, lowY, transform.position.z);
    }
  }

  /// <summary>
  /// Always goes to the right
  /// </summary>
  private void AutomaticMode() {
    transform.Translate(Vector3.right * Time.deltaTime * speed, Camera.main.transform);
  }
}
