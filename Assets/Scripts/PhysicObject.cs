using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PhysicObject based on unity's live session
/// https://goo.gl/L1ZQBh
/// </summary>
public class PhysicObject : MonoBehaviour {

  // allow to scale the gravity
  public float gravityModifier = 1f;

  // use to determine if the player is ground by comparing to all raycastHit2D normal
  public float minGroundNormalY = 0.65f;

  // does the object is grounded
  protected bool isGrounded;

  // set padding to never get stock on an another collider
  protected const float shellRadius = 0.01f;

  // minimal distance to check for collision
  protected const float minMoveDistance = 0.001f;

  // actual normal 
  protected Vector2 groundNormal;

  // incoming input from outside the class as where our object
  // is trying to move
  protected Vector2 targetVelocity;

  protected Vector2 velocity;

  protected Rigidbody2D rb2d;

  // precisely control which contact results get returned
  protected ContactFilter2D contactFilter;

  // store layers that are being detected on collision
  protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

  // use to copy the result store in hitBuffer array
  protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

  void OnEnable() {
    rb2d = (Rigidbody2D) GetComponent(typeof(Rigidbody2D));
  }

  // Use this for initialization
  void Start() {
    // doesn't get filter contact based on trigger collider
    contactFilter.useTriggers = false;

    // sets the layerMask filter property using the layerMask parameter provided
    // Physics2D get layer mask from ou project physics2D settings
    contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));

    // enables layer mask filtering
    contactFilter.useLayerMask = true;
  }

  // Update is called once per frame
  void Update() {
    targetVelocity = Vector2.zero;
    ComputeVelocity();
  }

  /// <summary>
  /// Calculate gravity/velocity to move our object
  /// </summary>
  void FixedUpdate() {

    // use default gravity with our custom gravity with the time since the last frame
    DefaultVelocityEquation();

    // add incoming value to velocity
    velocity.x = targetVelocity.x;

    // we alway set grounded to false until a collision is registered 
    isGrounded = false;

    // calculate a new position for our object
    Vector2 deltaPosition = velocity * Time.deltaTime;

    // store the direction where the object is trying to move along the ground
    // @see https://youtu.be/FwVdfCz5r2I?t=2m41s
    // allow us to get perpendicular vector to the original, and move smoothly
    // along the slope and don't move into the slope
    Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

    // move the object on the X axes
    Vector2 move = moveAlongGround * deltaPosition.x;

    // test x first because this way, it's handle better slope
    Movement(move, false);

    // move the object on the Y axes
    move = Vector2.up * deltaPosition.y;
    Movement(move, true);
  }

  /// <summary>
  /// Empty function. Use in children to set input
  /// </summary>
  protected virtual void ComputeVelocity() { }

  /// <summary>
  /// Reset velocity with the default operation
  /// </summary>
  protected void DefaultVelocityEquation() {
    velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
  }

  /// <summary>
  /// Applying movement/collision to our object
  /// </summary>
  /// <param name="move">new position</param>
  /// <param name="yMovement">Are we moving on the Y axes ?</param>
  void Movement(Vector2 move, bool yMovement) {

    // get the length of our vector move
    float distance = move.magnitude;

    // we need to check if distance is greater than a minimal value to prevent our object
    // to constantly checking collision when they're standing on a surface
    if (distance > minMoveDistance) {

      // does any of the attached colliders of our rigidbody are going to overlap with anything
      // in the next frame (distance + shellRadius) 
      // cast function https://docs.unity3d.com/ScriptReference/Rigidbody2D.Cast.html
      // cast function will return the number of contacts we have made (in the next frame)
      int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

      // clear result of the previous frame
      hitBufferList.Clear();

      // copy data of hitBuffer to hitBufferList
      for (int i = 0; i < count; i++) {
        hitBufferList.Add(hitBuffer[i]);
      }

      // check normal of each raycasthit2d to determine the angle of the things we are 
      // colliding with
      // we compare normal to a minimum ground normal value
      // normal (https://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html) 
      // are perpendicular vector
      for (int i = 0; i < hitBufferList.Count; i++) {

        Vector2 currentNormal = hitBufferList[i].normal;

        // compare current normal to a minimum value
        // does the player is grounded or not ?
        // if the angle of the object that our object is going to collide 
        // with means it would be consider a piece of ground
        // e.g. if a player has jumped toward a vertical wall and he is 
        // colliding with it, the generated angle with us to not consider the collision
        // has a piece of ground
        // We make sure that the player is considered grounded when the normal is at a certains angles
        // It will also not let the player slide slope unless the player touch the horizontal axis input
        // see https://youtu.be/eOqEHOhywOg?t=5m24s
        if (currentNormal.y > minGroundNormalY) {

          // since normal's angle is ok, we consider the player grounded
          isGrounded = true;

          // are we moving on the y axes
          if (yMovement) {
            groundNormal = currentNormal;
            currentNormal.x = 0;
          }
        }

        // getting the difference between the velocity and the current normal and determining
        // whether we need to substact from our velocity to prevent the player from entering
        // into another collider and stop his velocity (like jumping and hit his head on a plateform)
        // and have the player fall straigth down. We also want them to hit the selling and scrape
        // their head along the ceiling and continuing their horizontal movement
        float projection = Vector2.Dot(velocity, currentNormal);

        // cancel out the part of our velocity that would be stop by the collision if projection is 
        // negative
        if (projection < 0) {
          velocity = velocity - projection * currentNormal;
        }

        // does the collision in our list is less than our distance (move.magnitude)
        // it will prevent us of getting stock in another collider
        float modifiedDistance = hitBufferList[i].distance - shellRadius;
        distance = modifiedDistance < distance ? modifiedDistance : distance;
      }
    }

    // new position taking in account our modified velocity based on our collision projection
    rb2d.position = rb2d.position + move.normalized * distance;
  }
}
