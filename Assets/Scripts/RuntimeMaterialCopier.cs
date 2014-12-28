using UnityEngine;
using System.Collections;

public class RuntimeMaterialCopier : MonoBehaviour {
	public GameObject source;
	// Use this for initialization
	void Start () {
		RMaterial sm = source.GetComponent<RMaterial>();
		RMaterial mm = this.gameObject.AddComponent<RMaterial>();
		mm.UpdateTemplate(sm.properties);
	}
}
