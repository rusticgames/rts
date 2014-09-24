// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RusticGames.Act
{
	[System.Serializable]
	public class LegsState {
		public LegState advancer;
		public LegState helper;
		public bool pauseOnStateChange;
	}
	
	[System.Serializable]
	public class LegState {
		public MotorMotion thigh;
		public MotorMotion shin;
	}
	
	public enum MotorMotion { FORWARD, SET, BACKWARD, FREE }

	[System.Serializable]
	public class LegController {
		public Move owner;
		public UnityEngine.UI.Text updatee;
		public GameObject hip;
		public HingeJoint2D hipToKnee;
		public HingeJoint2D kneeToFoot;
		public HingeJoint2D foot;
		public float hipToKneeTargetAngle = -45f;
		public float kneeToFootTargetAngle = 0f;
		public JointMotor2D walkMotor;
		public JointMotor2D opposedMotor;
		public JointMotor2D setMotor;
		public float desiredWalkPower;
		public float maxWalkForce;
		public string lastHipToKneeAngle;
		public string lastKneeToFootAngle;
		public string state = "Precreation";
		public Color gizmoColor;

		public static void drawLegGizmo (LegController leg)
		{
			Gizmos.color = leg.gizmoColor;
			Gizmos.DrawLine (leg.hip.transform.position, leg.hipToKnee.transform.position);
			Gizmos.DrawLine (leg.hipToKnee.transform.position, leg.kneeToFoot.transform.position);
			Gizmos.DrawLine (leg.kneeToFoot.transform.position, leg.foot.transform.position);
		}
		
		public static void updateMotor (LegController l, HingeJoint2D j, MotorMotion m) {
			switch (m) {
			case	MotorMotion.FREE:
				j.useMotor = false;
				break;
			case MotorMotion.BACKWARD:
				j.useMotor = true;
				j.motor = l.opposedMotor;
				break;
			case MotorMotion.FORWARD:
				j.useMotor = true;
				j.motor = l.walkMotor;
				break;
			case MotorMotion.SET:
				j.useMotor = true;
				j.motor = l.setMotor;
				break;
			default:
				break;
			}
		}
		public static void updateMotors (LegController l, MotorMotion h2k, MotorMotion k2f) {
			updateMotor(l,l.hipToKnee,h2k);
			updateMotor(l,l.kneeToFoot,k2f);
			l.state = "HipKnee: " + h2k + ", KneeFoot: " + k2f;
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
		
		public static IEnumerator advanceOneLeg (LegController advancingLeg, LegController helpingLeg)
		{
			yield return new WaitForFixedUpdate ();
			if(advancingLeg.owner.legsPattern1.pauseOnStateChange) {
				Debug.Log("Breaking for change to pattern 1");
				Debug.Break();
				yield return new WaitForFixedUpdate ();
			}
			LegController.updateMotors (advancingLeg, advancingLeg.owner.legsPattern1.advancer.thigh, advancingLeg.owner.legsPattern1.advancer.shin);
			LegController.updateMotors (helpingLeg, helpingLeg.owner.legsPattern1.helper.thigh, helpingLeg.owner.legsPattern1.helper.shin);
			while (advancingLeg.hipToKnee.jointAngle - 1f > advancingLeg.hipToKneeTargetAngle) {
				yield return new WaitForFixedUpdate ();
			}
			
			if(advancingLeg.owner.legsPattern2.pauseOnStateChange) {
				Debug.Log("Breaking for change to pattern 2");
				Debug.Break();
				yield return new WaitForFixedUpdate ();
			}
			
			LegController.updateMotors (advancingLeg, advancingLeg.owner.legsPattern2.advancer.thigh, advancingLeg.owner.legsPattern2.advancer.shin);
			LegController.updateMotors (helpingLeg, helpingLeg.owner.legsPattern2.helper.thigh, helpingLeg.owner.legsPattern2.helper.shin);
			while (advancingLeg.kneeToFoot.jointAngle - 1f > advancingLeg.kneeToFootTargetAngle) {
				yield return new WaitForFixedUpdate ();
			}
		}
		
		public static IEnumerator walkRoutine(LegController leg1,LegController leg2) {
			LegController.createMotors(leg1);
			LegController.createMotors(leg2);
			
			leg1.updatee.color = leg1.gizmoColor;
			leg2.updatee.color = leg2.gizmoColor;
			
			//Debug.Log("Breaking for start of walk");
			//Debug.Break();
			
			yield return leg1.owner.StartCoroutine(advanceOneLeg (leg1, leg2));
			yield return leg2.owner.StartCoroutine(advanceOneLeg (leg2, leg1));
			
			yield return null;
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
	
}