using UnityEngine;
using System.Collections;
public enum LEG_STATE { DEFAULT, LIFTING, LOWERING, JUMPING }
[System.Serializable]
public struct LegData {
	public float maxJumpSpeed;
	public float maxJumpForce;
	public float maxLiftSpeed;
	public float maxLiftForce;
	public float maxFootAdvanceSpeed;
	public float maxFootAdvanceForce;
}
public class SimpleLeg : MonoBehaviour {
	public SliderJoint2D thigh;
	public SliderJoint2D foot;
	public float jumpFactor;
	public float liftFactor;
	public float footAdvanceFactor;
	public LegData legData;
	JointMotor2D liftMotor;
	JointMotor2D jumpMotor;
	JointMotor2D lowerMotor;
	JointMotor2D advanceMotor;
	JointMotor2D retractMotor;
	public LEG_STATE legState = LEG_STATE.DEFAULT;
	public bool isFullyLifted() {
		return (thigh.limitState == JointLimitState2D.LowerLimit);
	}
	public bool isFullyLowered() {
		return (thigh.limitState == JointLimitState2D.UpperLimit);
	}
	public bool isAdvanced() {
		return (foot.limitState == JointLimitState2D.UpperLimit);
	}
	public bool isAdvancedOpposed() {
		return (foot.limitState == JointLimitState2D.LowerLimit);
	}
	public bool isPastCenterOfMass() {
		return (foot.jointTranslation > 0f);
	}
	public bool isPastCenterOfMassOpposed() {
		return (foot.jointTranslation < 0f);
	}


	// Use this for initialization
	void Start () {
		liftMotor.maxMotorTorque = legData.maxLiftForce;
		liftMotor.motorSpeed = -legData.maxLiftSpeed;
		lowerMotor.maxMotorTorque = legData.maxLiftForce;
		lowerMotor.motorSpeed = legData.maxLiftSpeed;
		jumpMotor.maxMotorTorque = legData.maxJumpForce;
		jumpMotor.motorSpeed = legData.maxJumpSpeed;
		advanceMotor.maxMotorTorque = legData.maxFootAdvanceForce;
		advanceMotor.motorSpeed = legData.maxFootAdvanceSpeed;
		retractMotor.maxMotorTorque = legData.maxFootAdvanceForce;
		retractMotor.motorSpeed = -legData.maxFootAdvanceSpeed;
		jumpFactor = 1.0f;
		liftFactor = 1.0f;
		footAdvanceFactor = 1.0f;
	}
	
	public void lift() {
		legState = LEG_STATE.LIFTING;
		thigh.motor = liftMotor;
		thigh.useMotor = true;
	}
	
	public void jump() {
		legState = LEG_STATE.JUMPING;
		thigh.motor = jumpMotor;
		thigh.useMotor = true;
	}
	
	public void lower() {
		legState = LEG_STATE.LOWERING;
		thigh.motor = lowerMotor;
		thigh.useMotor = true;
	}
	
	public void advance () {
		foot.motor = advanceMotor;
		foot.useMotor = true;
	}
	
	public void advanceOpposed () {
		foot.motor = retractMotor;
		foot.useMotor = true;
	}
	
	public void relaxFoot () {
		foot.useMotor = false;
	}
	
	public void relaxThigh () {
		thigh.useMotor = false;
	}

	// Update is called once per frame
	void Update () {
		liftMotor.maxMotorTorque = legData.maxLiftForce;
		liftMotor.motorSpeed = -legData.maxLiftSpeed * liftFactor;
		lowerMotor.maxMotorTorque = legData.maxLiftForce;
		lowerMotor.motorSpeed = legData.maxLiftSpeed * liftFactor;
		jumpMotor.maxMotorTorque = legData.maxJumpForce;
		jumpMotor.motorSpeed = legData.maxJumpSpeed * jumpFactor;
		advanceMotor.maxMotorTorque = legData.maxFootAdvanceForce;
		advanceMotor.motorSpeed = legData.maxFootAdvanceSpeed * footAdvanceFactor;
		retractMotor.maxMotorTorque = legData.maxFootAdvanceForce;
		retractMotor.motorSpeed = -legData.maxFootAdvanceSpeed * footAdvanceFactor;
	}

	
	//TODO: this will likely go all screwy on inclines
	public static float getYAxisDiff(SimpleLeg first, SimpleLeg other) {
		return first.thigh.transform.localPosition.y - other.thigh.transform.localPosition.y;
	}

	//TODO: this will likely go all screwy on inclines
	public static float getXAxisDiff(SimpleLeg first, SimpleLeg other) {
		return first.foot.transform.localPosition.x - other.foot.transform.localPosition.x;
	}
}
