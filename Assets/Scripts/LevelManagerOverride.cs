using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using MoreMountains.MMInterface;

namespace MoreMountains.CorgiEngine {
  public class LevelManagerOverride : LevelManager {
		[Header("GameOver Settings")]
    public GameObject gameOverGUI;

    protected override IEnumerator SoloModeRestart() {
      if (PlayerPrefabs.Count() <= 0) {
        yield break;
      }

      if (LevelCameraController != null) {
        LevelCameraController.FollowsPlayer = false;
      }

      yield return new WaitForSeconds(RespawnDelay);

      if (LevelCameraController != null) {
        LevelCameraController.FollowsPlayer = true;
      }

      //if (CurrentCheckPoint != null) {
        //CurrentCheckPoint.SpawnPlayer(Players[0]);
      //}

      _started = DateTime.UtcNow;

      gameOverGUI.SetActive(true);
      // we send a new points event for the GameManager to catch (and other classes that may listen to it too)
      //MMEventManager.TriggerEvent(new CorgiEnginePointsEvent(PointsMethods.Set, _savedPoints));
      // we trigger a respawn event
      //MMEventManager.TriggerEvent(new CorgiEngineEvent(CorgiEngineEventTypes.Respawn));
    }
  }
}
