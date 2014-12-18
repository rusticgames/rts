using UnityEngine;
using System.Collections;

public class ReachObject : MonoBehaviour , IGoalCondition {
	public bool ConditionMet { get; set; }
	public GameObject objectToReach;

	void Start() {
		ConditionMet = true;
	}

	void Update() {
		
	}
}