using UnityEngine;
using RusticGames.Act;
using System.Collections;
using System.Collections.Generic;

public class Change : MonoBehaviour {
	private HashSet<InteractionResult> results = new HashSet<InteractionResult>();
	public Dictionary<InteractionResult, InteractionLogic> b = new Dictionary<InteractionResult, InteractionLogic>();
	public GameObject me;
	public bool readManualChange;
	public InteractionResult manualResult = InteractionResult.NONE;

	public delegate void InteractionLogic();
	void Reset ()
	{
		manualResult = InteractionResult.NONE;
		results = new HashSet<InteractionResult>();
		me = this.gameObject;
	}
	void Start ()
	{
		b.Add(InteractionResult.DIE, die);
		b.Add(InteractionResult.BOUNCE, bounce);
		b.Add(InteractionResult.TURN, turn);
		b.Add(InteractionResult.GROW, grow);
		b.Add(InteractionResult.SHRINK, shrink);
		b.Add (InteractionResult.SPAWN, spawn);
		b.Add(InteractionResult.NONE, delegate() { return;	});
		StartCoroutine (processChanges ());
	}

	void spawn ()
	{
		me.GetComponent<Spawner> ().spawn();
	}

	void die ()
	{
		if(isBig()) {
			shrink();
		} else {
			GameObject.Destroy (me);
		}
	}

	void bounce ()
	{
		Vector2 v = me.rigidbody2D.velocity;
		v.y = 0f;
		me.rigidbody2D.velocity = v;
		me.rigidbody2D.AddForce (Vector2.up * 2.5f, ForceMode2D.Impulse);
	}

	void turn ()
	{
		me.GetComponent<Move> ().moveDirection.x = -me.GetComponent<Move> ().moveDirection.x;
	}

	bool isBig() {
		return me.transform.localScale.y == 2f;
	}

	void grow ()
	{
		if (!isBig()) {
			Vector3 scale = me.transform.localScale;
			scale.y = 2f;
			me.transform.localScale = scale;
			me.GetComponent<Move> ().moveForce *= 2f;
		}
	}

	void shrink ()
	{
		if (isBig()) {
			Vector3 scale = me.transform.localScale;
			scale.y = 1;
			me.transform.localScale = scale;
			me.GetComponent<Move> ().moveForce *= 0.5f;
		}
	}

	public void addChange(InteractionResult r) {
		results.Add(r);
	}
	
	IEnumerator processChanges ()
	{
		while(true) {
			if(readManualChange) {
				b[manualResult]();
				readManualChange = false;
			}
			if(results.Count > 0) {
				foreach (var result in results) {
					if(b.ContainsKey(result)) {					
						b[result]();
					}
				}
				results.Clear();
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
