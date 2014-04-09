using UnityEngine;
using System.Collections;

public class Creator : MonoBehaviour
{
	public GameObject spawnObject;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightShift)) {
			GameObject.Instantiate(spawnObject, transform.position, transform.rotation);
		}
	}
}
