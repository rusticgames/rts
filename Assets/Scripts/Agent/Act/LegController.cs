// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RusticGames.Act
{
	[System.Serializable]
	public class Leg {
		public GameObject root;
		public SliderJoint2D rootJoint;
		public HingeJoint2D rootToBend;
		public HingeJoint2D bendToEnd;
		public HingeJoint2D end;
		public Color gizmoColor;
		public string state = "Precreation";
	}

	[System.Serializable]
	public class LegsState {
		public LegState advancer;
		public LegState helper;
		public bool pauseOnStateChange;
	}
	
	[System.Serializable]
	public class BalanceControlInfo {
		public float desiredLegSeparation;
	}
	[System.Serializable]
	public class LegState {
		public MotorMotion thigh;
		public MotorMotion shin;
	}

	public enum LegsUse { BALANCE, BRACE, DUCK, WALK, RUN, ADVANCE, STABILITIZE, SLOW, PUSH }
	
	public enum MotorMotion { FORWARD, SET, BACKWARD, FREE }

	[System.Serializable]
	public class LegController {
		public Move owner;
		public float hipToKneeTargetAngle = -45f;
		public float kneeToFootTargetAngle = 0f;
		public JointMotor2D walkMotor;
		public JointMotor2D opposedMotor;
		public JointMotor2D setMotor;
		public float desiredWalkPower;
		public float maxWalkForce;
		public string lastHipToKneeAngle;
		public string lastKneeToFootAngle;
		public LegsUse currentUse;

		public static void drawLegGizmo (Leg leg)
		{
			Gizmos.color = leg.gizmoColor;
			Gizmos.DrawLine (leg.root.transform.position, leg.rootToBend.transform.position);
			Gizmos.DrawLine (leg.rootToBend.transform.position, leg.bendToEnd.transform.position);
			Gizmos.DrawLine (leg.bendToEnd.transform.position, leg.end.transform.position);
		}
		
		public static void updateMotor (LegController lc, HingeJoint2D j, MotorMotion m) {
			switch (m) {
			case	MotorMotion.FREE:
				j.useMotor = false;
				break;
			case MotorMotion.BACKWARD:
				j.useMotor = true;
				j.motor = lc.opposedMotor;
				break;
			case MotorMotion.FORWARD:
				j.useMotor = true;
				j.motor = lc.walkMotor;
				break;
			case MotorMotion.SET:
				j.useMotor = true;
				j.motor = lc.setMotor;
				break;
			default:
				break;
			}
		}
		public static void updateMotors (LegController lc, Leg l, MotorMotion h2k, MotorMotion k2f) {
			updateMotor(lc, l.rootToBend,h2k);
			updateMotor(lc, l.bendToEnd,k2f);
			l.state = "HipKnee: " + h2k + ", KneeFoot: " + k2f;
		}
		
		public static void createMotors(LegController l){
			l.walkMotor.maxMotorTorque = l.maxWalkForce;
			l.walkMotor.motorSpeed = -l.desiredWalkPower;
			l.opposedMotor.maxMotorTorque = l.maxWalkForce;
			l.opposedMotor.motorSpeed = l.desiredWalkPower;
			l.setMotor.maxMotorTorque = l.maxWalkForce;
			l.setMotor.motorSpeed = 0;
		}
		/*
		public static IEnumerator advanceOneLeg (LegController c, Leg advancingLeg, Leg helpingLeg)
		{
			yield return new WaitForFixedUpdate ();
			if(c.owner.legsPattern1.pauseOnStateChange) {
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
		}*/
		
		public static IEnumerator walkRoutine(LegController c, Leg leg1, Leg leg2) {
			LegController.createMotors(c);
			
			//yield return leg1.owner.StartCoroutine(advanceOneLeg (leg1, leg2));
			//yield return leg2.owner.StartCoroutine(advanceOneLeg (leg2, leg1));
			
			yield return null;
		}
	}
}