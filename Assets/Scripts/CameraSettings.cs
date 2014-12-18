// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CameraSettings {
	public Vector3 position;
	public Vector3 rotation;
	public bool othographic = false;
	public float orthographicSize = 5f;
	public float nearClipPlane = 0.3f;
	public Transform parentTransform = null;
}
