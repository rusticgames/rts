using UnityEngine;
using System.Collections;

public class MoveLeftAndRight : MonoBehaviour
{

	public float speed = 6.0f;
	public float maxSpeed = 12.0f;

	void Update()
	{
		Vector3 newPosition = transform.position;
		newPosition.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		transform.position = newPosition;
	}
}
