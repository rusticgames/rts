using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGoal : MonoBehaviour {
	public UnityEngine.UI.Text text;
	public Transform heightDefiner;
	public List<History> players = new List<History>();
	private List<History> winners = new List<History>();
	public float checkInterval = 2f;

	void Start() {
		winners.AddRange(players);
		text.text = "Don't fall!";
		players.ForEach(h => h.OnStateSample.AddListener(heightTest));
		StartCoroutine(checkConditions());
	}

	void heightTest(History state, float timestamp) {
			if(state.transform.position.y < this.heightDefiner.position.y) {
				this.winners.Remove(state);
			}
	}
	
	IEnumerator checkConditions() {
		while(true) {
			if(winners.Count == 1)
			{
				text.text = winners[0].name + " won!";
			}

			if(winners.Count == 0)
			{
				text.text = "Everyone lost!";
			}
			yield return new WaitForSeconds(this.checkInterval);
		}
}
	

}
