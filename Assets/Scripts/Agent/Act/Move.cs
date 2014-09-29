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
		public LegController walker;
		public Girdle pelvis;
		public Girdle clavicle;
		public LegsState legsPattern1;
		public LegsState legsPattern2;
		public LegsUse currentGoal;

		public IEnumerator moveRoutine (GameObject mover)
		{
			while (true) {
				if(moving) {
					currentGoal = LegsUse.ADVANCE;
				} else {
					currentGoal = LegsUse.BALANCE;
				}
				yield return StartCoroutine(LegController.walkRoutine(walker, pelvis.limbOne, pelvis.limbTwo));
			}
		}
		
		void OnDrawGizmos ()
		{
			LegController.drawLegGizmo (pelvis.limbOne);
			LegController.drawLegGizmo (pelvis.limbTwo);
			LegController.drawLegGizmo (clavicle.limbOne);
			LegController.drawLegGizmo (clavicle.limbTwo);
			/*velocity = this.rigidbody2D.velocity;
			Gizmos.color = Color.red;
			Gizmos.DrawRay (this.rigidbody2D.position, moveDirection);
			Gizmos.color = Color.green;
			Gizmos.DrawRay (this.rigidbody2D.position, velocity);*/
		}
	}
}