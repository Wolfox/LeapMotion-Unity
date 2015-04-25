using UnityEngine;
using System.Collections;
using System.IO;
using HMM_Test_Library;
using Leap;

public class CubesLevelTestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Debug.Log (Class1.Test());
		/*Sign s = new Sign();
		Sequence sa = new Sequence();
		s = FrameToSign.Frame2Sign(new Frame());*/
		Stream writeStream = new FileStream("MyFile1.bin", FileMode.Create, FileAccess.Write, FileShare.None);
		/*HMM hmm = new HMM();
		hmm.Load();
		hmm.Finit();*/
		//HMM.SaveForTest();
		writeStream.Close ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("escape")) {
			Application.LoadLevel(0);
		}
	}
}
