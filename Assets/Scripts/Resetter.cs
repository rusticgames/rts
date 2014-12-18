using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resetter : MonoBehaviour {
	public List<History> resetList = new List<History>();

	// Use this for initialization
	void Start () {
		StartCoroutine(checkInput());
	}
	
	IEnumerator checkInput() {
		while(true) {
			if (Input.GetKey (KeyCode.R)) {
				this.Reset(resetList);
			}
			yield return new WaitForEndOfFrame();
		}
	}

	public static void Reset(List<History> rl) {
		rl.ForEach(x => x.ResetToInitial());
	}
}
