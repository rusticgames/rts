using UnityEngine;
using System.Collections;

[AddComponentMenu("Physics 2D/Explicit Shover")]
public class Shover : MonoBehaviour {
	public Vector2 force;
	public ForceMode2D linearForceMode;
	public float torque;
	public ForceMode2D angularForceMode;
	public Vector2 desiredPosition;
	public bool useDesiredPosition = false;
	public float desiredRotation;
	public bool useDesiredRotation = false;
	public float applicationInterval = 1f;
	public bool clearWhenApplied = true;
	public bool apply = false;
	public bool continuousApply = false;
	
	void Start () {
		StartCoroutine(checkApply());
	}
	
	IEnumerator checkApply () {
		while(true) {
			if(apply || continuousApply) {
				this.rigidbody2D.AddForce(force, linearForceMode);
				this.rigidbody2D.AddTorque(torque, angularForceMode);
				if(useDesiredPosition) this.rigidbody2D.MovePosition(desiredPosition);
				if(useDesiredRotation) this.rigidbody2D.MoveRotation(desiredRotation);
				if (clearWhenApplied) {
					force = Vector2.zero;
					torque = 0f;
					useDesiredPosition = false;
					useDesiredRotation = false;
				}
				apply = false;
			}
			if (applicationInterval < 0.01f) {
				applicationInterval = 0.01f;
			}
			yield return new WaitForSeconds(applicationInterval);
		}
	}
}