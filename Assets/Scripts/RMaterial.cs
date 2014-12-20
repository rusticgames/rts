using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RMaterial : MonoBehaviour {
	
	public RMaterialTemplate template;

	void Start () {
		UpdateTemplate(template);
	}

	public void UpdateTemplate(RMaterialTemplate t) {
		this.template = t;
		this.rigidbody.mass = t.mass;
		this.GetComponent<MeshRenderer>().material = t.renderMaterial;
	}
}