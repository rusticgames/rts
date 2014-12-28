using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class ToolUseData {
	public ToolUser user;
	public Vector3 heading;
	public Vector3 position;
	public Quaternion orientation;
	
	public ToolUseData (ToolUser user, Vector3 heading, Vector3 position, Quaternion orientation)
	{
		this.user = user;
		this.heading = heading;
		this.position = position;
		this.orientation = orientation;
	}
}

[System.Serializable]
public class ToolUsedAction : UnityEvent<ToolUseData>{};
public class ToolUser : MonoBehaviour {

	public float usePeriodSeconds = 0.5f;
	public KeyboardTargeter targeter;
	public KeyboardConfiguration keyboardConfig;
	public History history;
	public ToolUsedAction OnToolUsed = new ToolUsedAction();
	
	void Reset() {
		targeter = this.GetComponent<KeyboardTargeter>();
		keyboardConfig = this.GetComponent<KeyboardConfiguration>();
		history = this.GetComponent<History>();
	}
	
	void Start () {
		StartCoroutine(Use());
	}

	ToolUseData getCurrentUseData ()
	{
		return new ToolUseData (this, targeter.newTargetOffset, this.transform.position + targeter.newTargetOffset, this.transform.rotation);
	}
	
	IEnumerator Use() {
		while (true) {
				while (Input.GetKey(keyboardConfig.shoot)) {
					OnToolUsed.Invoke (getCurrentUseData ());
					yield return new WaitForSeconds(usePeriodSeconds);
				}
			yield return null;
		}
	}
}
