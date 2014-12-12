using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class History : MonoBehaviour {
	private Vector3 initialPosition;
	private Vector3 initialLinearVelocity;
	private Vector3 initialAngularVelocity;
	private Quaternion initialRotation;


	public List<GameObject> destroyableChildren = new List<GameObject>();
	void Start () {
		initialPosition = this.transform.position;
		initialLinearVelocity = this.rigidbody.velocity;
		initialAngularVelocity = this.rigidbody.angularVelocity;
		initialRotation = this.transform.rotation;
	}

	public void ResetToInitial() {
		List<GameObject> cl = destroyableChildren;
		destroyableChildren = new List<GameObject>();
		this.transform.position = initialPosition;
		this.rigidbody.velocity = initialLinearVelocity;
		this.rigidbody.angularVelocity = initialAngularVelocity;
		this.transform.rotation = initialRotation;
		cl.ForEach(x => GameObject.Destroy(x));
		cl.Clear();
	}

	public void addChild (GameObject o)
	{
		destroyableChildren.Add (o);
	}
}
