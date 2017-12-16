using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

// enums should begin with a maj https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/enum
// but since the book don't bother to use it
public enum GameState {
  PauseMenu,
  InGame,
  GameOver,
}

/// <summary>
/// Singleton game manager. Manage Game state like
/// InGame, pauseeMenu, GameOver etc.
/// </summary>
public class GameManager : MonoBehaviour {

  public static GameManager instance;
  public GameState currentGameState = GameState.InGame;
  public Canvas pauseMenuCanvas;
  public Canvas gameOverCanvas;

  void Awake() {
    if (instance == null) {
      instance = this;
    } else if (instance != this) {
      Destroy(gameObject);
    }
  }

  void Start() {
    currentGameState = GameState.InGame;
  }

  /// <summary>
  /// InGame
  /// </summary>
  public void InGame() {
    SetGameState(GameState.InGame);
  }

  /// <summary>
  /// The game is over
  /// </summary>
  public void GameOver() {
    SetGameState(GameState.GameOver);
  }

  /// <summary>
  /// Pause Game
  /// </summary>
  public void PauseMenu() {
    SetGameState(GameState.PauseMenu);
  }


  /// <summary>
  /// Set new game state
  /// </summary>
  void SetGameState(GameState newGameState) {
    
    if (newGameState == GameState.PauseMenu) {
      // setup Unity scene for menu state
      pauseMenuCanvas.gameObject.SetActive(true);
      gameOverCanvas.gameObject.SetActive(false);
    } else if (newGameState == GameState.InGame) {
      pauseMenuCanvas.gameObject.SetActive(false);
      gameOverCanvas.gameObject.SetActive(false);
    } else if (newGameState == GameState.GameOver) {
      // setup Unity scene for menu state
      pauseMenuCanvas.gameObject.SetActive(false);
      gameOverCanvas.gameObject.SetActive(true);
    }

    currentGameState = newGameState;
  }
}
