using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour {

	public Camera mouseCamera;
	public Canvas canvas;
	public GameObject panelPrefab;
	public PhysicalPropertiesManager ppm;

	private GameObject lastClicked;

	void Start () {
		StartCoroutine(checkInput());
	}
	
	IEnumerator checkInput() {
		while(true) {
			if (Input.GetMouseButtonDown(0)) {
				Ray cameraRay = mouseCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(cameraRay, out hit)) {
					PhysicalProperties p = hit.collider.gameObject.GetComponent<PhysicalProperties>();
					if (p != null) {
						ButtonMaker b = ((GameObject)GameObject.Instantiate(panelPrefab)).GetComponent<ButtonMaker>();
						b.objectToModify = p;
						b.ppm = ppm;
						b.transform.SetParent(canvas.transform, false);
					}
					this.lastClicked = hit.collider.gameObject;
				}
			}
			yield return null;
		}
	}
}
