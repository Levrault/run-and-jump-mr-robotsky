using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Explosion effect
/// Can hurt player nearby
/// </summary>
public class Explosion : MonoBehaviour {

  public GameObject explosionGameObject;
  private ExplosionSound explosionSound;

  void Awake() {
    explosionSound = GetComponent<ExplosionSound>();
    explosionGameObject.SetActive(false);
  }

  /// <summary>
  /// KA-BOOM!
  /// Active explosion object and trigger explosion state 
  /// </summary>
  public void Explode() {
    explosionGameObject.SetActive(true);
    explosionSound.PlayExplosionAudioClip();
    explosionGameObject.GetComponent<Animator>().SetTrigger("isTriggered");
  }
}
