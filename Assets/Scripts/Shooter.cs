using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	public GameObject projectilePrefab;
	public float projectileSpeed = 20.0f;

	public Object Use(Vector3 position, Quaternion rotation, Vector3 heading) {
		GameObject o = (GameObject)Instantiate(projectilePrefab, position, rotation);
		o.rigidbody.AddForce(heading * projectileSpeed, ForceMode.Force);
		return o;
	}
}