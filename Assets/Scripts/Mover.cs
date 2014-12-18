using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mover : MonoBehaviour
{
	Vector3 moveTarget;
	GameObject followTarget;
	GameObject attackTarget;
	public float followDistance = 1.0f;	
	public float attackRange = 10f;
	public float maximumVelocity = 1.0f;
	[Range (0, 100)]
	public float maximumAngularVelocity = 1.0f;
	public Mechanism forceSource;
	public Mechanism projectileForceSource;
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
	public GameObject projectilePrefab;
	public bool DEBUG_MODE = false;
	
	// Start is called just before any of the
	// Update methods is called the first time.
	void Start ()
	{
		if (followTarget == null) {
			this.follow (this.gameObject);
		}
		moveTarget = followTarget.transform.position;
		StartCoroutine(attackRoutine());
		StartCoroutine(moveRoutine());
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
		
	IEnumerator moveRoutine() {
		while(SelectUnits.GAME_IS_RUNNING) {
			if (followTarget != this.gameObject) {
				moveTarget = followTarget.transform.position;
			}
			
			distanceVector = GetDistanceToTarget (moveTarget);
			lastLinearForce = GetDesiredVelocity (distanceVector);
			lastAngularForce = GetRotationTarget (distanceVector);
			forceSource.applyLinearForce(this.rigidbody, lastLinearForce, relativeLinear);
			forceSource.applyAngularForce(this.rigidbody, lastAngularForce, relativeAngular);
			if(isJumping && Physics.Raycast(this.transform.position, Vector3.down, 1.1f)){
				isJumping = false;
				projectileForceSource.applyLinearForce(this.rigidbody, jumpForce, relativeLinear);
				yield return new WaitForSeconds(jumpDelay);
			}
			yield return new WaitForFixedUpdate();
		}
	}
	public Vector3 jumpForce = new Vector3(0.0f, 3f, 0f);
	bool isJumping = false;
	float attackPeriodSeconds = 1f;
	public float jumpDelay = 1f;

	float projectileSpeed = 20f;
	
	IEnumerator attackRoutine() {
		while(SelectUnits.GAME_IS_RUNNING) {
			while(attackTarget != null && attackTarget.activeInHierarchy) {
				Vector3 distance = GetDistanceToTarget(attackTarget.transform.position);
				if(distanceVector.magnitude < attackRange) {
					Vector3 startPosition = this.transform.position;
					startPosition.y = startPosition.y + 3;
					
					GameObject o = (GameObject)Instantiate(projectilePrefab, startPosition, this.transform.rotation);
					
					projectileForceSource.applyLinearForce(o.rigidbody, distance.normalized * projectileSpeed, relativeLinear);
					yield return new WaitForSeconds(attackPeriodSeconds);
				}
				yield return new WaitForFixedUpdate();
			}
			yield return new WaitForFixedUpdate();
		}
	}

	public void moveTo (Vector3 target)
	{
		if(DEBUG_MODE) Debug.Log("moveto");
		this.follow (this.gameObject);
		this.moveTarget = target;
	}
	
	public void follow (GameObject target)
	{
		if(DEBUG_MODE) Debug.Log("follow");
		this.followTarget = target;
	}
	
	public void attack (GameObject target)
	{
		if(DEBUG_MODE) Debug.Log("attack");
		this.attackTarget = target;
	}
	
	public void stop ()
	{
		if(DEBUG_MODE) Debug.Log("stop");
		this.moveTo(this.transform.position);
		this.attackTarget = null;
	}

	public void jump () {		
		if(DEBUG_MODE) Debug.Log("stop");
		this.isJumping = true;
	}
}
