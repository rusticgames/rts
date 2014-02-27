using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour {
	
	private GameObject player;
	private GameObject selectedUnit;
	private Camera mainCamera = new Camera();
	private CameraSettings camStart = new CameraSettings();
	private CameraSettings camTop = new CameraSettings();
	private CameraSettings camISO = new CameraSettings();

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		mainCamera = Camera.main;

		camStart.position = mainCamera.transform.position;
		camStart.rotation = mainCamera.transform.localEulerAngles;

		camTop.position = new Vector3(0, 60f, 0);
		camTop.rotation = new Vector3(90f, 0, 0);

		camISO.position = new Vector3(0, 0, 0);
		camISO.rotation = new Vector3(30f, 315f, -8f);
		camISO.othographic = true;
		camISO.orthographicSize = 20f;
		camISO.nearClipPlane = -35f;
	}
	
	void OnGUI () {
		GUILayout.BeginArea(new Rect(10f, 10f, 100, 200));

		GUILayout.Label("Camera");

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Top")) {
			DetachCamera();
			UpdateCamera(camTop);
		}
		if (GUILayout.Button("ISO")) {
			DetachCamera();
			UpdateCamera(camISO);
		}
		GUILayout.EndHorizontal();

		if (GUILayout.Button("Reset")) {
			DetachCamera();
			UpdateCamera(camStart);
		}

		GUILayout.EndArea();
	}

	void UpdateCamera(CameraSettings settings) {
		mainCamera.transform.position = settings.position;
		mainCamera.transform.localEulerAngles = settings.rotation;
		mainCamera.orthographic = settings.othographic;
		mainCamera.orthographicSize = settings.orthographicSize;
		mainCamera.nearClipPlane = settings.nearClipPlane;
	}

	void AttachCamera() {
		if (selectedUnit != null)
			mainCamera.transform.parent = selectedUnit.transform;
	}

	void DetachCamera() {
		if (mainCamera.transform.parent != null)
			mainCamera.transform.parent = null;
	}
}

public class CameraSettings {
	public Vector3 position;
	public Vector3 rotation;
	public bool othographic = false;
	public float orthographicSize = 5f;
	public float nearClipPlane = 0.3f;
}