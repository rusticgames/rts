using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaleWithCollection : MonoBehaviour
{
	public enum Mode {Grow, Shrink}

	public Mode mode;
	public float scaleFactor = 1.0f;

	private List<GameObject> collection;

	void Start()
	{
		collection = gameObject.GetComponent<Collector>().collection;
	}

	void Update()
	{
		float multiplier = collection.Count * scaleFactor;
		if (mode == Mode.Shrink)
			multiplier = -multiplier;
		if (multiplier == 0)
			multiplier = 1;
		transform.localScale = transform.localScale * multiplier;
	}
}
