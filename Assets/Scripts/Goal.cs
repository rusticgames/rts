using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goal : MonoBehaviour {

	public bool allConditionsMet = false;

	private Component[] conditions;

	void Start () {
		conditions = GetComponents(typeof(IGoalCondition));
		StartCoroutine(Check());
	}

	IEnumerator Check () {
		while (!allConditionsMet) {
			bool _allConditionsMet = true;
			for (var i = 0; i < conditions.Length; i++) {
				if (!((IGoalCondition)conditions[i]).ConditionMet) _allConditionsMet = false;
			}
			allConditionsMet = _allConditionsMet;
			yield return null;
		}
		Debug.Log("Ya fuckin' won!");
	}
}

public interface IGoalCondition
{
	bool ConditionMet { get; set; }
}