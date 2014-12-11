using UnityEngine;
using System.Collections;

public class AreaGoal : MonoBehaviour {
	public UnityEngine.UI.Text text;

	void Start() {
		text.text = "Reach the goal!";
	}

	void OnTriggerEnter(Collider other) {
		// Destroy(other.gameObject);
		text.text = other.name + " the goal!";
	}
}
