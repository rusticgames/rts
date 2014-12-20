using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RMaterialManager : MonoBehaviour {

	public List<RMaterialTemplate> templates = new List<RMaterialTemplate>();
	
	public static void ApplyTemplate(RMaterial m, RMaterialTemplate t) {
		m.UpdateTemplate(t);
	}
}
