using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mover : MonoBehaviour
{
	Vector3 moveTarget;
	GameObject followTarget;
	public float followDistance = 1.0f;
	public float maximumVelocity = 1.0f;
	[Range (0, 100)]
	public float maximumAngularVelocity = 1.0f;
	public Mechanism forceSource;
	public bool overrideLinear = false;
	public bool overrideAngular = false;
	public bool relativeLinear = false;
	public bool relativeAngular = false;
	public Vector3 linear;
	public Vector3 angular;
	public Vector3 lastLinearForce;
	public Vector3 lastAngularForce;
	public float minimumAngularOffset = 5.0f;
	private Vector3 distanceVector;
	
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

	void OnDrawGizmos () {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(moveTarget, 1f);
		Gizmos.DrawLine(this.transform.position, moveTarget);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(this.transform.position, transform.up);
		Gizmos.color = Color.white;
		Gizmos.DrawRay(this.transform.position, transform.forward);
	}

	Vector3 GetDistanceToTarget ()
	{
		if (followTarget != this.gameObject) {
			moveTarget = followTarget.transform.position;
		}

		return moveTarget - this.rigidbody.position;
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
	
	Vector3 GetRotationTarget (Vector3 distance)
	{
		if(overrideAngular) return angular;
		Vector3 currentOrientation = this.rigidbody.rotation.eulerAngles;
		Vector3 targetTorque = currentOrientation;
		targetTorque.y = 180 * Mathf.Atan2 (distance.x, distance.z) / Mathf.PI;
		targetTorque -= currentOrientation;
		if(targetTorque.y > 180.0f) targetTorque.y -= 360.0f;
		if(targetTorque.y < -180.0f) targetTorque.y += 360.0f;
		if(Mathf.Abs(targetTorque.y) < minimumAngularOffset) {
			targetTorque.y = 0;
		}
		return targetTorque - rigidbody.angularVelocity;
	}
	
	void try2DMove ()
	{
		distanceVector = GetDistanceToTarget ();
		lastLinearForce = GetDesiredVelocity (distanceVector);
		lastAngularForce = GetRotationTarget (distanceVector);
		forceSource.applyLinearForce(this.rigidbody, lastLinearForce, relativeLinear);
		forceSource.applyAngularForce(this.rigidbody, lastAngularForce, relativeAngular);
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
