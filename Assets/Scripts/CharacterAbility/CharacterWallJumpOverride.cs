using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine 
{
  /// <summary>
  /// Override corgi engine's characterWalljump to work with CharacterWallClingingOverride
  /// </summary>
  public class CharacterWallJumpOverride : CharacterWalljump {
    private CharacterHorizontalMovement _characterHorizontalMovement;
    private CharacterWallClinging _characterWallClinging;

    /// <summary>
    /// On start, we store our characterJump component
    /// </summary>
    protected override void Initialization() {
      base.Initialization();
      _characterHorizontalMovement = GetComponent<CharacterHorizontalMovement>();
      _characterWallClinging = GetComponent<CharacterWallClinging>();
    }

    /// <summary>
    /// Every frame, we chack if we're pressing the jump button
    /// </summary>
    protected override void HandleInput() {
      if (_inputManager.JumpButton.State.CurrentState == MMInput.ButtonStates.ButtonDown) {
        _characterHorizontalMovement.AbilityPermitted = true;
        Walljump();
      }
    }

    /// <summary>
    /// Performs a walljump if the conditions are met
    /// </summary>
    protected override void Walljump() {
      if (!AbilityPermitted
        || _condition.CurrentState != CharacterStates.CharacterConditions.Normal) {
        return;
      }

      // wall jump
      float wallJumpDirection;

      // if we're here the jump button has been pressed. If we were wallclinging, we walljump
      if (_movement.CurrentState == CharacterStates.MovementStates.WallClinging) {
        _movement.ChangeState(CharacterStates.MovementStates.WallJumping);

        // we decrease the number of jumps left
        if (_characterJump != null) {
          _characterJump.SetNumberOfJumpsLeft(_characterJump.NumberOfJumpsLeft - 1);
          _characterJump.SetJumpFlags();
          // we start our sounds
          PlayAbilityStartSfx();
        }

        _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
        _controller.GravityActive(true);
        _controller.SlowFall(0f);

        // If the character is colliding to the right with something (probably the wall)
        if (_character.IsFacingRight) {
          wallJumpDirection = -1f;
        } else {
          wallJumpDirection = 1f;
        }

        Vector2 walljumpVector = new Vector2(
          wallJumpDirection * WallJumpForce.x,
          Mathf.Sqrt(2f * WallJumpForce.y * Mathf.Abs(_controller.Parameters.Gravity))
        );

        _controller.AddForce(walljumpVector);
        return;
      }
    }
  }
}
