using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectUnits : MonoBehaviour
{
	private const bool GAME_IS_RUNNING = true;
	public HashSet<GameObject> selectedUnits = new HashSet<GameObject>();

	//it occurs to me, maybe for the first time, that we could limit selection 
	//to certain cameras. are there interesting possibilities with modal 
	//interactions (spoiler: yes vi-rts)
	public HUD cameraProvider;
	public float selectBoxDelay = 0.1f;
	private GUIBox dragSelect;

	void Start()
	{
		dragSelect = new GUIBox();
		StartCoroutine(CheckSelect());
		StartCoroutine(CheckMove());
	}

	private MouseToWorldMapper mouseCheck = new MouseToWorldMapper();
	IEnumerator CheckSelect(){
		while(GAME_IS_RUNNING){
			while(!(Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)) {
				yield return null;
			}

			if (!(Input.GetKey(KeyCode.LeftShift)
			    || Input.GetKey(KeyCode.RightShift)
			    || Input.GetKey(KeyCode.LeftControl)
			    || Input.GetKey(KeyCode.RightControl))) {
				ClearSelectedUnits();
			}

			Camera c = cameraProvider.getBestGuessCameraFromScreenPoint(Input.mousePosition);
			Vector3 clickPoint = Input.mousePosition;

			if(mouseCheck.IsMouseOverObject(c) && mouseCheck.LastMouseHit.collider.tag == "Unit") {
				if(Input.GetKey(KeyCode.LeftControl)||Input.GetKey(KeyCode.RightControl)){
					ToggleSelectedUnit(mouseCheck.LastMouseHit.collider.gameObject);
				} else {
					AddToSelectedUnits(mouseCheck.LastMouseHit.collider.gameObject);
				}
			}

			yield return new WaitForSeconds(selectBoxDelay);
			if(!Input.GetMouseButton(0)) { continue; }

			while(Input.GetMouseButton(0)) {
				UpdateSelectBox(clickPoint,Input.mousePosition);
				yield return null;
			}
			FinishBoxSelection(c);
		}
	}

	IEnumerator CheckMove(){
		while(GAME_IS_RUNNING){
			while(!(Input.GetMouseButtonDown(1) 
		        && GUIUtility.hotControl == 0
		        && selectedUnits.Count > 0
			    && mouseCheck.IsMouseOverObject(cameraProvider.getBestGuessCameraFromScreenPoint(Input.mousePosition))
	    	)) {
				yield return null;
			}
			
			foreach (GameObject unit in selectedUnits) {
				if (mouseCheck.LastMouseHit.collider.tag == "Unit") {
					((Mover)unit.GetComponent<Mover>()).follow(mouseCheck.LastMouseHit.collider.gameObject);
				} else {
					((Mover)unit.GetComponent<Mover>()).moveTo(mouseCheck.LastMouseHit.point);
				}
			}

			yield return null;
		}
	}

	void moveIt(GameObject unit, RaycastHit hitinfo) {
	}
	
	void moveTo(GameObject unit, RaycastHit hitinfo) {

	}

	delegate void GameObjectDelegate(GameObject o);

	void OnGUI()
	{
		GUI.Box(dragSelect.ScreenRect, GUIContent.none, dragSelect.Style);
	}

	void UpdateSelectBox(Vector3 startPoint, Vector3 endPoint)
	{
		Vector2 pos = new Vector2(startPoint.x, startPoint.y);
		Vector2 size = new Vector2(endPoint.x - startPoint.x, endPoint.y - startPoint.y);
		dragSelect.UpdateBoxFromScreen(pos, size);
	}

	void FinishBoxSelection(Camera currentCamera)
	{
		GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
		Rect box = dragSelect.WorldRect;

		foreach (GameObject unit in allUnits) {
			Vector2 pos = currentCamera.WorldToScreenPoint(unit.transform.position);
			if (box.Contains(pos)) AddToSelectedUnits(unit);
		}

		dragSelect.ClearBox();
	}

	void AddToSelectedUnits(GameObject unit)
	{
		selectedUnits.Add(unit);
		ChangeUnitColor(unit, Color.green);
	}
	
	void RemoveFromSelectedUnits(GameObject unit)
	{
		selectedUnits.Remove(unit);
		ChangeUnitColor(unit, Color.white);
	}
	
	void ToggleSelectedUnit(GameObject unit)
	{
		if (selectedUnits.Contains(unit)) {
			RemoveFromSelectedUnits(unit);
		} else {
			AddToSelectedUnits(unit);
		}
	}

	void ClearSelectedUnits()
	{
		foreach (GameObject unit in selectedUnits) {
			ChangeUnitColor(unit, Color.white);
		}
		selectedUnits.Clear();
	}
	

	void ChangeUnitColor(GameObject unit, Color color)
	{
		unit.renderer.material.color = color;
	}
}