using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

/// <summary>
/// Paralax effect (thanks to brackeys)
/// https://www.youtube.com/watch?v=5E5_Fquw7BM
/// </summary>
public class Parallaxing:MonoBehaviour {

	public Transform[] backgrounds; 
	public float smoothing = 1f; 

	private float[] scales; 
	private Transform cam; 
	private Vector3 previousCamPosition; 

	void Awake() {
		cam = Camera.main.transform; 
	}

	void Start () {
		previousCamPosition = cam.position; 
		scales = new float[backgrounds.Length]; 	

		for (int i = 0; i < backgrounds.Length; i++) {
			scales[i] = backgrounds[i].position.z * -1; 
		}
	}
	
	void Update () {

		for (int i = 0; i < backgrounds.Length; i++) {
			// the parallax is the opposite of the camera movement because the previous frame * scale
			float parallax = (previousCamPosition.x - cam.position.x) * scales[i];

			// set a target x position witch is the current position plus the parallax
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			// create a target position witch is the background current position with it's target x position
			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			// fade between current position and the target position using lerp
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		// set the previousCamPos to the camera's position at the end of the frame
		previousCamPosition = cam.position;
		
	}
}
