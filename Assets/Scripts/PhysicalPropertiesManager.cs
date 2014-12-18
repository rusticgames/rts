using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicalPropertiesManager : MonoBehaviour {

	public List<PhysicalPropertyTemplate> templates = new List<PhysicalPropertyTemplate>();
	
	public static void ApplyPhysicalPropertiesTemplate(PhysicalProperties p, PhysicalPropertyTemplate t) {
		p.updatePropertiesTemplate(t);
	}
}
