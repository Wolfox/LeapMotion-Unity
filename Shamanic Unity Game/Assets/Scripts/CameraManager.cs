using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public enum CameraType {FirstPerson, Global};

	public CameraType cameraType;

	public Transform player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos = new Vector3();
		newPos.x = player.position.x;
		newPos.y = transform.position.y;
		newPos.z = player.position.z;

		transform.position = newPos;
	}
}
