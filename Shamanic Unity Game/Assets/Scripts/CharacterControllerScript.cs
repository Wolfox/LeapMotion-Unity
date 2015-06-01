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

	private enum GameState {Game, Pause, EndGame};
	private GameState state = GameState.Game;

	public GameObject PlayerMesh;
	public GUIScript guiScript;
	public HandController controller;
	public AudioSource audio;

	public Material RedMaterial;
	public Material GreenMaterial;
	public Material BlueMaterial;
	public Material NeutralMaterial;
	
	private LevelManager.GameColors potionColor;
	private bool onExit;
	private bool onPotion;

	private float horzInput;
	private float vertInput;

	private Classifier classifier;
	//private string action = "";
	//private State gameState;

	void Awake() {
		Game.StartCulture();
	}

	// Use this for initialization
	void Start() {
		PlayerMesh.GetComponent<Renderer>().material = NeutralMaterial;
		onExit = false;
		onPotion = false;
		potionColor = LevelManager.GameColors.Neutral;
		UpdateState(Game.GameState());
	}

	void UpdateState(State state) {
		//gameState = state;
		classifier = Game.GetClassifier(state);
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

	/*void SaveFrame() {
		Frame frame = controller.GetFrame();
		if(frame.Id != prevFrame.Id) {
			Sign s = Sequences.Utils.FrameToSign(frame, prevFrame);
			buffer.AddSign(s);
		}
		prevFrame = frame;
	}*/

	void FixedUpdate() {

		if(state == GameState.Pause && !guiScript.CheckPause()) {
			Resume ();
		}

		vertInput = 0;
		horzInput = 0;
		
		//SaveFrame();

		CheckActions();

		float keyH = Input.GetAxis("Horizontal");
		float keyV = Input.GetAxis("Vertical");

		if(keyH != 0) {horzInput = keyH;}
		if(keyV != 0) {vertInput = keyV;}

		MoveAndRotate();
	}

	void CheckActions() {
		//double[][] sequence = buffer.getSequence().GetArray();

		/*List<string> actions = gameState.GetActions();
		for(int i = 0; i < actions.Count; i++) {
			Debug.Log (actions[i]+ "(" + i +") - " + classifier.testModel(i, sequence));
		}*/

		//classifier.testModel(0,sequence);

		List<string> allActions = controller.GetGestures(classifier);
		List<string> actions = Game.UpdateActions(allActions);

		switch(state) {
		case GameState.Game:
			CheckMove(allActions);
			CheckPauseDrinkGrab(actions);
			break;
		case GameState.Pause:
			CheckPauseActions(actions);
			break;
		case GameState.EndGame:
			CheckEndGameActions(actions);
			break;
		default:
			break;
		}
	}

	void CheckMove(List<string> actions) {
		for (int i = 0; i< actions.Count; i++) {
			switch(actions[i]) {
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
			default:
				break;
			}
		}
	}

	void CheckPauseDrinkGrab(List<string> actions) {
		for (int i = 0; i< actions.Count; i++) {
			switch(actions[i]) {
			case "PAUSE":
				Pause();
				break;
			case "DRINK":
				Drink();
				break;
			case "GRAB":
				Grab();
				break;
			default:
				break;
			}
		}
	}

	void CheckPauseActions(List<string> actions) {
		for (int i = 0; i< actions.Count; i++) {
			switch(actions[i]) {
			case "RESUME":
				Resume ();
				break;
			case "MUTE":
				Mute();
				break;
			case "UNMUTE":
				Unmute();
				break;
			default:
				break;
			}
		}
	}

	void CheckEndGameActions(List<string> actions) {
		for (int i = 0; i< actions.Count; i++) {
			switch(actions[i]) {
			case "RESUME":
				guiScript.BackToMainMenu();
				break;
			case "QUIT":
				guiScript.Quit();
				break;
			default:
				break;
			}
		}
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
		if(state != GameState.Game) {return;}

		Vector3 target = transform.position + transform.forward*value;
		target.y = transform.position.y;
		transform.position = Vector3.MoveTowards(transform.position, target, 0.1f);
	}

	void Rotate (float value) {
		if(state != GameState.Game) {return;}
		transform.RotateAround(transform.position, Vector3.up, value);
	}

	void Grab() {
		if(!onExit) {return;}
		state = GameState.EndGame;
		UpdateState(Game.EndGameState());
		guiScript.EndGame();
	}

	void Drink() {
		if(!onPotion) {return;}
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

	public void Pause() {
		state = GameState.Pause;
		UpdateState(Game.PauseState());
		guiScript.Pause();
	}

	public void Resume() {
		state = GameState.Game;
		UpdateState(Game.GameState());
		guiScript.Resume ();
	}

	public void Mute() {
		audio.mute = true;
		Debug.Log ("MUTE");
	}

	public void Unmute() {
		audio.mute = false;
		Debug.Log ("UNMUTE");
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
