// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenToWorldMapper {
	int freshestFrame = 0;
	bool hitThisFrame = false;
	RaycastHit hitInfo = new RaycastHit();
	Ray ray = new Ray();
	
	public bool IsScreenPointOverObject(Vector3 screenPoint, Camera perspective) {
		if(	UnityEngine.Time.frameCount > freshestFrame ) {
			freshestFrame = UnityEngine.Time.frameCount;
			
			hitInfo = new RaycastHit();
			ray = perspective.ScreenPointToRay(screenPoint);
			hitThisFrame = Physics.Raycast(ray, out hitInfo);
		}
		return hitThisFrame;
	}

	public RaycastHit LastHitInfo {
		get {
			return hitInfo;
		}
	}
	
	public GameObject LastHitObject {
		get {
			return hitInfo.collider.gameObject;
		}
	}
}
