using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class History : MonoBehaviour {
	private Vector3 initialPosition;
	private Vector3 initialLinearVelocity;
	private Vector3 initialAngularVelocity;
	private Quaternion initialRotation;

	public delegate void PositionTrackingAction(Vector3 position, float timestamp);
	public event PositionTrackingAction OnPositionSampled;
	public float positionSampleRate = 0.5f;
	public List<Object> destroyableChildren = new List<Object>();

	void Start () {
		initialPosition = this.transform.position;
		initialLinearVelocity = this.rigidbody.velocity;
		initialAngularVelocity = this.rigidbody.angularVelocity;
		initialRotation = this.transform.rotation;
		StartCoroutine(samplePosition());
	}
	
	IEnumerator samplePosition() {
		while(true) {
			if(OnPositionSampled != null) {
				OnPositionSampled(this.transform.position, UnityEngine.Time.timeSinceLevelLoad);
			}
			yield return new WaitForSeconds(positionSampleRate);
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
