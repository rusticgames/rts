using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RMaterialManager : MonoBehaviour {
	[System.Serializable]
	public class CanonicalRMaterial {
		public string materialName;
		public RMaterialProperties materialTemplate;
		public CanonicalRMaterial(string s, RMaterialProperties mp) {
			this.materialName = s;
			this.materialTemplate = mp;
		}
	}

	// I'm trying something weird here, which is giving us a bunch of different ways to provide material templates 
	// to this class, then having the class interpret them all into it's own format

	public List<RMaterial> prefabsToCanonize = new List<RMaterial>();
	public List<CanonicalRMaterial> templates = new List<CanonicalRMaterial>();
	public void Start() {
		prefabsToCanonize.ForEach(ptc => RMaterialManager.CanonizeMaterial(this, ptc.name, ptc));
	}

	public static RMaterialManager CanonizeMaterial(RMaterialManager mm, string s, RMaterial m) {
		mm.templates.Add(new CanonicalRMaterial(s, m.properties));
		return mm;
	}
}
