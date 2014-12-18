using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RusticGames.Act
{

	public class Move : MonoBehaviour
	{
		public Vector2 moveDirection;
		public float moveForce;
		public Vector2 velocity;
		public ForceMode2D forceMode;
		public bool moving = false;

		public IEnumerator moveRoutine (GameObject mover)
		{
			while (true) {
				yield return new WaitForSeconds(1f);
			}
		}
		
		void OnDrawGizmos ()
		{
		}
	}
}