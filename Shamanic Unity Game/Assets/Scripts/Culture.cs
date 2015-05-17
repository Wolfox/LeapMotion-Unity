using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Distributions.Multivariate;
using Sequences;
using System.IO;
using Accord.Statistics.Models.Markov.Learning;
using Accord.Statistics.Models.Markov.Topology;

public static class Culture { //: MonoBehaviour {

	public static string culture = "NL";

	public static Dictionary<string, HiddenMarkovModel<MultivariateNormalDistribution>> allModels =
		new Dictionary<string, HiddenMarkovModel<MultivariateNormalDistribution>> ();

	public static bool alreadyStarted = false;

	public static CulturalLayer culturalLayer = new CulturalLayer();

	public static List<HiddenMarkovModel<MultivariateNormalDistribution>> GetModels(State state) {
		return state.GetModelsWithCulture(allModels, culturalLayer, culture);
	}

	public static void InitAllModels() {
		allModels.Add("OPEN_HAND",
			HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/OpenModel.bin"));
		allModels.Add("POINT_FRONT",
			HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/FrontModel.bin"));
		allModels.Add("POINT_RIGHT",
			HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/RightModel.bin"));
		allModels.Add("POINT_LEFT",
			HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/LeftModel.bin"));
		allModels.Add("POINT_BACK",
			HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/BackModel.bin"));
		allModels.Add("OPEN_FRONT",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/OPEN_FRONT.bin"));
		allModels.Add("OPEN_RIGHT",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/OPEN_RIGHT.bin"));
		allModels.Add("OPEN_LEFT",
		              HiddenMarkovModel<MultivariateNormalDistribution>.Load("GestureModels/OPEN_LEFT.bin"));
	}

	public static void InitCulturalLayer() {
		culturalLayer.AddCustomGesture("NOTHING", "OPEN_HAND");
		culturalLayer.AddCustomGesture("SELECT", "POINTING");
		culturalLayer.AddCustomGesture("CLICK", "INDEX_CLICK");
		culturalLayer.AddCustomGesture("NUMBER_1","INDEX");
		culturalLayer.AddCustomGesture("NUMBER_2","INDEX_MIDDLE");
		culturalLayer.AddCustomGesture("NUMBER_3","INDEX_MIDDLE_RINGER");
		culturalLayer.AddCustomGesture("YES","THUMBS_UP");
		culturalLayer.AddCustomGesture("UNMUTE", "MOUTH_MIMIC");
		culturalLayer.AddCustomGesture("PAUSE", "HAND_HALT");
		culturalLayer.AddCustomGesture("QUIT", "WAVE");
		//culturalLayer.AddCustomGesture("", "");


		//culturalLayer.AddCultureGesture("NUMBER_0","PT","FIST");
		culturalLayer.AddCultureGesture("NO", "PT", "THUMBS_DOWN");
		culturalLayer.AddCultureGesture("FRONT", "PT", "POINT_FRONT");
		culturalLayer.AddCultureGesture("RIGHT", "PT", "POINT_RIGHT");
		culturalLayer.AddCultureGesture("LEFT", "PT", "POINT_LEFT");
		culturalLayer.AddCultureGesture("BACK", "PT", "POINT_BACK");
		culturalLayer.AddCultureGesture("DRINK", "PT", "BOTTLE_MIMIC");
		//culturalLayer.AddCultureGesture("HELP", "PT", "STRECH_INDEX");
		culturalLayer.AddCultureGesture("MUTE", "PT", "GRAB_AIR");
		culturalLayer.AddCultureGesture("RESUME", "PT", "INDEX_ROTATING");
		//culturalLayer.AddCultureGesture("", "PT", "");


		//culturalLayer.AddCultureGesture("NUMBER_0", "NL", "THE_RING");
		culturalLayer.AddCultureGesture("NO", "NL", "WAVE_NO_THANKS");
		culturalLayer.AddCultureGesture("FRONT", "NL", "OPEN_FRONT");
		culturalLayer.AddCultureGesture("RIGHT", "NL", "OPEN_RIGHT");
		culturalLayer.AddCultureGesture("LEFT", "NL", "OPEN_LEFT");
		culturalLayer.AddCultureGesture("DRINK", "NL", "HOLDING_GLASS");
		//culturalLayer.AddCultureGesture("HELP", "NL", "HAND_HALT");
		culturalLayer.AddCultureGesture("MUTE", "NL", "INDEX_HUSH");
		culturalLayer.AddCultureGesture("RESUME", "NL", "HAND_ROTATING");
		//culturalLayer.AddCultureGesture("", "NL", "");
	}

	public static void CreateModelFromFrames(string readPath, string writePath)
	{
		SequenceList seq = Utils.FramesToSequenceList(Utils.LoadListListFrame(readPath));
		
		HiddenMarkovModel<MultivariateNormalDistribution> hmm;
		MultivariateNormalDistribution mnd = new MultivariateNormalDistribution(seq.GetArray()[0][0].Length);
		hmm = new HiddenMarkovModel<MultivariateNormalDistribution>(new Forward(5), mnd);
		
		var teacher = new BaumWelchLearning<MultivariateNormalDistribution>(hmm);
		teacher.Run(seq.GetArray());
		hmm.Save(writePath);
	}

}
