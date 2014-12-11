using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyboardMover : MonoBehaviour {

	public float followDistance = .001f;	
	public float maximumVelocity = 10.0f;
	public bool overrideLinear = false;
	public bool relativeLinear = false;
	public Vector3 linear;
	public Vector3 lastLinearForce;
	public KeyboardTargeter targeter;

	private Vector3 distanceVector;

	void Reset() {
		targeter = this.GetComponent<KeyboardTargeter>();
	}

	void Start () {
		StartCoroutine(moveRoutine());
	}

	IEnumerator moveRoutine() {
		while(true) {
			distanceVector = GetDistanceToTarget (targeter.moveTarget);
			lastLinearForce = GetDesiredVelocity (distanceVector);
			this.rigidbody.AddForce (lastLinearForce, ForceMode.Force);
			yield return new WaitForFixedUpdate();
		}
	}

	Vector3 GetDistanceToTarget (Vector3 target)
	{
		return target - this.rigidbody.position;
	}
	
	Vector3 GetDesiredVelocity (Vector3 distance)
	{		
		if(overrideLinear) return linear;
		Vector3 desiredVelocity = new Vector3 (0, 0, 0);
		if (distance.sqrMagnitude > (followDistance * followDistance)) {
			desiredVelocity = distance.normalized * maximumVelocity;
		}
		
		return desiredVelocity - rigidbody.velocity;
	}
}
