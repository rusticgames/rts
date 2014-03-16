using UnityEngine;
using System.Collections;

[AddComponentMenu("Movement/Constant Rotation")]
public class CoRotator : MonoBehaviour {
	public Vector3 rotationRates;
	public float timeInterval = 0.1f;
	public bool keepOnKeepingOn = true;
	

	void Start () {
		StartCoroutine("rotate");

	}


	IEnumerator rotate () {
		while(keepOnKeepingOn) {
			transform.Rotate (rotationRates);
			yield return new WaitForSeconds(timeInterval);
		}
	}
}
