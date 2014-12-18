using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyboardTargeter : MonoBehaviour {

	public Vector3 moveTarget;
	public Vector3 newTargetOffset;
	public KeyboardConfiguration keyboardConfig;
	
	void Reset() {
		keyboardConfig = this.GetComponent<KeyboardConfiguration>();
	}

	void Start () {
		moveTarget = this.rigidbody.position;
		StartCoroutine(checkInput());
	}
	
	IEnumerator checkInput() {
		while(true) {
			newTargetOffset = Vector3.zero;
			if (Input.GetKey (keyboardConfig.up)) {
				newTargetOffset.z += .1f;
			}
			if (Input.GetKey (keyboardConfig.down)) {
				newTargetOffset.z -= .1f;
			}
			if (Input.GetKey (keyboardConfig.left)) {
				newTargetOffset.x -= .1f;
			}
			if (Input.GetKey (keyboardConfig.right)) {
				newTargetOffset.x += .1f;
			}
			moveTarget = newTargetOffset + this.rigidbody.position;
			yield return new WaitForFixedUpdate();
		}
	}
}
