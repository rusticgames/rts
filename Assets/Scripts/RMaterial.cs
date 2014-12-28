using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RMaterialProperties {
	public float mass;
	public Material renderMaterial;
}
public class RMaterial : MonoBehaviour {
	public delegate void UpdateRMaterialEvent (RMaterialProperties t);
	//class UpdateRMaterialEvent : UnityEngine.Events.UnityEvent<RMaterial> {};
	public RMaterialProperties properties;
	public UpdateRMaterialEvent update = noop;

	void Start () {
	}

	public static void noop (RMaterialProperties t) {}

	public void UpdateTemplate(RMaterialProperties t) {
		this.properties = t;
		update = UpdateTemplate;
		resetPropertiesToTemplate (t);
	}

	void resetPropertiesToTemplate (RMaterialProperties t)
	{
		this.rigidbody.mass = t.mass;
		this.GetComponent<MeshRenderer> ().material = t.renderMaterial;
	}
}