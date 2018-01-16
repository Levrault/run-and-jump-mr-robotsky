using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make player explode when GameOver screen appear
/// </summary>
public class PlayerGameOverUI : MonoBehaviour {
  public GameObject explosion;
  public GameObject player;
  public float timeBeforeExplosion = 1f;

  void OnEnable() {
    if (explosion != null) {
      StartCoroutine(Explode(timeBeforeExplosion));
    }
  }

  /// <summary>
  /// Show player explode
  /// </summary>
  /// <param name="time"></param>
  /// <returns></returns>
  IEnumerator Explode(float time) {
    yield return new WaitForSeconds(time);
    player.SetActive(false);
    explosion.SetActive(true);
    explosion.GetComponent<Animator>().SetTrigger("triggerExplosion");
  }
}
