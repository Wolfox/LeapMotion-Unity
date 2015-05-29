using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Distributions.Multivariate;
using System.Collections.Generic;
using Sequences;
using Leap;
//using HMM_Test_Library;

public class CharacterControllerScript : MonoBehaviour {

	static public bool isPaused = false;

	public GameObject PlayerMesh;
	public GUIScript guiScript;
	public HandController controller;

	public Material RedMaterial;
	public Material GreenMaterial;
	public Material BlueMaterial;
	public Material NeutralMaterial;
	
	private LevelManager.GameColors potionColor;
	private bool onExit;
	private bool onPotion;

	private float horzInput;
	private float vertInput;

	private Sample sample;
	private Classifier classifier;
	private string action = "";
	private State gameState;

	void Awake() {
		sample = new Sample(50);
		Culture.StartCulture();
		StartState();
	}

	// Use this for initialization
	void Start() {
		PlayerMesh.GetComponent<Renderer>().material = NeutralMaterial;
		onExit = false;
		onPotion = false;
		potionColor = LevelManager.GameColors.Neutral;
	}

	void StartStateGame() {
		gameState = new State("Game State");
		gameState.AddAction("NOTHING");
		gameState.AddAction("FRONT");
		gameState.AddAction("RIGHT");
		gameState.AddAction("LEFT");
		gameState.AddAction("BACK");
		//gameState.AddAction("DRINK");
		//gameState.AddAction("GRAB");
		gameState.AddAction("MUTE");
		//gameState.AddAction("UNMUTE");
		gameState.AddAction("PAUSE");
		StartClassifier();
	}

	void StartStateTest() {
		gameState = new State("Test State");
		gameState.AddAction("NOTHING");
		gameState.AddAction("TEST1");
		gameState.AddAction("TEST2");
		gameState.AddAction("TEST3");
		gameState.AddAction("TEST4");
		gameState.AddAction("TEST5");
		gameState.AddAction("TEST6");
		StartClassifier();
	}

	void StartStatePause() {
		gameState = new State("Test State");
		gameState.AddAction("NOTHING");
		gameState.AddAction("RESUME");
		gameState.AddAction("MUTE");
		gameState.AddAction("UNMUTE");
		StartClassifier();
	}

	void StartStateNumber() {
		gameState = new State("Number State");
		gameState.AddAction("NOTHING");
		gameState.AddAction("NUMBER_1");
		gameState.AddAction("NUMBER_2");
		gameState.AddAction("NUMBER_3");
		StartClassifier();
	}

	void StartState() {
		StartStateGame();
	}

	void StartClassifier() {
		classifier = new Classifier(Culture.GetModels(gameState), gameState.GetActions());
		classifier.StartClassifier();
	}

	HiddenMarkovModel<MultivariateNormalDistribution> HelpLoad(string path) {
		return HiddenMarkovModel<MultivariateNormalDistribution>.Load(path);
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown ("Grab") && onExit) {
			Grab();
		}
		if(Input.GetButtonDown("Drink") && onPotion) {
			Drink();
		}
		if(Input.GetButtonDown("Pause")) {
			Pause();
		}
	}

	void SaveFrame() {
		Frame frame = controller.GetFrame();
		Sign s = Sequences.Utils.FrameToSign(frame);
		sample.AddSign(s);
	}

	void FixedUpdate() {

		vertInput = 0;
		horzInput = 0;
		
		SaveFrame();

		CheckFrame();

		float keyH = Input.GetAxis("Horizontal");
		float keyV = Input.GetAxis("Vertical");

		if(keyH != 0) {horzInput = keyH;}
		if(keyV != 0) {vertInput = keyV;}

		MoveAndRotate();
	}

	void CheckFrame() {
		double[][] sequence = sample.getSequence().GetArray();

		/*List<string> actions = gameState.GetActions();
		for(int i = 0; i < actions.Count; i++) {
			Debug.Log (actions[i]+ "(" + i +") - " + classifier.testModel(i, sequence));
		}*/

		//classifier.testModel(0,sequence);

		action = classifier.ComputeToString(sequence);

		Debug.Log(action);
		
		/*switch(action) {
		case "NOTHING":
			break;
		case "FRONT":
			vertInput = 1;
			break;
		case "BACK":
			vertInput = -1;
			break;
		case "RIGHT":
			horzInput = 1;
			break;
		case "LEFT":
			horzInput = -1;
			break;
		case "PAUSE":
			Pause();
			break;
		}*/
	}

	void OnTriggerEnter (Collider other) {
		switch(other.tag) {
			case "Potion":
				onPotion = true;
				potionColor = ((Potion)other.GetComponent(typeof(Potion))).color;
				break;
			case "Finish":
				onExit = true;
				break;
			default:
				break;
		}
	}

	void OnTriggerExist (Collider other) {
		switch(other.tag) {
			case "Potion":
				onPotion = false;
				potionColor = LevelManager.GameColors.Neutral;
				break;
			case "Finish":
				onExit = false;
				break;
			default:
				break;
		}
	}

	void MoveAndRotate() {
		if(vertInput != 0) { Move (vertInput); }

		if(horzInput != 0) {  Rotate(horzInput); }
	}

	void Move (float value) {
		if(isPaused) {return;}

		Vector3 target = transform.position + transform.forward*value;
		target.y = transform.position.y;
		transform.position = Vector3.MoveTowards(transform.position, target, 0.1f);
	}

	void Rotate (float value) {
		if(isPaused) {return;}
		transform.RotateAround(transform.position, Vector3.up, value*3);
	}

	void Grab() {
		Application.LoadLevel(0);
	}

	void Drink() {
		switch(potionColor) {
		case LevelManager.GameColors.Red:
			gameObject.layer = LayerMask.NameToLayer("Red");
			guiScript.ChangeText("RED");
			guiScript.ChangeColor(Color.red);
			break;
		case LevelManager.GameColors.Green:
			gameObject.layer = LayerMask.NameToLayer("Green");
			guiScript.ChangeText("GREEN");
			guiScript.ChangeColor(Color.green);
			break;
		case LevelManager.GameColors.Blue:
			gameObject.layer = LayerMask.NameToLayer("Blue");
			guiScript.ChangeText("BLUE");
			guiScript.ChangeColor(Color.blue);
			break;
		case LevelManager.GameColors.Neutral:
			gameObject.layer = 0;
			guiScript.ChangeText("");
			guiScript.ChangeColor(Color.clear);
			break;
		}
	}

	void Pause() {
		isPaused = true;
		guiScript.Pause();
	}

	public void FillColor(LevelManager.GameColors color) {
		switch(color) {
			case LevelManager.GameColors.Red:
				ChangeColor(RedMaterial);
				break;
			case LevelManager.GameColors.Green:
				ChangeColor(GreenMaterial);
				break;
			case LevelManager.GameColors.Blue:
				ChangeColor(BlueMaterial);
				break;
			case LevelManager.GameColors.Neutral:
			default:
				ChangeColor(NeutralMaterial);
				break;
		}
	}

	private void ChangeColor(Material color) {
		PlayerMesh.GetComponent<Renderer>().material = color;
	}
}
