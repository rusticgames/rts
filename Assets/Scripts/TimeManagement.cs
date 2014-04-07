using UnityEngine;
using System.Collections;

public class TimeManagement : MonoBehaviour {
	public bool paused = false;

	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			paused = !paused;
			Pause(paused);
		}
	}

	void OnGUI () {
		if (paused) {
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Paused");
		}
	}

	public void Pause (bool paused) {
		Time.timeScale = paused ? 0.0f : 1.0f;
		Rigidbody[] rigidbodies = FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
		foreach (Rigidbody rigidbody in rigidbodies) {
			rigidbody.isKinematic = paused;
		}
	}
}
