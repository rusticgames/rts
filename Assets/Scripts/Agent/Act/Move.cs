using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RusticGames.Act
{
	[System.Serializable]
	public class LegController {
		public HingeJoint2D hipToKnee;
		public HingeJoint2D kneeToFoot;
		public JointMotor2D walkMotor;
		public JointMotor2D opposedMotor;
		public JointMotor2D setMotor;
		public float desiredWalkPower;
		public float maxWalkForce;
		public string lastHipToKneeAngle;
		public string lastKneeToFootAngle;
		public string state = "Precreation";
		
		public static void moveOpposed(LegController leg) {
			leg.hipToKnee.motor = leg.walkMotor;
			leg.kneeToFoot.motor = leg.opposedMotor;
			leg.state = "moveOpposed";
		}
		
		public static void moveBack(LegController leg) {
			leg.hipToKnee.motor = leg.opposedMotor;
			leg.kneeToFoot.motor = leg.walkMotor;
			leg.state = "moveBack";
		}
		
		public static void advanceShin(LegController leg) {
			leg.hipToKnee.motor = leg.setMotor;
			leg.kneeToFoot.motor = leg.walkMotor;
			leg.state = "advanceShin";
		}
		
		public static void set(LegController leg) {
			leg.hipToKnee.motor = leg.setMotor;
			leg.kneeToFoot.motor = leg.setMotor;
			leg.state = "set";
		}

		public static void createMotors(LegController l){
			l.walkMotor.maxMotorTorque = l.maxWalkForce;
			l.walkMotor.motorSpeed = -l.desiredWalkPower;
			l.opposedMotor.maxMotorTorque = l.maxWalkForce;
			l.opposedMotor.motorSpeed = l.desiredWalkPower;
			l.setMotor.maxMotorTorque = l.maxWalkForce;
			l.setMotor.motorSpeed = 0;
			l.state = "Created";
		}

		public void updateAngles() {
			lastHipToKneeAngle = "<" + hipToKnee.limits.min + "> " + hipToKnee.jointAngle  + "/" + hipToKnee.referenceAngle + " <" + hipToKnee.limits.max + "> ";
			lastKneeToFootAngle = "<" + kneeToFoot.limits.min + "> " + kneeToFoot.jointAngle + "/" + kneeToFoot.referenceAngle + " <" + kneeToFoot.limits.max + "> ";
		}


		public static IEnumerator walk(LegController leg1,LegController leg2) {
			LegController.createMotors(leg1);
			LegController.createMotors(leg2);
			LegController.set (leg2);
			while(true) {
				LegController.moveOpposed (leg1);
				while (leg1.hipToKnee.jointAngle > leg1.hipToKnee.limits.min) {
					yield return new WaitForFixedUpdate ();
				}
				//LegController.moveBack(leg2);
				LegController.advanceShin (leg1);
				while (leg1.kneeToFoot.jointAngle > leg1.kneeToFoot.limits.min) {
					yield return new WaitForFixedUpdate ();
				}
				LegController.set (leg1);
				LegController.moveOpposed (leg2);
				while (leg2.hipToKnee.jointAngle > leg2.hipToKnee.limits.min) {
					yield return new WaitForFixedUpdate ();
				}
				LegController.moveBack(leg1);
				LegController.advanceShin (leg2);
				while (leg2.kneeToFoot.jointAngle > leg2.kneeToFoot.limits.min) {
					yield return new WaitForFixedUpdate ();
				}
				LegController.set (leg2);
				yield return null;
				//yield return moveLeg (leg1);
				//yield return moveLeg (leg2);
			}
		}
		public void lift() {
			/*
			 *       o
			 *      * 
			 *     /- 
			 *    / / 
			 *   /    
			 *        
			 *       o
			 *      * 
			 *     /- 
			 *   _/ | 
			 *       
			 *        
			 *       o
			 *      * 
			 *     /  
			 *   _| \ 
			 *      | 
			 *        
			 *       o
			 *      * 
			 *     /- 
			 *    / / 
			 *   /    
			 */
		}
	}

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

		void Start ()
		{
		}

		void Update() {		if(biped) {
				leftLeg.updateAngles();
				rightLeg.updateAngles();
			}
		}

		public IEnumerator moveRoutine (GameObject mover)
		{
			while (true) {
				if(moving) {
					if(biped) {
						yield return StartCoroutine(LegController.walk(rightLeg, leftLeg));
					} else {
						mover.rigidbody2D.AddForce (moveDirection * moveForce, forceMode);
					}
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