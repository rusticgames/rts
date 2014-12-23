using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour {

	public Camera mouseCamera;
	public Canvas canvas;
	public GameObject panelPrefab;
	public RMaterialManager rmaterialManager;

	void Start () {
		StartCoroutine(checkInput());
	}
	
	IEnumerator checkInput() {
		while(true) {
			if (Input.GetMouseButtonDown(0)) {
				Ray cameraRay = mouseCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(cameraRay, out hit)) {
					RMaterial p = hit.collider.gameObject.GetComponent<RMaterial>();
					if (p != null) {
						ButtonMaker b = ((GameObject)GameObject.Instantiate(panelPrefab)).GetComponent<ButtonMaker>();
						b.objectToModify = p;
						b.manager = rmaterialManager;
						b.transform.SetParent(canvas.transform, false);
					}
				}
			}
			yield return null;
		}
	}
}
