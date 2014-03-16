using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour {
	public Camera mainCamera;
	public Camera insetCamera;
	public SelectUnits selection;
	private GameObject selectedUnit = null;

	private CameraSettings camStart = new CameraSettings();
	private CameraSettings camTop = new CameraSettings();
	private CameraSettings camISO = new CameraSettings();
	private CameraSettings camUnit = new CameraSettings();

	private Rect GUIArea = new Rect(10f, 10f, 100, 200);

	void Start() {
		camStart.position = mainCamera.transform.position;
		camStart.rotation = mainCamera.transform.localEulerAngles;
		
		camTop.position = new Vector3(0, 60f, 0);
		camTop.rotation = new Vector3(90f, 0, 0);

		camUnit.position = new Vector3(0, 0, 0);
		camUnit.rotation = new Vector3(90f, 180f, 0);

		camISO.position = new Vector3(0, 0, 0);
		camISO.rotation = new Vector3(30f, 315f, -8f);
		camISO.othographic = true;
		camISO.orthographicSize = 20f;
		camISO.nearClipPlane = -35f;
	}

	public Camera getBestGuessCameraFromScreenPoint(Vector3 point){
		return (insetCamera.pixelRect.Contains(point)) ? insetCamera : mainCamera;
	}

	void OnGUI () {
		GUILayout.BeginArea(GUIArea);

		GUILayout.Label("Camera");

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Top")) {
			UpdateCamera(camTop);
		}

		if (GUILayout.Button("ISO")) {
			UpdateCamera(camISO);
		}
		GUILayout.EndHorizontal();
		
		if (GUILayout.Button("Unit")) {
			if (selection.selectedUnits.Count > 0) {
				camUnit.parentTransform = selection.LastSelected.transform;
				UpdateCamera(camUnit);
			}
		}

		if (GUILayout.Button("Reset")) {
			UpdateCamera(camStart);
		}

		GUILayout.Label("Inset Camera");
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Top")) {
			insetCamera.enabled = true;
			UpdateCamera(insetCamera, camTop);
		}
		
		if (GUILayout.Button("ISO")) {
			insetCamera.enabled = true;
			UpdateCamera(insetCamera, camISO);
		}
		GUILayout.EndHorizontal();
		
		if (GUILayout.Button("Unit")) {
			if (selection.selectedUnits.Count > 0) {
				insetCamera.enabled = true;
				camUnit.parentTransform = selection.LastSelected.transform;
				UpdateCamera(insetCamera, camUnit);
			}
		}
		
		if (GUILayout.Button("Reset")) {
			insetCamera.enabled = false;
		}
		
		GUILayout.EndArea();
	}
	
	void UpdateCamera(CameraSettings settings) {
		UpdateCamera(mainCamera, settings);
	}
	
	void UpdateCamera(Camera camera, CameraSettings settings) {
		camera.transform.parent = settings.parentTransform;
		camera.transform.localPosition = settings.position;
		camera.transform.localEulerAngles = settings.rotation;
		camera.orthographic = settings.othographic;
		camera.orthographicSize = settings.orthographicSize;
		camera.nearClipPlane = settings.nearClipPlane;
	}

	void AttachCamera() {
		if (selectedUnit != null)
			mainCamera.transform.parent = selectedUnit.transform;
	}
}

public class CameraSettings {
	public Vector3 position;
	public Vector3 rotation;
	public bool othographic = false;
	public float orthographicSize = 5f;
	public float nearClipPlane = 0.3f;
	public Transform parentTransform = null;
}

public class MouseToWorldMapper {
	int freshestFrame = 0;
	bool hitThisFrame = false;
	RaycastHit hitInfo = new RaycastHit();
	Ray ray = new Ray();
	
	public bool IsMouseOverObject(Camera currentCamera) {
		if(	UnityEngine.Time.frameCount > freshestFrame ) {
			freshestFrame = UnityEngine.Time.frameCount;
			
			hitInfo = new RaycastHit();
			ray = currentCamera.ScreenPointToRay(Input.mousePosition);
			hitThisFrame = Physics.Raycast(ray, out hitInfo);
		}
		return hitThisFrame;
	}
	
	public RaycastHit LastMouseHit {
		get {
			return hitInfo;
		}
	}
}