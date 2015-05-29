using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Leap;

public class MainMenuScript : MonoBehaviour {

	public GameObject LoadingScene;
	public Transform testCube;

	private Controller controller;
	private Button[] buttons = {};

	public void Start() {
		controller = new Controller();
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
		controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
		
		controller.Config.SetFloat("Gesture.ScreenTap.MinForwardVelocity", 30.0f);
		controller.Config.SetFloat("Gesture.ScreenTap.HistorySeconds", .5f);
		controller.Config.SetFloat("Gesture.ScreenTap.MinDistance", 1.0f);
		
		controller.Config.SetFloat("Gesture.KeyTap.MinDownVelocity", 40.0f);
		controller.Config.SetFloat("Gesture.KeyTap.HistorySeconds", .2f);
		controller.Config.SetFloat("Gesture.KeyTap.MinDistance", 1.0f);
		controller.Config.Save();
		
		UpdateButtons();
	}

	public void UpdateButtons() {
		buttons = GetComponentsInChildren<Button>();
	}

	public void Update() {
		Frame frame = controller.Frame();
		GestureList gestures = frame.Gestures();

		for(int i = 0; i < gestures.Count; i++) {
			Gesture gesture = gestures[i];
			Vector3 vect = new Vector3();
			if(gesture.Type == ScreenTapGesture.ClassType()) {
				Debug.Log ("tap");
				ScreenTapGesture screentapGesture = new ScreenTapGesture(gesture);
				vect = screentapGesture.Position.ToUnityScaled();
			}
			if(gesture.Type == KeyTapGesture.ClassType()) {
				Debug.Log ("key");
				KeyTapGesture screentapGesture = new KeyTapGesture(gesture);
				vect = screentapGesture.Position.ToUnityScaled();
			}

			vect *= 20;
			vect -= new Vector3(0,5,3);

			foreach(Button button in buttons) {
				Vector3[] corners = new Vector3 [4];
				RectTransform rectTrans = button.gameObject.GetComponent<RectTransform>();
				rectTrans.GetWorldCorners(corners);
				if(ContainInWorld(corners, vect)) {
					button.onClick.Invoke();
				}
			}
		}
	}

	private bool ContainInWorld(Vector3[] corners, Vector3 point) {
		Vector3[] cornersInCamera = Array.ConvertAll(corners, element => Camera.main.WorldToViewportPoint(element));
		Vector3 pointInCamera = Camera.main.WorldToViewportPoint(point); 

				testCube.position = point;
		return Contain(cornersInCamera[0], cornersInCamera[2], pointInCamera);
	}

	private bool Contain(Vector2 downLeft, Vector2 topRight, Vector2 pos){
		return (pos.x >= downLeft.x && pos.x <= topRight.x &&
		        pos.y >= downLeft.y && pos.y <= topRight.y);
	}

	public void LoadGame(int level) {
		LoadingScene.SetActive(true);
		Application.LoadLevel(level);
	}

	public void LoadMenu(int level) {
		Application.LoadLevel(level);
	}

	public void Quit() {
		Application.Quit();
	}
}
