using UnityEngine;
using System.Collections;

public class MoveTarget : MonoBehaviour {

	public GameObject mover;
	public float moverSpeed = 1.0f;

	private Vector3 target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		target = transform.position;
		float step = moverSpeed * Time.deltaTime;
		mover.transform.position = Vector3.MoveTowards(mover.transform.position, target, step);
	}
}