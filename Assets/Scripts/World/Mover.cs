using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mover : MonoBehaviour
{
	Vector3 moveTarget;
	GameObject followTarget;
	public float followDistance = 1.0f;
	public float maximumVelocity = 1.0f;
	public float maximumAngularVelocity = 1.0f;
	public Mechanism forceSource;
	public bool stopByArrival = false;
	public bool overrideLinear = false;
	public bool overrideAngular = true;
	public bool relativeLinear = false;
	public bool relativeAngular = false;
	public Vector3 linear;
	public Vector3 angular;
	public bool logPrints = false;
	public bool logClear = false;
	public Vector3 lastLinearVelocity;
	public Vector3 lastAngularVelocity;
	
	// Start is called just before any of the
	// Update methods is called the first time.
	void Start ()
	{
		if (followTarget == null) {
			this.follow (this.gameObject);
		}
		moveTarget = followTarget.transform.position;

	}
	
	// Update is called every frame, if the
	// MonoBehaviour is enabled.
	void FixedUpdate ()
	{
		try2DMove ();
	}

	Vector2 GetDistanceToTarget ()
	{
		if (followTarget != this.gameObject) {
			moveTarget = followTarget.transform.position;
		}

		return new Vector2(moveTarget.x - this.rigidbody.position.x, moveTarget.z - this.rigidbody.position.z);
	}

	Vector3 GetDesiredVelocity (Vector2 distanceVector)
	{		
		Vector3 desiredVelocity = new Vector3 (0, 0, 0);
		if (distanceVector.sqrMagnitude > (followDistance * followDistance)) {
			Vector2 moveDirection = distanceVector.normalized;
			desiredVelocity.x = maximumVelocity * moveDirection.x;
			desiredVelocity.z = maximumVelocity * moveDirection.y;
		}

		return desiredVelocity - rigidbody.velocity;
	}

	Vector3 GetRotationTarget (Vector2 distanceVector)
	{
		Quaternion q;
		Vector3 orientationTarget = this.rigidbody.rotation.eulerAngles;
		if(logClear) Debug.ClearDeveloperConsole();
		string logString = "current: " + orientationTarget.ToString();
		if(orientationTarget.sqrMagnitude > 25.0f) {
			//orientationTarget.y = (180 - (360 * Mathf.Atan2 (distanceVector.x, distanceVector.y) / (2 * Mathf.PI)));
			orientationTarget.y = (360 * Mathf.Atan2 (distanceVector.x, distanceVector.y) / (2 * Mathf.PI));
		}
		logString += ", target: " + orientationTarget.ToString();
		orientationTarget = orientationTarget - this.rigidbody.rotation.eulerAngles;
		logString += "\r\ndelta: " + orientationTarget.ToString();
		//orientationTarget.y %= 360;
		//logString += ", corrected: " + orientationTarget.ToString();
		if(logPrints && orientationTarget.y != this.rigidbody.rotation.eulerAngles.y) {
			if(Mathf.Abs(orientationTarget.y) > 180f){
				Debug.LogError(logString);
			}else {
				Debug.LogWarning(logString);
			}
		}
		return orientationTarget;
	}

	void try2DMove ()
	{
		var distanceVector = GetDistanceToTarget ();
		lastAngularVelocity = rigidbody.angularVelocity;
		lastLinearVelocity = rigidbody.velocity;
		var linearForce = overrideLinear ? linear : GetDesiredVelocity (distanceVector);
		forceSource.applyLinearForce(this.rigidbody, linearForce, relativeLinear);
		var torque = overrideAngular ? angular : GetRotationTarget (distanceVector);
		forceSource.applyAngularForce(this.rigidbody, torque, relativeAngular);
	}
	
	public void moveTo (Vector3 target)
	{
		this.follow (this.gameObject);
		this.moveTarget = target;
	}
	
	public void follow (GameObject target)
	{
		this.followTarget = target;
	}
}
