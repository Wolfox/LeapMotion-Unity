using UnityEngine;
using System.Collections;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Distributions.Multivariate;
using System.Collections.Generic;
using Sequences;
using Leap;
//using HMM_Test_Library;

public class CharacterControllerScript : MonoBehaviour {

	//public Transform characterCamera;

	public GameObject PlayerMesh;
	public GUIText GUIText;
	public HandController controller;

	public Material RedMaterial;
	public Material GreenMaterial;
	public Material BlueMaterial;
	public Material NeutralMaterial;

	private bool onExit;
	private bool onPotion;
	private LevelManager.GameColors potionColor;

	private Sample sample;
	private Classifier classifier;
	private string action;

	private State gameState;

	void Awake() {
		sample = new Sample(50);
		StartState();
		if(!Culture.alreadyStarted) {
			Culture.CreateModelFromFrames("Frames/OPEN_FRONT.frs", "GestureModels/OPEN_FRONT.bin");
			Culture.CreateModelFromFrames("Frames/OPEN_RIGHT.frs", "GestureModels/OPEN_RIGHT.bin");
			Culture.CreateModelFromFrames("Frames/OPEN_LEFT.frs", "GestureModels/OPEN_LEFT.bin");
			Culture.InitAllModels();
			Culture.InitCulturalLayer();
			Culture.alreadyStarted = true;
		}
	}

	// Use this for initialization
	void Start() {
		PlayerMesh.GetComponent<Renderer>().material = NeutralMaterial;
		onExit = false;
		onPotion = false;
		potionColor = LevelManager.GameColors.Neutral;
		StartController();
	}

	void StartState() {
		gameState = new State("Game State");
		gameState.AddAction("NOTHING");
		gameState.AddAction("FRONT");
		gameState.AddAction("RIGHT");
		gameState.AddAction("LEFT");
		gameState.AddAction("BACK");
	}

	void StartController() {

		classifier = new Classifier(Culture.GetModels(gameState), gameState.GetActions());
		classifier.StartClassifier();
	}

	HiddenMarkovModel<MultivariateNormalDistribution> HelpLoad(string path) {
		return HiddenMarkovModel<MultivariateNormalDistribution>.Load(path);
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown ("Grab") && onExit) {
			Application.LoadLevel(0);
		}
		if(Input.GetButtonDown("Drink") && onPotion) {
			switch(potionColor) {
				case LevelManager.GameColors.Red:
					gameObject.layer = LayerMask.NameToLayer("Red");
					GUIText.text = "RED";
					GUIText.color = Color.red;
					break;
				case LevelManager.GameColors.Green:
					gameObject.layer = LayerMask.NameToLayer("Green");
					GUIText.text = "GREEN";
					GUIText.color = Color.green;
					break;
				case LevelManager.GameColors.Blue:
					gameObject.layer = LayerMask.NameToLayer("Blue");
					GUIText.text = "BLUE";
					GUIText.color = Color.blue;
					break;
				case LevelManager.GameColors.Neutral:
					gameObject.layer = 0;
					GUIText.text = "";
					GUIText.color = Color.white;
					break;
			}
		}
	}

	void SaveFrame() {
		Frame frame = controller.GetFrame();
		Sign s = FrameToSign.Frame2Sign(frame);
		sample.AddSign(s);
	}

	void FixedUpdate() {

		float v = 0;
		float h = 0;
		
		SaveFrame();
		action = classifier.ComputeToString(sample.getSequence().GetArray());

		Debug.Log(action);

		switch(action) {
		case "NOTHING":
			break;
		case "FRONT":
			v = 1;
			break;
		case "BACK":
			v = -1;
			break;
		case "RIGHT":
			h = 1;
			break;
		case "LEFT":
			h = -1;
			break;
		}

		if(v != 0){
			Move (v);
		}

		if(h != 0) {
			Rotate(h);
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

	void Move (float value) {

		Vector3 target = transform.position + transform.forward*value;
		target.y = transform.position.y;

		transform.position = Vector3.MoveTowards(transform.position, target, 0.1f);
	}

	void Rotate (float value) {

		transform.RotateAround(transform.position, Vector3.up, value*3);
		//transform.Rotate(new Vector3(0.0f,value,0.0f));
		//float target = transform.rotation.eulerAngles.y + value;
		//transform.rotation = Quaternion.AngleAxis(target,Vector3.up);
	}

	public void FillColor(LevelManager.GameColors colour) {
		switch(colour) {
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
