using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Replicator : MonoBehaviour {

	public float replicationInterval = 1.0f;
	public List<Vector3> replicationAxes = new List<Vector3>();
	public bool replicate = false;
	public bool childrenShouldReplicate = false;
	public float childrenReplicationIntervalRepeatFactor = 1.0f;

	void Start() {
		StartCoroutine(Replicate());
	}
	
	IEnumerator Replicate() {
		while(true) {
			yield return new WaitForSeconds(replicationInterval);
			if(replicate) {
				replicationAxes.ForEach(axis => {
					Vector3 newPosition = transform.position + axis;
					GameObject o = (GameObject)Instantiate(gameObject, newPosition, transform.rotation);
					o.GetComponent<Replicator>().replicate = childrenShouldReplicate;
					o.GetComponent<Replicator>().replicationInterval	= replicationInterval * childrenReplicationIntervalRepeatFactor;
				});
			}
		}
	}
}
