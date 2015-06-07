using UnityEngine;
using System.Collections;
using Leap;
using Shamanic_Interface;

public class HandGesture : MonoBehaviour {

	private SequenceBuffer buffer = new SequenceBuffer(Game.bufferSize);

	public void AddSign(Hand hand, Frame previousFrame) {
		buffer.AddSign(Shamanic_Interface.Utils.HandToSign(hand, previousFrame));
	}

	public string GetAction(Classifier classifier) {
		return classifier.ComputeToString(buffer.getSequence().GetArray());
	}
}
