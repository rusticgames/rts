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
	void Update ()
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
		Vector3 orientationTarget = new Vector3 (90, 0, (180 - 360 * Mathf.Atan2 (distanceVector.x, distanceVector.y) / (2 * Mathf.PI)));
		orientationTarget = orientationTarget - this.rigidbody.rotation.eulerAngles;
		orientationTarget.x = 0f;
		orientationTarget.y = 0f;
		if(orientationTarget)
		orientationTarget.z *= 0.1f;
		return orientationTarget;
	}

	void try2DMove ()
	{
		var distanceVector = GetDistanceToTarget ();

		var linearForce = GetDesiredVelocity (distanceVector);
		forceSource.applyLinearForce(this.rigidbody, linearForce);

		var torque = GetRotationTarget (distanceVector);
		forceSource.applyAngularForce(this.rigidbody, torque);

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
