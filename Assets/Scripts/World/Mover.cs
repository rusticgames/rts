using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mover : MonoBehaviour
{
	Vector3 moveTarget;
	GameObject followTarget;
	public float followDistance = 1.0f;
	public float maximumVelocity = 1.0f;
	
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

	void try2DMove ()
	{
		Vector2 moveDirection;
		Vector3 desiredVelocity = new Vector3 (0, 0, 0);

		if (followTarget != this.gameObject) {
			moveTarget = followTarget.transform.position;
		}
		moveDirection.x = moveTarget.x - this.transform.position.x;
		moveDirection.y = moveTarget.z - this.transform.position.z;

		transform.eulerAngles = new Vector3 (90, 0, (180 - 360 * Mathf.Atan2 (moveDirection.x, moveDirection.y) / (2*Mathf.PI)));
		
		if (moveDirection.sqrMagnitude > (followDistance * followDistance)) {
			moveDirection.Normalize ();
			desiredVelocity.x = maximumVelocity * moveDirection.x;
			desiredVelocity.z = maximumVelocity * moveDirection.y;
		}

		Vector3 deltaVelocity = desiredVelocity - rigidbody.velocity;
		rigidbody.AddForce (deltaVelocity, ForceMode.Acceleration);
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
