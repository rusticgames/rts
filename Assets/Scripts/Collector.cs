using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collector : MonoBehaviour
{
	public List<GameObject> collection;
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && collection.Count > 0)
			DropCollectable(collection[0]);
	}

	void DropCollectable(GameObject collectable)
	{
		collectable.transform.parent = null;
		collectable.SetActive(true);
		collectable.GetComponentInChildren<CoRotator>().StartCoroutine("rotate");
		collection.Remove(collectable);
	}
}
