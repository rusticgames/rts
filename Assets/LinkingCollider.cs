using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinkingCollider : MonoBehaviour {
	
	// OnCollisionEnter is called when this
	// collider/rigidbody has begun touching
	// another rigidbody/collider.
	void OnCollisionEnter (Collision collision) {
		Linkable linkee = collision.gameObject.GetComponent<Linkable>();
		if (linkee != null) { 
			FixedJoint joint = this.gameObject.AddComponent<FixedJoint>();
			joint.connectedBody = linkee.gameObject.rigidbody;
		}
	}
}
