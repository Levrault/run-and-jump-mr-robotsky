using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine {
  /// <summary>
  /// Coin manager
  /// </summary>
  [AddComponentMenu("Corgi Engine/Items/Collectable")]
  public class Collectable : PickableItem {
    /// The amount of points to add when collected
    public int PointsToAdd = 1000;
    private Animator _animator;

    protected override void Start() {
      base.Start();
      _animator = gameObject.GetComponent<Animator>();
		}

    /// <summary>
    /// Triggered when something collides with the coin
    /// </summary>
    /// <param name="collider">Other.</param>
    protected override void Pick() {
      // we send a new points event for the GameManager to catch (and other classes that may listen to it too)
      MMEventManager.TriggerEvent(new CorgiEnginePointsEvent(PointsMethods.Add, PointsToAdd));
      _animator.SetTrigger("isCollected");
    }
  }
}