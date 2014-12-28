using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinkingCollider : MonoBehaviour {
	public System.Type jointType = typeof(FixedJoint);
	public string typeOfJoint;
	public Joint joint;
	// OnCollisionEnter is called when this
	// collider/rigidbody has begun touching
	// another rigidbody/collider.
	void OnCollisionEnter (Collision collision) {
		Linkable linkee = collision.gameObject.GetComponent<Linkable>();
		if (linkee != null) { 
			joint = this.gameObject.AddComponent<FixedJoint>();
			joint.connectedBody = linkee.gameObject.rigidbody;
		}
	}
}
