using UnityEngine;
using System.Collections;

public class CoRotator : MonoBehaviour {
	public Vector3 rotationRates;
	
	// Use this for initialization
	void Start () {
		StartCoroutine("rotate");
	}

	IEnumerator rotate () {
		while(true) {
			transform.Rotate (rotationRates);
			yield return new WaitForSeconds(.1f);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
