using UnityEngine;
using System.Collections;

public class Environment : MonoBehaviour {
	public float ambientEnergyFactor = 10f;
	public float ReleaseEnergy (float surfaceArea) {
		//Debug.Log ("energy intook: " + (surfaceArea * ambientEnergyFactor).ToString());
		return surfaceArea * ambientEnergyFactor;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
