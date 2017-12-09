using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LandMines script behavior. Landmines will
/// detect player and explode after X times
/// </summary>
public class Landmines : MonoBehaviour {

  // Landmine's params
  public float timeBeforeExplosion = 1f;
  public LandminesSound landminesSound;

  // Landmine's component
  private Animator animator;
  private float delaySound = .5f;

  void Start() {
    animator = (Animator) GetComponent(typeof(Animator));
    landminesSound = (LandminesSound) GetComponent(typeof(LandminesSound));
  }

  /// <summary>
  /// Sent when another object enters a trigger collider attached to this
  /// object (2D physics only).
  /// </summary>
  /// <param name="other">The other Collider2D involved in this collision.</param>
  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      StartCoroutine(Explode(timeBeforeExplosion));
      StartCoroutine(ExplosionSound(timeBeforeExplosion+ delaySound));
    }
  }

  /// <summary>
  /// LandMine explode
  /// </summary>
  /// <param name="time"></param>
  /// <returns></returns>
  IEnumerator Explode(float time) {
    yield return new WaitForSeconds(time);
    animator.SetTrigger("triggerExplosion");
  }

  /// <summary>
  /// LandMine explosion sound
  /// </summary>
  /// <param name="time"></param>
  /// <returns></returns>
  IEnumerator ExplosionSound(float time) {
    yield return new WaitForSeconds(time);
    landminesSound.PlayExplosionAudioClip();
  }
}
