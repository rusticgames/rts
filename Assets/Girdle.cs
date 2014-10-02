using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MuscleControl {
	public float targetSpeed;
	public float maxTorque;
	private JointMotor2D realMotor;
	private JointMotor2D realOpposedMotor;
	public JointMotor2D motor {
		get {
			realMotor.maxMotorTorque = maxTorque;
			realMotor.motorSpeed = targetSpeed;
			return realMotor;
		}
	}
	public JointMotor2D opposedMotor{
		get {
			realOpposedMotor.maxMotorTorque = maxTorque;
			realOpposedMotor.motorSpeed = -targetSpeed;
			return realOpposedMotor;
		}
	}
}

public class Girdle : MonoBehaviour {
	public GameObject root;
	public RusticGames.Act.Leg limbOne;
	public RusticGames.Act.Leg limbTwo;
	public bool directHingeControl;
	public MuscleControl rootUpDown;
	public MuscleControl rootToBendHinge;
	public MuscleControl bendToEndHinge;

	// Update is called every frame, if the
	// MonoBehaviour is enabled.
	void Update () {
		if(directHingeControl) {
			limbOne.rootJoint.motor = rootUpDown.motor;
			limbTwo.rootJoint.motor = rootUpDown.opposedMotor;
			limbOne.rootToBend.motor = rootToBendHinge.motor;
			limbTwo.rootToBend.motor = rootToBendHinge.opposedMotor;
			limbOne.bendToEnd.motor = bendToEndHinge.motor;
			limbTwo.bendToEnd.motor = bendToEndHinge.opposedMotor;

		}
	}
	
	// Reset to default values.
	void Reset () {
		List<SliderJoint2D> joints = new List<SliderJoint2D>();
		this.GetComponentsInChildren<SliderJoint2D>(joints);

		if (joints.Count < 2) {
			Debug.LogError("These hips (or shoulders) are lying! Can't autoconfigure girdle.");
		}
		
		root = this.gameObject;
		collectLeg(limbOne, joints[0]);
		collectLeg(limbTwo, joints[1]);
		limbOne.gizmoColor = Color.yellow;
		limbTwo.gizmoColor = Color.cyan;
	}

	void collectLeg(RusticGames.Act.Leg l, SliderJoint2D hip) {
		List<HingeJoint2D> joints = new List<HingeJoint2D>();
		l.rootJoint = hip;
		l.root = hip.gameObject;
		hip.GetComponentsInChildren<HingeJoint2D>(joints);
		l.rootToBend = joints.Find(joint => (joint.name.Contains("HipToKnee") || joint.name.Contains("ShoulderToElbow")));
		joints.Remove(l.rootToBend);
		l.bendToEnd = joints.Find(joint => (joint.name.Contains("KneeToFoot") || joint.name.Contains("ElbowToHand")));
		joints.Remove(l.bendToEnd);
		l.end = joints[0];
	}
}
