using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Leap;
using Sequences;
using System.Collections.Generic;

public class MainMenuScript : MonoBehaviour {

	private enum MainMenuState {Main, Start, Loading, Quitting};
	private MainMenuState state;

	public GameObject loadingScene;
	public GameObject startGameScene;
	public Text numOfColors;
	public Text culture;
	public Transform testCube;

	public HandController controller;
	private Button[] buttons = {};
	private Classifier classifier;

	public void Awake() {
		Game.StartCulture();
		classifier = Game.GetClassifier(Game.StartGameState());
	}

	public void Start() {
		state = MainMenuState.Main;
		controller.EnableTapGestures();		
		UpdateButtons();
	}

	public void UpdateButtons() {
		buttons = GetComponentsInChildren<Button>();
	}

	public void UpdateStartGameScene() {
		numOfColors.text = Game.numberOfColors.ToString();
		if(Game.culture == "") {
			culture.text = "None";
		} else {
			culture.text = Game.culture;
		}
	}

	public void Update() {
		if(state == MainMenuState.Start && Game.culture != "") {
			YesNoGestures();
		}
		TapGestures();
	}

	private void TapGestures() {
		GestureList gestures = controller.GetTapGestures();

		for(int i = 0; i < gestures.Count; i++) {
			Gesture gesture = gestures[i];
			Vector3 vect = new Vector3();
			if(gesture.Type == ScreenTapGesture.ClassType()) {
				//Debug.Log ("tap");
				ScreenTapGesture screentapGesture = new ScreenTapGesture(gesture);
				vect = screentapGesture.Position.ToUnityScaled();
			}
			if(gesture.Type == KeyTapGesture.ClassType()) {
				//Debug.Log ("key");
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

	private void YesNoGestures() {
		string action = "";
		List<string> allActions = controller.GetGestures(classifier);
		List<string> actions = Game.UpdateActions(allActions);

		foreach(string atc in allActions) {
			Debug.Log (atc);
		}

		if(actions.Count == 1) {
			action = actions[0];
		}
		
		switch(action) {
		case "YES":
			LoadGame();
			break;
		case "NO":
			BackToMainMenu();
			break;
		case "NOTHING":
		default:
			break;
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

	public void LoadOption(int i) {
		if(state == MainMenuState.Main) {
			switch(i) {
			case 0:
				StartGame ();
				break;
			case 1:
				LoadOptionsMenu();
				break;
			case 2:
				Quit();
				break;
			default:
				break;
			}
		}
	}

	private void StartGame () {
		state = MainMenuState.Start;
		startGameScene.SetActive(true);
		startGameScene.GetComponent<StartGamePanelScript>().UpdateYesNoInterface();
		UpdateStartGameScene();
		UpdateButtons();
	}

	public void BackToMainMenu() {
		state = MainMenuState.Main;
		startGameScene.SetActive(false);
		UpdateButtons();
	}

	public void LoadGame () {
		state = MainMenuState.Loading;
		startGameScene.SetActive(false);
		loadingScene.SetActive(true);
		Application.LoadLevel(2);
	}
	
	private void LoadOptionsMenu() {
		Application.LoadLevel(1);
	}

	private void Quit() {
		state = MainMenuState.Quitting;
		Application.Quit();
	}
}
