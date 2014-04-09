using UnityEngine;
using System.Collections;


public class Collectable : MonoBehaviour
{
	public bool isCollectable = true;
	
	void OnTriggerEnter(Collider other)
	{
		if (isCollectable) {
			other.GetComponent<Collector>().collection.Add(gameObject);
			isCollectable = false;
			transform.parent = other.transform;
			gameObject.SetActive(false);
		}
	}

	
	void OnTriggerExit()
	{
		if (!isCollectable)
			isCollectable = true;
	}
}