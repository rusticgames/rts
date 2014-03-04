using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectUnits : MonoBehaviour
{
	public List<GameObject> selectedUnits;

	private Vector3 lastClickPoint;
	private DragSelect dragSelect;

	void Start()
	{
		dragSelect = new DragSelect();
	}
	
	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo = new RaycastHit();

		if (Input.GetMouseButtonDown(0)) {
			if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == "Unit") {
				GameObject unit = hitInfo.collider.gameObject;

				if (!Input.GetKey(KeyCode.LeftShift))
					ClearSelectedUnits();

				AddToSelectedUnits(unit);
			} else if (Physics.Raycast(ray, out hitInfo)) {
				ClearSelectedUnits();
			}

			lastClickPoint = Input.mousePosition;
		}

		if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0) {
			if (Physics.Raycast(ray, out hitInfo)) {
				Mover seeker;
				if (hitInfo.collider.tag == "Unit") {
					GameObject target = hitInfo.collider.gameObject;

					selectedUnits.ForEach(delegate(GameObject unit) {
						seeker = (Mover)unit.GetComponent<Mover>();
						seeker.follow(target);
					});
				} else {
					selectedUnits.ForEach(delegate(GameObject unit) {
						seeker = (Mover)unit.GetComponent<Mover>();
						seeker.moveTo(hitInfo.point);
					});
				}
			}
		}
	}

	void OnGUI()
	{
		GUI.Box(dragSelect.GUIBox, GUIContent.none, dragSelect.Style);
		if (Input.GetMouseButton(0)) DrawSelectBox();
		if (Input.GetMouseButtonUp(0)) StopDrawingSelectBox();
	}

	void DrawSelectBox()
	{
		Vector2 pos = new Vector2(lastClickPoint.x, lastClickPoint.y);
		Vector2 size = new Vector2(Input.mousePosition.x - lastClickPoint.x, Input.mousePosition.y - lastClickPoint.y);
		dragSelect.UpdateBox(pos, size);
	}

	void StopDrawingSelectBox()
	{
		GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
		Rect box = dragSelect.Box;

		foreach (GameObject unit in allUnits) {
			Vector2 pos = Camera.main.WorldToScreenPoint(unit.transform.position);
			if (box.Contains(pos)) AddToSelectedUnits(unit);
		}

		dragSelect.ClearBox();
	}

	void AddToSelectedUnits(GameObject unit)
	{
		if (!selectedUnits.Contains(unit)) {
			selectedUnits.Add(unit);
			ChangeUnitColor(unit, Color.green);
		}
	}

	void RemoveFromSelectedUnits(GameObject unit)
	{
		if (selectedUnits.Contains(unit)) {
			selectedUnits.Remove(unit);
			ChangeUnitColor(unit, Color.white);
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