using UnityEngine;
using RusticGames.Act;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class InteractionResultMapping {
	public InteractorType element;
	public InteractionResult reaction;
}

public class GeneralCollider {
	public bool is3d = true;
	public Collider collider3d;
	public Collider2D collider2d;
	public GameObject go;
	public GeneralCollider(Collider c) { is3d = true; collider3d = c; go = c.gameObject; }
	public GeneralCollider(Collider2D c) { is3d = false; collider2d = c; go = c.gameObject; }
}

public class Interaction : MonoBehaviour {
	public List<InteractionResultMapping> reactions = new List<InteractionResultMapping>();
	public Dictionary<InteractorType, InteractionResult> a = new Dictionary<InteractorType, InteractionResult>();
	public Change changeTarget;
	
	public void logInteraction(InteractionResult r, GameObject g) {
		Debug.LogWarning ("<" + this + "> performing <" + r + "> because of <" + g + ">");
	}
	
	public void logInteractionCheck(InteractorType t) {
		Debug.Log ("<" + this + "> checking interaction with <" + t + "> on behalf of <" + changeTarget + ">");
	}
	
	public void logCollision(GameObject go) {
		Debug.Log ("<" + changeTarget + "> (me: <" + this + ">) collided with <" + go + ">");
	}

	void Reset () {
		a[InteractorType.DEFAULT] = InteractionResult.NONE;
		changeTarget = this.GetComponent<Change>();
		if(changeTarget == null) {
			changeTarget = this.GetComponentInParent<Change>();
		}
	}

	void Start () {
		a[InteractorType.DEFAULT] = InteractionResult.NONE;
		reactions.ForEach(m => a[m.element] = m.reaction);
	}

	void processCollisions (Composition c, GeneralCollider gc)
	{
		logCollision(gc.go);
		if(c == null) { return; };
		c.elements.ForEach(x => this.processCollision(x, gc));
	}
	
	void processCollision (InteractorType x, GeneralCollider gc)
	{
		if(a.ContainsKey(x)) {
			logInteraction(a[x], gc.go);
			changeTarget.addChange(a [x]);
			return;
		}
		
		if(a.ContainsKey(InteractorType.DEFAULT)) {
			changeTarget.addChange(a [InteractorType.DEFAULT]);
		}
	}

	void OnCollisionEnter (Collision collision) {
		processCollisions(collision.collider.GetComponent<Composition>(), new GeneralCollider (collision.collider));
	}
	void OnCollisionEnter2D (Collision2D collision) {
		processCollisions(collision.collider.GetComponent<Composition>(), new GeneralCollider (collision.collider));
	}
	void OnTriggerEnter2D(Collider2D collider) {
		processCollisions(collider.GetComponent<Composition>(), new GeneralCollider (collider));
	}
	void OnTriggerEnter(Collider collider) {
		processCollisions(collider.GetComponent<Composition>(), new GeneralCollider (collider));
	}
}
