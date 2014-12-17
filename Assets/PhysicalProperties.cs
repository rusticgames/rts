using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicalProperties : MonoBehaviour {
	
	public PhysicalPropertyTemplate propertiesTemplate;

	// Start is called just before any of the
	// Update methods is called the first time.
	void Start () {
		this.rigidbody.mass = propertiesTemplate.mass;
		this.GetComponent<MeshRenderer>().material = propertiesTemplate.renderMaterial;
	}
	
}
