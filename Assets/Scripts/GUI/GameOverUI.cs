using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Behavior of the GameOverIu
/// Game Over screen appear when all player are deads
/// </summary>
public class GameOverUI : MonoBehaviour {

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
