using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Behavior of the GameOverIu
/// Game Over screen appear when all player are deads
/// </summary>
public class GameOverUI : MonoBehaviour {

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

  /// <summary>
  /// Restart level
  /// </summary>
  public void RestartLevel() {
    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
  }

  /// <summary>
  /// Quit to main menu
  /// @TODO: add main menu
  /// </summary>
  public void QuitLevel() {
    Debug.Log("QuiLevel");
  }
}
