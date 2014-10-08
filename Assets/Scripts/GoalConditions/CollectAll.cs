using UnityEngine;
using System.Collections;

public class CollectAll : MonoBehaviour, IGoalCondition {

	public bool ConditionMet { get; set; }

	private GameObject[] collectables;

	void Start () {
		collectables = GameObject.FindGameObjectsWithTag("Collectable");
		StartCoroutine(CheckForConditionMet());
	}
	
	IEnumerator CheckForConditionMet () {
		while (!ConditionMet) {
			foreach (var c in collectables) {
				Transform cParent = c.transform.parent;
				if (cParent == null || cParent.tag != "Player") {
					ConditionMet = false;
					yield return null;
				} else {
					ConditionMet = true;
				}
			}
		}
	}
}