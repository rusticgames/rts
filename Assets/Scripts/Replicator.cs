using UnityEngine;
using System.Collections;

public class Replicator : MonoBehaviour {

	public float replicationInterval = 1.0f;
	public Vector3 replicationAxes;
	public bool replicate = false;
	
	void Start() {
		StartCoroutine(Replicate());
	}
	
	IEnumerator Replicate() {
		while(true) {
			yield return new WaitForSeconds(replicationInterval);
			if (replicate) {
				Vector3 newPosition = transform.position + replicationAxes;
				GameObject o = (GameObject)Instantiate(gameObject, newPosition, transform.rotation);
				o.GetComponent<Replicator>().replicate = false;
			}
		}
	}
}
