using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	Transform cam;
	float cameraDistance = 0;
	float cameraDistanceMin = -100;
	float cameraDistanceMax = 100;
	public float scrollSpeed = 0.5f;
	public float speed = 1f;
	public float rotationSpeed = 10;
	// Use this for initialization
	void Start () {
		cam = this.gameObject.transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		cameraDistance = 0;
		float z = Input.GetAxis("Vertical");
		float x = Input.GetAxis("Horizontal");
		cameraDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed *-1;
     	cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
		Vector3 dir = new Vector3(x * speed + cam.position.x,  cam.position.y + cameraDistance, z * speed + cam.position.z);
		Vector3 newPos = Vector3.Lerp(cam.position,dir,Time.deltaTime);
		cam.position = newPos;

		if(Input.GetKey(KeyCode.Z)){
			cam.Rotate(0, Time.deltaTime * rotationSpeed,0);
		}
		if(Input.GetKey(KeyCode.C)){
			cam.Rotate(0, -Time.deltaTime * rotationSpeed,0);
		}
	}
}
