using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Explosion effect
/// Can hurt player nearby
/// </summary>
public class Explosion : MonoBehaviour {

  public GameObject explosionGameObject;
  public AudioClip explosionSound;

  void Awake() {
    explosionGameObject.SetActive(false);
  }

  /// <summary>
  /// KA-BOOM!
  /// Active explosion object and trigger explosion state 
  /// </summary>
  public void Explode() {
    explosionGameObject.SetActive(true);
		SoundManager.instance.PlaySingle(explosionSound);
    explosionGameObject.GetComponent<Animator>().SetTrigger("isTriggered");
  }
}
