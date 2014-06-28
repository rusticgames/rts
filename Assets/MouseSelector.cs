using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseSelector : MonoBehaviour {
	public float selectBoxDelay = 0.1f;
	public HUD cameraProvider;
	public SelectUnits controller;
	private ScreenToWorldMapper mouseChecker = new ScreenToWorldMapper();
	private GUIBox dragSelect = new GUIBox();
	private GUIStyle boxStyle;
	public List<GameObject> selection = new List<GameObject>();
	public GameObject selectedObject;
	
	void Start() {
		boxStyle = new GUIStyle();
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, new Color(0f,1f,0f,.1f));
		texture.Apply();
		boxStyle.normal.background = texture;
	}

	public IEnumerator select (Vector3 startPoint, SelectionType type){
		List<GameObject> newSelection = new List<GameObject>();
		if(type == SelectionType.MOUSE_POINT)
		{
			if (mouseChecker.IsScreenPointOverObject (startPoint, cameraProvider)) {
				newSelection.Add(mouseChecker.LastHitObject);
			}
		}
		
		if(type == SelectionType.MOUSE_BOX)
		{
			while (controller.intents.Contains (ControllerIntent.SPECIFY_POINT)) {
				UpdateSelectBox (startPoint, Input.mousePosition);
				yield return null;
			}

			FinishBoxSelection (cameraProvider.getBestGuessCameraFromScreenPoint(startPoint), newSelection);
		}
		selection = newSelection;

		yield return null;
}	
	
	void OnGUI()
	{
		GUI.Box(dragSelect.ScreenRect, GUIContent.none, boxStyle);
	}
	
	void UpdateSelectBox(Vector3 startPoint, Vector3 endPoint)
	{
		Vector2 pos = new Vector2(startPoint.x, startPoint.y);
		Vector2 size = new Vector2(endPoint.x - startPoint.x, endPoint.y - startPoint.y);
		dragSelect.UpdateBoxFromScreen(pos, size);
	}


	
	void FinishBoxSelection(Camera currentCamera, List<GameObject> boxSelected)
	{
		GameObject[] allObjects = GameObject. FindGameObjectsWithTag("Unit"); //TODO: re-evaluate where this tag should go
		Rect box = dragSelect.WorldRect;
		
		foreach (GameObject o in allObjects) {
			Vector2 pos = currentCamera.WorldToScreenPoint(o.transform.position);
			if (box.Contains(pos)) boxSelected.Add(o);
		}
		dragSelect.ClearBox();
	}
}



public enum SelectionType
{
	MOUSE_POINT,
	MOUSE_BOX
}