using UnityEngine;
using System.Collections;

public class ConstrainedMover : MonoBehaviour {

	public float speed = 1.0f;
	public float constraintRadius = 3.0f;
	
	private Vector3 constraintCenter;

	void Start() {
		constraintCenter = transform.position;
	}

	void Update() {
		Vector3 newPosition = constraintCenter;
		newPosition.x += Input.GetAxis("Horizontal") * constraintRadius;
		newPosition.y += Input.GetAxis("Vertical") * constraintRadius;

		transform.position = newPosition;


		Debug.Log (Input.GetAxis("Horizontal") + " (" + newPosition.x + ")" + ", " + Input.GetAxis("Vertical") + " (" + newPosition.y + ")");
		
		// Vector3 normalizedPosition = Vector3.Normalize(newPosition);
		// transform.position = normalizedPosition;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(constraintCenter, constraintRadius);
	}
}
