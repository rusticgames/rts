using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyboardMover : MonoBehaviour {

	public Vector3 moveTarget;
	public Vector3 newTargetOffset;
	public float followDistance = .001f;	
	public float maximumVelocity = 100.0f;
	public bool overrideLinear = false;
	public bool relativeLinear = false;
	public Vector3 linear;
	public Vector3 lastLinearForce;
	public KeyCode up;
	public KeyCode down;
	public KeyCode left;
	public KeyCode right;

	private Vector3 distanceVector;

	void Start () {
		moveTarget = this.rigidbody.position;
		StartCoroutine(checkInput());
		StartCoroutine(moveRoutine());
	}
	
	IEnumerator checkInput() {
		while(true) {
			newTargetOffset = Vector3.zero;
			if (Input.GetKey (up)) {
				newTargetOffset.z += .1f;
			}
			if (Input.GetKey (down)) {
				newTargetOffset.z -= .1f;
			}
			if (Input.GetKey (left)) {
				newTargetOffset.x -= .1f;
			}
			if (Input.GetKey (right)) {
				newTargetOffset.x += .1f;
			}
			moveTarget = newTargetOffset + this.rigidbody.position;
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator moveRoutine() {
		while(true) {
			distanceVector = GetDistanceToTarget (moveTarget);
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
