using UnityEngine;
using RusticGames.Act;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class InteractionResultMapping {
	public InteractorType e;
	public InteractionResult r;
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
	private static List<InteractorType> defaultElement = new List<InteractorType>(){InteractorType.DEFAULT};
	public List<InteractorType> elements = new List<InteractorType>();
	public List<InteractionResultMapping> reactions = new List<InteractionResultMapping>();
	public Dictionary<InteractorType, InteractionResult> a = new Dictionary<InteractorType, InteractionResult>();
	public GameObject me;
	public Change changeTarget;
	
	public void logInteraction(InteractionResult r, GameObject g) {
		Debug.LogWarning ("<" + me + "> performing <" + r + "> because of <" + g + ">");
	}
	
	public void logInteractionCheck(InteractorType t) {
		Debug.Log ("<" + me + "> checking interaction between my <" + this + "> and <" + t + ">");
	}
	
	public void logCollision(GameObject go) {
		Debug.Log ("<" + this + "> (me: <" + me + ">) collided with <" + go + ">");
	}

	void Start () {
		if (me == null) {
						me = gameObject;
				}
		if(changeTarget == null) {
			changeTarget = me.GetComponent<Change>();
		}
		reactions.ForEach(m => a.Add(m.e, m.r));
	}

	void processCollisions (Interaction i, GeneralCollider c)
	{
		logCollision(c.go);
		List<InteractorType> iList;
		if(i == null) { iList = defaultElement; } else {iList = i.elements;}
		iList.ForEach(x => this.processCollision(x, c));
	}
	
	void processCollision (InteractorType x, GeneralCollider c)
	{
		logInteractionCheck(x);
		if(a.ContainsKey(x)) {
			logInteraction(a[x], c.go);
			changeTarget.addChange(a [x]);
			return;
		}

		if(a.ContainsKey(InteractorType.DEFAULT)) {
			Debug.Log(changeTarget);
			changeTarget.addChange(a [InteractorType.DEFAULT]);
		}
	}

	void OnCollisionEnter (Collision collision) {
		processCollisions(collision.collider.GetComponent<Interaction>(), new GeneralCollider (collision.collider));
	}
	void OnCollisionEnter2D (Collision2D collision) {
		processCollisions(collision.collider.GetComponent<Interaction>(), new GeneralCollider (collision.collider));
	}
	void OnTriggerEnter2D(Collider2D collider) {
		processCollisions(collider.GetComponent<Interaction>(), new GeneralCollider (collider));
	}
	void OnTriggerEnter(Collider collider) {
		processCollisions(collider.GetComponent<Interaction>(), new GeneralCollider (collider));
	}
}
