using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

	public GameObject LoadingScene;

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
