using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HUD : MonoBehaviour {
	public Camera mainCamera;
	public Camera insetCamera = null;
	public ScreenToWorldMapper mapper;

	private CameraSettings camStart = new CameraSettings();
	private CameraSettings camTop = new CameraSettings();
	private CameraSettings camISO = new CameraSettings();
	private CameraSettings camUnit = new CameraSettings();

	private Rect GUIArea = new Rect(10f, 10f, 100, 200);

	void Start() {
		mapper = new ScreenToWorldMapper();
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
		if (insetCamera == null || !insetCamera.enabled) { return mainCamera; }
		else {
			Debug.Log("inset");
			return (insetCamera.pixelRect.Contains(point)) ? insetCamera : mainCamera;
    }
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
			Debug.LogError("unit selection not implemented for camera");
			/*if (selection.selectedUnits.Count > 0) {
				camUnit.parentTransform = selection.LastSelected.transform;
				UpdateCamera(camUnit);
			}*/
		}

		if (GUILayout.Button("Reset")) {
			UpdateCamera(camStart);
		}

		if (insetCamera != null) {
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
				Debug.LogError("unit selection not implemented for camera");
				/*if (selection.selectedUnits.Count > 0) {
					insetCamera.enabled = true;
					camUnit.parentTransform = selection.LastSelected.transform;
					UpdateCamera(insetCamera, camUnit);
				}*/
			}
			
			if (GUILayout.Button("Reset")) {
				insetCamera.enabled = false;
			}
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

	void AttachCamera(GameObject g) {
		mainCamera.transform.parent = g.transform;
	}

	public bool isScreenPointValid(Vector3 screenPoint) {
		return mapper.IsScreenPointOverObject(screenPoint, getBestGuessCameraFromScreenPoint(screenPoint));
	}
	
	public GameObject getObjectAtScreenPoint ()
	{
		return mapper.LastHitObject;
	}
	
	public Vector3 getWorldPointAtScreenPoint ()
	{
		return mapper.LastHitInfo.point;
	}

	public List<GameObject> getAllObjectsInScreenBox (Rect box, Vector3 startPoint)
	{
		List<GameObject> boxContained = new List<GameObject>();
		GameObject[] allObjects = GameObject. FindGameObjectsWithTag("Unit"); //TODO: re-evaluate where this tag should go
		Camera currentCamera = getBestGuessCameraFromScreenPoint(startPoint);

		foreach (GameObject o in allObjects) {
			Vector2 pos = currentCamera.WorldToScreenPoint(o.transform.position);
			if (box.Contains(pos)) boxContained.Add(o);
		}
		return boxContained;
	}
}