using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Distributions.Multivariate;
using Sequences;
using System.IO;

public static class Culture {

	public static string culture = "PT";

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
		allModels.Add("TEST1",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/HALT_HAND.bin"));
		allModels.Add("TEST2",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/HALT_HANDR.bin"));
		allModels.Add("TEST3",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/HALT_HANDR_2x.bin"));
		allModels.Add("TEST4",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/HALT_HAND_half.bin"));
		allModels.Add("TEST5",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/HALT_HAND_half2x.bin"));
		allModels.Add("TEST6",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/HALT_HAND_alt.bin"));
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
		//culturalLayer.AddCustomGesture("GRAB", "GRAB");
		//culturalLayer.AddCustomGesture("", "");
		culturalLayer.AddCustomGesture("TEST1", "TEST1");
		culturalLayer.AddCustomGesture("TEST2", "TEST2");
		culturalLayer.AddCustomGesture("TEST3", "TEST3");
		culturalLayer.AddCustomGesture("TEST4", "TEST4");
		culturalLayer.AddCustomGesture("TEST5", "TEST5");
		culturalLayer.AddCustomGesture("TEST6", "TEST6");


		//culturalLayer.AddCultureGesture("NUMBER_0","PT","FIST");
		culturalLayer.AddCultureGesture("NO", "PT", "THUMBS_DOWN");
		culturalLayer.AddCultureGesture("FRONT", "PT", "POINT_FRONT");
		culturalLayer.AddCultureGesture("RIGHT", "PT", "POINT_RIGHT");
		culturalLayer.AddCultureGesture("LEFT", "PT", "POINT_LEFT");
		culturalLayer.AddCultureGesture("BACK", "PT", "POINT_BACK");
		//culturalLayer.AddCultureGesture("DRINK", "PT", "BOTTLE_MIMIC");
		//culturalLayer.AddCultureGesture("HELP", "PT", "STRECH_INDEX");
		//culturalLayer.AddCultureGesture("MUTE", "PT", "GRAB_AIR");
		culturalLayer.AddCultureGesture("RESUME", "PT", "INDEX_ROTATING");
		//culturalLayer.AddCultureGesture("", "PT", "");


		//culturalLayer.AddCultureGesture("NUMBER_0", "NL", "THE_RING");
		culturalLayer.AddCultureGesture("NO", "NL", "WAVE_NO_THANKS");
		culturalLayer.AddCultureGesture("FRONT", "NL", "OPEN_FRONT");
		culturalLayer.AddCultureGesture("RIGHT", "NL", "OPEN_RIGHT");
		culturalLayer.AddCultureGesture("LEFT", "NL", "OPEN_LEFT");
		//culturalLayer.AddCultureGesture("DRINK", "NL", "HOLDING_GLASS");
		//culturalLayer.AddCultureGesture("HELP", "NL", "HAND_HALT");
		culturalLayer.AddCultureGesture("MUTE", "NL", "INDEX_HUSH");
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

}
