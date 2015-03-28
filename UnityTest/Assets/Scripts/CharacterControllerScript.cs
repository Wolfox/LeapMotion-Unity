using UnityEngine;
using System.Collections;

public class CharacterControllerScript : MonoBehaviour {

	//public Transform characterCamera;

	public GameObject PlayerMesh;
	public GUIText GUIText;

	public Material RedMaterial;
	public Material GreenMaterial;
	public Material BlueMaterial;
	public Material NeutralMaterial;

	private bool onExit;
	private bool onPotion;
	private LevelManager.GameColors potionColor;


	// Use this for initialization
	void Start() {
		PlayerMesh.GetComponent<Renderer>().material = NeutralMaterial;
		onExit = false;
		onPotion = false;
		potionColor = LevelManager.GameColors.Neutral;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown ("Grab") && onExit) {
			Debug.Log("That's all, folks!");
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

	void FixedUpdate() {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

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
