using UnityEngine;
using RusticGames.Act;
using System.Collections;
using System.Collections.Generic;

public class Change : MonoBehaviour {
	private HashSet<InteractionResult> results;
	public Dictionary<InteractionResult, InteractionLogic> b = new Dictionary<InteractionResult, InteractionLogic>();
	public GameObject me;

	public delegate void InteractionLogic();
	void Start ()
	{
		results = new HashSet<InteractionResult>();
		b.Add(InteractionResult.DIE, delegate() {
			GameObject.Destroy(me);
		});
		
		b.Add(InteractionResult.BOUNCE, delegate() { 
			Vector2 v = me.rigidbody2D.velocity;
			v.y = 0f;
			me.rigidbody2D.velocity = v;
			me.rigidbody2D.AddForce(Vector2.up * 2.5f, ForceMode2D.Impulse);
		});
		
		b.Add(InteractionResult.TURN, delegate() {
			me.GetComponent<Move>().moveDirection.x = -me.GetComponent<Move>().moveDirection.x;
		});
		
		b.Add(InteractionResult.GROW, delegate() {
			Vector3 scale = me.transform.localScale;
			scale.y += 1;
			me.transform.localScale = scale;
		});
		
		b.Add(InteractionResult.NONE, delegate() { return;	});
		StartCoroutine (processChanges ());
	}

	public void addChange(InteractionResult r) {
		results.Add(r);
	}
	
	IEnumerator processChanges ()
	{
		while(true) {
			if(results.Count > 0) {
				Debug.LogWarning("<" + me + "> starting result processing");
				foreach (var result in results) {
					Debug.LogWarning("<" + me + "> starting <" + result + ">");
					if(b.ContainsKey(result)) {					
						b[result]();
						Debug.LogWarning("<" + me + "> done <" + result + ">");
					}
				}
				results.Clear();
				Debug.LogWarning("<" + me + "> done result processing");
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
