using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseSelector : MonoBehaviour {
	public float selectBoxDelay = 0.2f; //TODO: add distance delay as well
	public HUD controllingHUD;
	public SelectUnits controller;
	private GUIBox dragSelect = new GUIBox();
	private GUIStyle boxStyle;
	public GameObject selectedObject;

	
	void Start() {
		boxStyle = new GUIStyle();
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, new Color(0f,1f,0f,.1f));
		texture.Apply();
		boxStyle.normal.background = texture;
	}
	
	public List<GameObject> selectPoint (Vector3 point){
		List<GameObject> newSelection = new List<GameObject>();
		
		HUD.ScreenPointToWorldInfo i = controllingHUD.getWorldInfoAtScreenPoint(point);
		if(i.isValid)
		{
			newSelection.Add(i.objectAtPoint);
		}
		
		return newSelection;
	}	
	
	public List<GameObject> selectBox (Vector3 startPoint, Vector3 endPoint){
		UpdateSelectBox(startPoint, endPoint);
		List<GameObject> newSelection = controllingHUD.getAllObjectsInScreenBox(dragSelect.WorldRect, startPoint);
		dragSelect.ClearBox();
		
		return newSelection;
	}
	
	void OnGUI()
	{
		GUI.Box(dragSelect.ScreenRect, GUIContent.none, boxStyle);
	}
	
	public void UpdateSelectBox(Vector3 startPoint, Vector3 endPoint)
	{
		Vector2 pos = new Vector2(startPoint.x, startPoint.y);
		Vector2 size = new Vector2(endPoint.x - startPoint.x, endPoint.y - startPoint.y);
		dragSelect.UpdateBoxFromScreen(pos, size);
	}
}