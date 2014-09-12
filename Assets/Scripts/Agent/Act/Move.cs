using UnityEngine;
using System.Collections;

namespace RusticGames.Act
{
	public class Move : MonoBehaviour
	{
		public Vector2 moveDirection;
		public float moveForce;
		public Vector2 velocity;
		public ForceMode2D forceMode;
		public bool moving = false;

		void Start ()
		{
		}

		public IEnumerator moveRoutine (GameObject mover)
		{
			while (true) {
				if(moving) {
					mover.rigidbody2D.AddForce (moveDirection * moveForce, forceMode);
				}
				yield return new WaitForFixedUpdate ();
			}
		}
		
		void OnDrawGizmos ()
		{
			velocity = this.rigidbody2D.velocity;
			Gizmos.color = Color.red;
			Gizmos.DrawRay (this.rigidbody2D.position, moveDirection);
			Gizmos.color = Color.green;
			Gizmos.DrawRay (this.rigidbody2D.position, velocity);
		}
	}
}