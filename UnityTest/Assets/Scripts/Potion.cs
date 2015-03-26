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
				renderer.material = RedMaterial;
				break;
			case LevelManager.GameColors.Green:
				renderer.material = GreenMaterial;
				break;
			case LevelManager.GameColors.Blue:
				renderer.material = BlueMaterial;
				break;
			default:
				throw new System.ArgumentNullException();
				break;
		}
	}
}
