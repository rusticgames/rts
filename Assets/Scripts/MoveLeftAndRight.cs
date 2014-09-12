using UnityEngine;
using RusticGames.Act;
using System.Collections;

public class MoveLeftAndRight : MonoBehaviour
{
	public Move moveAction;
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine (thinkRoutine ());
		StartCoroutine (moveAction.moveRoutine(this.gameObject));
	}
	
	IEnumerator thinkRoutine ()
	{
		while (true) {
			moveAction.moveDirection.x = Input.GetAxis("Horizontal");
			yield return new WaitForFixedUpdate ();
		}
	}

	public float speed = 6.0f;
	public float maxSpeed = 12.0f;

}
