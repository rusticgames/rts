using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectUnits : MonoBehaviour {
	// public Material selectedMaterial;
	public List<GameObject> selectedUnits;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo = new RaycastHit();

		if (Input.GetMouseButtonDown(0)) {
			if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "Unit") {
				GameObject obj = hitInfo.collider.gameObject;

			    if (!selectedUnits.Contains(obj)) {
					if (!Input.GetKey(KeyCode.LeftShift)) ClearSelectedUnits();
					selectedUnits.Add(obj);
					obj.renderer.material.color = Color.green;
				}
			} else {
				ClearSelectedUnits();
			}
		}
	}

	void ClearSelectedUnits() {
		selectedUnits.ForEach(delegate(GameObject o) {
			o.renderer.material.color = Color.white;
		});
		selectedUnits.Clear();
	}
}