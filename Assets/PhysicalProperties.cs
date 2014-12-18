using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicalProperties : MonoBehaviour {
	
	public PhysicalPropertyTemplate propertiesTemplate;

	void Start () {
		updatePropertiesTemplate(propertiesTemplate);
	}

	public void updatePropertiesTemplate(PhysicalPropertyTemplate template) {
		this.propertiesTemplate = template;
		this.rigidbody.mass = template.mass;
		this.GetComponent<MeshRenderer>().material = template.renderMaterial;
	}
}
