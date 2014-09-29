using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Girdle : MonoBehaviour {
	public SliderJoint2D root;
	public RusticGames.Act.Leg limbOne;
	public RusticGames.Act.Leg limbTwo;
	
	// Start is called just before any of the
	// Update methods is called the first time.
	void Start () {
		
	}
	
	// Update is called every frame, if the
	// MonoBehaviour is enabled.
	void Update () {
		
	}
	
	// Reset to default values.
	void Reset () {
		List<SliderJoint2D> joints = new List<SliderJoint2D>();
		this.GetComponentsInChildren<SliderJoint2D>(joints);

		if (joints.Count < 3) {
			Debug.LogError("These hips (or shoulders) are lying! Can't autoconfigure girdle.");
		}
		
		root = this.gameObject.GetComponent<SliderJoint2D>();
		joints.Remove(root);
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
