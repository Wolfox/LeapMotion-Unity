using UnityEngine;
using System.Collections;

public class Potion : MonoBehaviour {

	public LevelManager.GameColors color;

	public Material RedMaterial;
	public Material GreenMaterial;
	public Material BlueMaterial;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FillColor(LevelManager.GameColors colour) {
		switch(colour) {
			case LevelManager.GameColors.Red:
				GetComponent<Renderer>().material = RedMaterial;
				break;
			case LevelManager.GameColors.Green:
				GetComponent<Renderer>().material = GreenMaterial;
				break;
			case LevelManager.GameColors.Blue:
				GetComponent<Renderer>().material = BlueMaterial;
				break;
			default:
				throw new System.ArgumentNullException();
				break;
		}
	}
}
