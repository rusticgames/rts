using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RusticGames.Act
{
	[System.Serializable]
	public class LegController {
		public HingeJoint2D knee;
		public HingeJoint2D foot;
		public JointAngleLimits2D walkAngleKnee;
		public JointAngleLimits2D walkAngleFoot;
		public float desiredWalkPower;
		public float maxWalkForce;
	}

	public class Move : MonoBehaviour
	{
		public Vector2 moveDirection;
		public float moveForce;
		public Vector2 velocity;
		public ForceMode2D forceMode;
		public bool moving = false;
		public bool biped = false;
		public List<LegController> legs;

		void Start ()
		{
		}

		public IEnumerator moveRoutine (GameObject mover)
		{
			while (true) {
				if(moving) {
					if(biped) {
						if(leg.
					}

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