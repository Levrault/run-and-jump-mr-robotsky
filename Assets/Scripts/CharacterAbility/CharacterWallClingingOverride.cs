using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine {
  /// <summary>
  /// Override corgi engine's characterWallClinging by adding a timer before the player 
  /// leave the clinging state
  /// Should improve wall jump
  /// </summary>
  public class CharacterWallClingingOverride : CharacterWallClinging {
    public float TimeBeforeExitWallClinging = 0.5f;

    // keep track of the collision
    [HideInInspector]
    public bool IsCollidingRight = false;

    /// <summary>
    /// Makes the player stick to a wall when jumping
    /// </summary>
    protected override void WallClinging() {
      base.WallClinging();

      IsCollidingRight = _controller.State.IsCollidingRight;

      if (_movement.CurrentState == CharacterStates.MovementStates.WallClinging) {
        // freeze player on clinging state
        _characterHorizontalMovement.AbilityPermitted = false;
      }
    }

    /// <summary>
    /// If the character is currently wallclinging, checks if we should exit the state
    /// </summary>
    protected override void ExitWallClinging() {
      if (_movement.CurrentState == CharacterStates.MovementStates.WallClinging) {

        bool shouldExit = false;

        if ((_controller.State.IsGrounded) || (_controller.Speed.y > 0)) {
          shouldExit = true;
        }

        Vector3 raycastOrigin = transform.position;
        Vector3 raycastDirection;
        if (_character.IsFacingRight) {
          raycastOrigin = raycastOrigin + transform.right * _controller.Width() / 2;
          raycastDirection = transform.right - transform.up;
        } else {
          raycastOrigin = raycastOrigin - transform.right * _controller.Width() / 2;
          raycastDirection = -transform.right - transform.up;
        }

        // we cast our ray 
        RaycastHit2D hit = MMDebug.RayCast(
          raycastOrigin,
          raycastDirection,
          WallClingingTolerance,
          _controller.PlatformMask | _controller.OneWayPlatformMask | _controller.MovingOneWayPlatformMask,
          Color.black,
          _controller.Parameters.DrawRaycastsGizmos
        );

        // if player change is direction to the opposite direction
        if ((_character.IsFacingRight && (_horizontalInput <= -_inputManager.Threshold.x)) ||
          !_character.IsFacingRight && (_horizontalInput >= _inputManager.Threshold.x)) {
          StartCoroutine(WaitBeforeExitClinging(TimeBeforeExitWallClinging));
        }

        // we check if the ray hit anything. If it didn't, or if we're not moving in the direction of the wall, we exit
        if (!hit || shouldExit) {
          ExitClinging();
        }
      }
    }

    /// <summary>
    /// Wait X secondes before exit clinging
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator WaitBeforeExitClinging(float time) {
      yield return new WaitForSeconds(time);
      ExitClinging();
    }

    /// <summary>
    /// Let the character exit the clinging state
    /// </summary>
    private void ExitClinging() {
      // if we're not wallclinging anymore, we reset the slowFall factor, and reset our state.
      _controller.SlowFall(0f);
      // we reset the state
      _movement.ChangeState(CharacterStates.MovementStates.Idle);

      // we play our exit sound
      StopAbilityUsedSfx();
      PlayAbilityStopSfx();

      _characterHorizontalMovement.AbilityPermitted = true;
      IsCollidingRight = false;
    }

  }
}
