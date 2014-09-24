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
		public bool biped = false;
		public LegController leftLeg;
		public LegController rightLeg;
		public LegsState legsPattern1;
		public LegsState legsPattern2;
		private float startingX;
		public float displacement;

		void Start ()
		{
			startingX = this.transform.position.x;
		}

		void Update() {		if(biped) {
				leftLeg.updateAngles();
				rightLeg.updateAngles();
			}
			displacement = startingX - this.transform.position.x;
		}

		public IEnumerator moveRoutine (GameObject mover)
		{
			while (true) {
				if(moving) {
					if(biped) {
						yield return StartCoroutine(LegController.walkRoutine(rightLeg, leftLeg));
					} else {
						mover.rigidbody2D.AddForce (moveDirection * moveForce, forceMode);
					}
				}
				yield return new WaitForFixedUpdate ();
			}
		}
		
		void OnDrawGizmos ()
		{
			if(biped) {
				LegController.drawLegGizmo (leftLeg);
				LegController.drawLegGizmo (rightLeg);
				Gizmos.color = Color.red;
				if(displacement < 0) Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(this.transform.position, this.displacement / 10f);
			}
			else {
				velocity = this.rigidbody2D.velocity;
				Gizmos.color = Color.red;
				Gizmos.DrawRay (this.rigidbody2D.position, moveDirection);
				Gizmos.color = Color.green;
				Gizmos.DrawRay (this.rigidbody2D.position, velocity);
			}
		}
	}
}