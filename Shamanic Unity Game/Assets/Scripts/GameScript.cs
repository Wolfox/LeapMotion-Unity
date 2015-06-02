using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Distributions.Multivariate;
using Sequences;
using System.IO;

public static class Game {

	private static Dictionary<string,float> actionsBuffer = new Dictionary<string, float>();
	private static float minActionTime = 1.5f;

	public static string culture = "";
	public static int bufferSize = 50;
	public static int numberOfColors = 3;

	public static Dictionary<string, HiddenMarkovModel<MultivariateNormalDistribution>> allModels =
		new Dictionary<string, HiddenMarkovModel<MultivariateNormalDistribution>> ();

	public static bool alreadyStarted = false;

	public static CulturalLayer culturalLayer = new CulturalLayer();

	public static List<HiddenMarkovModel<MultivariateNormalDistribution>> GetModels(State state) {
		return state.GetModelsWithCulture(allModels, culturalLayer, culture);
	}

	public static void InitAllModels() {
		allModels.Add("OPEN_HAND",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/OPEN_HAND.bin"));
		allModels.Add("POINT_FRONT",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/POINT_FRONT.bin"));
		allModels.Add("POINT_RIGHT",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/POINT_RIGHT.bin"));
		allModels.Add("POINT_LEFT",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/POINT_LEFT.bin"));
		allModels.Add("POINT_BACK",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/POINT_BACK.bin"));
		allModels.Add("OPEN_FRONT",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/OPEN_FRONT.bin"));
		allModels.Add("OPEN_RIGHT",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/OPEN_RIGHT.bin"));
		allModels.Add("OPEN_LEFT",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/OPEN_LEFT.bin"));
		allModels.Add("HAND_HALT",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/HALT_HAND.bin"));
		allModels.Add("HAND_ROTATING",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/HAND_ROTATING.bin"));
		allModels.Add("INDEX_HUSH",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/INDEX_HUSH.bin"));
		allModels.Add("INDEX_ROTATING",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/INDEX_ROTATING.bin"));
		allModels.Add("MOUTH_MIMIC",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/MOUTH_MIMIC.bin"));
		allModels.Add("INDEX",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/NUM1.bin"));
		allModels.Add("INDEX_MIDDLE",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/NUM2.bin"));
		allModels.Add("INDEX_MIDDLE_RINGER",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/NUM3.bin"));
		allModels.Add("THE_RING",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/THE_RING.bin"));
		allModels.Add("THUMBS_DOWN",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/THUMBS_DOWN.bin"));
		allModels.Add("THUMBS_UP",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/THUMBS_UP.bin"));
		allModels.Add("WAVE",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/WAVE.bin"));
		allModels.Add("WAVE_NO_THANKS",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/WAVE_NO_THANKS.bin"));
		allModels.Add("BOTTLE_MIMIC",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/DRINK_PT.bin"));
		allModels.Add("HOLDING_GLASS",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/DRINK_NL.bin"));
		allModels.Add("GRAB",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/GRAB.bin"));
	}

	public static void InitCulturalLayer() {
		culturalLayer.AddCustomGesture("NOTHING", "OPEN_HAND");
		culturalLayer.AddCustomGesture("SELECT", "POINTING");
		culturalLayer.AddCustomGesture("CLICK", "INDEX_CLICK");
		culturalLayer.AddCustomGesture("NUMBER_1","INDEX");
		culturalLayer.AddCustomGesture("NUMBER_2","INDEX_MIDDLE");
		culturalLayer.AddCustomGesture("NUMBER_3","INDEX_MIDDLE_RINGER");
		culturalLayer.AddCustomGesture("YES","THUMBS_UP");
		culturalLayer.AddCustomGesture("MUTE", "INDEX_HUSH");
		culturalLayer.AddCustomGesture("UNMUTE", "MOUTH_MIMIC");
		culturalLayer.AddCustomGesture("PAUSE", "HAND_HALT");
		culturalLayer.AddCustomGesture("QUIT", "WAVE");
		culturalLayer.AddCustomGesture("GRAB", "GRAB");
		culturalLayer.AddCustomGesture("MUTE", "INDEX_HUSH");
		//culturalLayer.AddCustomGesture("", "");

		//culturalLayer.AddCultureGesture("NUMBER_0","PT","FIST");
		culturalLayer.AddCultureGesture("NO", "PT", "THUMBS_DOWN");
		culturalLayer.AddCultureGesture("FRONT", "PT", "POINT_FRONT");
		culturalLayer.AddCultureGesture("RIGHT", "PT", "POINT_RIGHT");
		culturalLayer.AddCultureGesture("LEFT", "PT", "POINT_LEFT");
		culturalLayer.AddCultureGesture("BACK", "PT", "POINT_BACK");
		culturalLayer.AddCultureGesture("DRINK", "PT", "BOTTLE_MIMIC");
		culturalLayer.AddCultureGesture("RESUME", "PT", "INDEX_ROTATING");
		//culturalLayer.AddCultureGesture("", "PT", "");


		//culturalLayer.AddCultureGesture("NUMBER_0", "NL", "THE_RING");
		culturalLayer.AddCultureGesture("NO", "NL", "WAVE_NO_THANKS");
		culturalLayer.AddCultureGesture("FRONT", "NL", "OPEN_FRONT");
		culturalLayer.AddCultureGesture("RIGHT", "NL", "OPEN_RIGHT");
		culturalLayer.AddCultureGesture("LEFT", "NL", "OPEN_LEFT");
		culturalLayer.AddCultureGesture("DRINK", "NL", "HOLDING_GLASS");
		culturalLayer.AddCultureGesture("RESUME", "NL", "HAND_ROTATING");
		//culturalLayer.AddCultureGesture("", "NL", "");
	}

	public static void StartCulture() {
		if(!alreadyStarted) {
			InitAllModels();
			InitCulturalLayer();
			alreadyStarted = true;
		}
	}

	public static Classifier GetClassifier(State state) {
		Classifier classifier = new Classifier(GetModels(state), state.GetActions());
		classifier.StartClassifier();
		return classifier;
	}

	public static State GameState() {
		State state = new State("Game State");
		state.AddAction("NOTHING");
		state.AddAction("FRONT");
		state.AddAction("RIGHT");
		state.AddAction("LEFT");
		state.AddAction("BACK");
		state.AddAction("DRINK");
		state.AddAction("GRAB");
		state.AddAction("PAUSE");
		/*state.AddAction("MUTE");
		state.AddAction("UNMUTE");*/
		return state;
	}

	public static State ChooseNumberState() {
		State state = new State("Choose Number State");
		state.AddAction("NOTHING");
		state.AddAction("NUMBER_1");
		state.AddAction("NUMBER_2");
		state.AddAction("NUMBER_3");
		return state;
	}

	public static State StartGameState() {
		State state = new State("Pause State");
		state.AddAction("NOTHING");
		state.AddAction("YES");
		state.AddAction("NO");
		return state;
	}

	public static State PauseState() {
		State state = new State("Pause State");
		state.AddAction("NOTHING");
		state.AddAction("RESUME");
		state.AddAction("MUTE");
		state.AddAction("UNMUTE");
		return state;
	}

	public static State EndGameState() {
		State state = new State("End Game State");
		state.AddAction("NOTHING");
		state.AddAction("QUIT");
		state.AddAction("RESUME");
		return state;
	}

	public static State NothingState() {
		State state = new State("Nothing State");
		state.AddAction("NOTHING");
		return state;
	}

	public static List<string> UpdateActions(List<string> actions) {
		List<string> actionsToCheck = new List<string> (actionsBuffer.Keys);
		List<string> returnActions = new List<string>();

		foreach(string action in actions) {
			actionsToCheck.Remove(action);
			if(!actionsBuffer.ContainsKey(action)) {
				actionsBuffer[action] = Time.time;
			}

			if(actionsBuffer[action] + minActionTime < Time.time) {
				returnActions.Add (action);
			}
		}

		foreach (string actionToCheck in actionsToCheck) {
			actionsBuffer.Remove(actionToCheck);
		}

		return returnActions;
	}
}