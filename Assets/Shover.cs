using UnityEngine;
using System.Collections;

[AddComponentMenu("Physics 2D/Explicit Shover")]
public class Shover : MonoBehaviour {
	public Vector2 force;
	public ForceMode2D linearForceMode;
	public float torque;
	public ForceMode2D angularForceMode;
	public Vector2 desiredPosition;
	public float desiredRotation;
	public float applicationInterval = 1f;
	public bool clearForcesWhenApplied = true;
	public bool apply = false;
	
	void Start () {
		StartCoroutine(checkApply());
	}
	
	IEnumerator checkApply () {
		while(true) {
			if(apply) {
				this.rigidbody2D.AddForce(force, linearForceMode);
				this.rigidbody2D.AddTorque(torque, angularForceMode);
				this.rigidbody2D.MovePosition(desiredPosition);
				this.rigidbody2D.MoveRotation(desiredRotation);
				if (clearForcesWhenApplied) {
					force = Vector2.zero;
					torque = 0f;
				}
				desiredPosition = Vector2.zero;
				desiredRotation = 0f;
			}
			if (applicationInterval < 0.1f) {
				applicationInterval = 0.1f;
			}
			yield return new WaitForSeconds(applicationInterval);
		}
	}
}