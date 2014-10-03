using UnityEngine;
using System.Collections;

[System.Serializable]
public struct HingeData {
	public float referenceAngle;
	public JointLimitState2D limitState;
	public float jointSpeed;
	public float jointAngle;
	public float reactionTorqueForNextTenthOfASecond;
	public Vector2 reactionForceForNextTenthOfASecond;
	public float torqueForNextTenthOfASecond;
}

[System.Serializable]
public struct SliderData {
	public float referenceAngle;
	public JointLimitState2D limitState;
	public float jointSpeed;
	public float jointTranslation;
	public float reactionTorqueForNextTenthOfASecond;
	public Vector2 reactionForceForNextTenthOfASecond;
	public float forceForNextTenthOfASecond;
}

[System.Serializable]
public struct BodyData {
	public Vector2 worldCenterOfMass;
	public Vector2 centerOfMass;
	public float inertia;
	public float mass;
	public float rotation;
	public Vector2 position;
	public float angularVelocity;
	public Vector2 velocity;
	}

public class Investigator : MonoBehaviour {
	public Rigidbody2D body;
	public BodyData bodyState;
	public HingeJoint2D hinge;
	public HingeData hingeState;
	public SliderJoint2D slider;
	public SliderData sliderState;
	// Use this for initialization
	void Reset () {
		hinge = this.GetComponent<HingeJoint2D>();
		slider = this.GetComponent<SliderJoint2D>();
		body = this.GetComponent<Rigidbody2D>();
	}

	void Start () {
		if(hinge != null) {StartCoroutine(HingeUpdate());}
		if(body != null) {StartCoroutine(BodyUpdate());}
		if(slider != null) {StartCoroutine(SliderUpdate());}
	}
	
	public IEnumerator BodyUpdate() {
		while (true) {
			bodyState.velocity = body.velocity;
			bodyState.angularVelocity = body.angularVelocity;
			bodyState.position = body.position;
			bodyState.rotation = body.rotation;
			bodyState.mass = body.mass;
			bodyState.inertia = body.inertia;
			bodyState.centerOfMass = body.centerOfMass;
			bodyState.worldCenterOfMass = body.worldCenterOfMass;
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	public IEnumerator HingeUpdate() {
		while (true) {
			hingeState.torqueForNextTenthOfASecond =  hinge.GetMotorTorque(0.1f);
			hingeState.reactionForceForNextTenthOfASecond = hinge.GetReactionForce(0.1f);
			hingeState.reactionTorqueForNextTenthOfASecond = hinge.GetReactionTorque(0.1f);
			hingeState.jointAngle = hinge.jointAngle;
			hingeState.jointSpeed = hinge.jointSpeed;
			hingeState.limitState = hinge.limitState;
			hingeState.referenceAngle = hinge.referenceAngle;
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	public IEnumerator SliderUpdate() {
		while (true) {
			sliderState.forceForNextTenthOfASecond =  slider.GetMotorForce(0.1f);
			sliderState.jointTranslation = slider.jointTranslation;
			sliderState.jointSpeed = slider.jointSpeed;
			sliderState.limitState = slider.limitState;
			sliderState.referenceAngle = slider.referenceAngle;
			yield return new WaitForSeconds(0.1f);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
