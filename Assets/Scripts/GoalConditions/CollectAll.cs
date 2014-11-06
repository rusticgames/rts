using UnityEngine;
using System.Collections;

public class CollectAll : MonoBehaviour, IGoalCondition {

	public bool ConditionMet { get; set; }

	private GameObject[] collectables;

	void Start () {
		collectables = GameObject.FindGameObjectsWithTag("Collectable");
		StartCoroutine(CheckCondition());
	}
	
	IEnumerator CheckCondition () {
		while (!ConditionMet) {
			bool _conditionMet = true;
			foreach (var c in collectables) {
				Transform parent = c.transform.parent;
				if (parent == null || parent.tag != "Player") _conditionMet = false;
			}
			ConditionMet = _conditionMet;
			yield return null;
		}
	}
}