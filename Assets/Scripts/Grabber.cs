using UnityEngine;
using System.Collections;

public class Grabber : MonoBehaviour {
	public GameObject grabee;
	public Joint grip;
	public float grabRange = 2f;
	
	public void release() {
		Joint oldGrip = grip;
		grip = null;
		Object.Destroy(oldGrip);
		grabee = null;
	}
	public void use(ToolUseData d) {
		if(grabee != null) {
			release();
			return;
		}
		RaycastHit info;
		Ray checkRay = new Ray(d.position, d.heading);
		if(Physics.Raycast(checkRay, out info, grabRange)) {
			grabee = info.collider.gameObject;
			grip = this.gameObject.AddComponent<FixedJoint>();
			grip.connectedBody = grabee.gameObject.rigidbody;
		}
	}
}