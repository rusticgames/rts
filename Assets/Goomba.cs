using UnityEngine;
using RusticGames.Act;
using System.Collections;

public class Goomba : MonoBehaviour
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
			if (this.transform.position.x > 30) {
				Vector3 newPosition = this.transform.position;
				newPosition.x -= 20;
				this.transform.position = newPosition;
			}
			yield return new WaitForFixedUpdate ();
		}
	}
}
