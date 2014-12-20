using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ToolUser : MonoBehaviour {

	public float usePeriodSeconds = 0.5f;
	public KeyboardTargeter targeter;
	public KeyboardConfiguration keyboardConfig;
	public History history;
	public Shooter currentTool;
	
	void Reset() {
		targeter = this.GetComponent<KeyboardTargeter>();
		keyboardConfig = this.GetComponent<KeyboardConfiguration>();
		history = this.GetComponent<History>();
	}
	
	void Start () {
		StartCoroutine(Use());
	}


	
	IEnumerator Use() {
		while (true) {
			if (Input.GetKey(keyboardConfig.shoot)) {
				Vector3 heading = targeter.newTargetOffset;
				Vector3 startPosition = this.transform.position + heading;
				history.addChild(currentTool.Use(startPosition, this.transform.rotation, heading));
				yield return new WaitForSeconds(usePeriodSeconds);
			}
			yield return null;
		}
	}
}
