using UnityEngine;
using System.Collections;

public class OneWayFollow : MonoBehaviour {
	public GameObject target;
	public Vector3 followDirection = new Vector3(1,0,0);
	public Vector3 positionDifference;
	public Vector3 newLocation;

	void Reset () {
		target = this.gameObject;
		followDirection = new Vector3(1,0,0);
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null) {target = this.gameObject;}
		if(target != this.gameObject) {
			tryUpdate();
		} else {
			tryFindTarget();
		}
	}
	
	public void tryFindTarget() {
		GameObject newTarget = GameObject.Find("Mario(Clone)");
		if(newTarget != null) target = newTarget;
	}
	
	public void tryUpdate() {
		positionDifference = (target.transform.position - this.transform.position);
		newLocation = positionDifference;
		if((newLocation.x * followDirection.x) <= 0) newLocation.x = 0;
		if((newLocation.y * followDirection.y) <= 0) newLocation.y = 0;
		if((newLocation.z * followDirection.z) <= 0) newLocation.z = 0;
		
		this.transform.position = this.transform.position + newLocation;
	}
}
