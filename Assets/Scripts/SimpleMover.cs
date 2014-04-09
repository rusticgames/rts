using UnityEngine;
using System.Collections;

public class SimpleMover : MonoBehaviour
{
	public float speed = 1.0f;

	void Update ()
	{
		Vector3 newPosition = transform.position;
		newPosition.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		newPosition.y += Input.GetAxis("Vertical") * speed * Time.deltaTime;
		transform.position = newPosition;
	}
}
