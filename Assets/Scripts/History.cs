using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class History : MonoBehaviour {
	private Vector3 initialPosition;
	private Vector3 initialLinearVelocity;
	private Vector3 initialAngularVelocity;
	private Quaternion initialRotation;

	[System.Serializable]
	public class HistoryTrackingAction : UnityEngine.Events.UnityEvent<History, float>{};
	public HistoryTrackingAction OnStateSample = new HistoryTrackingAction();
	public float stateSampleRate = 0.5f;
	public List<Object> destroyableChildren = new List<Object>();

	void Start () {
		initialPosition = this.transform.position;
		initialLinearVelocity = this.rigidbody.velocity;
		initialAngularVelocity = this.rigidbody.angularVelocity;
		initialRotation = this.transform.rotation;
		StartCoroutine(sampleState());
	}
	
	IEnumerator sampleState() {
		while(true) {
			if(OnStateSample != null) {
				OnStateSample.Invoke(this, UnityEngine.Time.timeSinceLevelLoad);
			}
			yield return new WaitForSeconds(stateSampleRate);
		}
	}

	public void ResetToInitial() {
		List<Object> cl = destroyableChildren;
		destroyableChildren = new List<Object>();
		this.transform.position = initialPosition;
		this.rigidbody.velocity = initialLinearVelocity;
		this.rigidbody.angularVelocity = initialAngularVelocity;
		this.transform.rotation = initialRotation;
		cl.ForEach(x => Object.Destroy(x));
		cl.Clear();
	}

	public void addChild (Object o)
	{
		destroyableChildren.Add (o);
	}
}
