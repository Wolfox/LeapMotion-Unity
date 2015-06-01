using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIScript : MonoBehaviour {

	public GameObject pausePanel;
	public GameObject endGamePanel;
	public Text guiText;
	public Image guiPanel;

	public void Pause() {
		pausePanel.SetActive(true);
	}

	public void Resume() {
		pausePanel.SetActive(false);
	}

	public void ChangeText(string text) {
		guiText.text = text;
	}

	public void ChangeTextColor(Color color) {
		guiText.color = color;
	}

	public void ChangeColor(Color color) {
		color.a /= 4;
		guiPanel.color = color;
	}

	/*public void ChangeAplha(float alpha) {
		Color color = guiPanel.color;
		color.a = alpha;
		guiPanel.color = color;
	}*/

	public string GetText() {
		return guiText.text;
	}

	public bool CheckPause() {
		return pausePanel.activeSelf;
	}

	public void EndGame() {
		endGamePanel.SetActive(true);
	}

	public void Quit() {
		Application.Quit();
	}

	public void BackToMainMenu() {
		Application.LoadLevel(0);
	}

}
