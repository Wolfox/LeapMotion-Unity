using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Leap;
using System;
using System.Linq;
using Sequences;

public class OptionsMenuScript : MonoBehaviour {

	private enum OptionsMenuState {Main, Culture, Colors};

	public GameObject culturePanel;
	public GameObject numOfColorsPanel;
	public Transform testCube;
	
	private OptionsMenuState state;
	private Button[] buttons = {};

	private Controller controller;
	private Sample sample;
	private GestureList gestures = new GestureList();
	private Classifier classifier;
	private State culturalState;

	public void Awake() {
		sample = new Sample(50);
		Culture.StartCulture();
		culturalState = new State("Numbers");
		culturalState.AddAction("NOTHING");
		culturalState.AddAction("NUMBER_1");
		culturalState.AddAction("NUMBER_2");
		culturalState.AddAction("NUMBER_3");
		classifier = new Classifier(Culture.GetModels(culturalState), culturalState.GetActions());
		classifier.StartClassifier();
	}

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

	void FixedUpdate() {
		Frame frame = controller.Frame();

		if(state == OptionsMenuState.Colors) {
			gestures = new GestureList();
			Sign s = Sequences.Utils.FrameToSign(frame);
			sample.AddSign(s);
		} else {
			sample.ClearSequence();
			gestures = frame.Gestures();
		}
	}

	public void Update() {
		if(state == OptionsMenuState.Colors) {
			NumberGestures();
		} else {
			TapGestures();
		}

	}

	private void TapGestures() {
		/*Frame frame = controller.Frame();
		GestureList gestures = frame.Gestures();*/

		for(int i = 0; i < gestures.Count; i++) {
			Gesture gesture = gestures[i];
			Vector3 vect = new Vector3();
			if(gesture.Type == ScreenTapGesture.ClassType()) {
				Debug.Log ("screen");
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

	private void NumberGestures() {
		double[][] sequence = sample.getSequence().GetArray();
		string action = classifier.ComputeToString(sequence);
		Debug.Log(action);
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

	public void LoadMenu(int level) {
		Application.LoadLevel(level);
	}

	public void OpenOption(int option) {
		if(state == OptionsMenuState.Main) {
			switch(option) {
			case(0):
				LoadMenu(0);
				break;
			case(1):
				OpenNumOfColors();
				break;
			case(2):
				OpenCulture();
				break;
			default:
				Debug.LogError("Option " + option + " not recognized");
				break;
			}
		}
	}

	private void OpenCulture() {
		state = OptionsMenuState.Culture;
		culturePanel.SetActive(true);
		UpdateButtons();
	}

	private void OpenNumOfColors() {
		state = OptionsMenuState.Colors;
		numOfColorsPanel.SetActive(true);
	}

	public void ChangeCulture(string culture) {
		Culture.culture = culture;
		state = OptionsMenuState.Main;
		culturePanel.SetActive(false);
		UpdateButtons();
	}

	public void ChangeNumOfColors(int number) {
		state = OptionsMenuState.Main;
		numOfColorsPanel.SetActive(false);
		UpdateButtons();
	}

}
