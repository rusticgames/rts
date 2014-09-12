using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Interactor
{
	PROJECTILE,
	TERRAIN,
	UNIT,
	GOOMBA,
	MUSHROOM,
	BRICK,
	PRIZE,
	ONEUP,
	MARIO,
	MARIO_HEAD,
	MARIO_BOOTS
}

public enum InteractionResult {
	DIE,
	ATTACH,
	BOUNCE
}

[System.Serializable]
public class InteractionResultMapping {
	public Interactor e;
	public InteractionResult r;
}

public class GeneralCollider {
	public bool is3d = true;
	public Collider collider3d;
	public Collider2D collider2d;
	public GeneralCollider(Collider c) { is3d = true; collider3d = c;}
	public GeneralCollider(Collider2D c) { is3d = false; collider2d = c;}
}

public class Interaction : MonoBehaviour {
	public List<Interactor> elements = new List<Interactor>();
	public List<InteractionResultMapping> reactions = new List<InteractionResultMapping>();
	public Dictionary<Interactor, InteractionResult> a = new Dictionary<Interactor, InteractionResult>();
	public Dictionary<InteractionResult, InteractionLogic> b = new Dictionary<InteractionResult, InteractionLogic>();
	public GameObject me;
	public bool DEBUG_MODE = false;
	
	public delegate void InteractionLogic(GeneralCollider c);
	void Start () {
		if (me == null) {
						me = gameObject;
				}
		reactions.ForEach(m => a.Add(m.e, m.r));
		b.Add(InteractionResult.DIE, delegate(GeneralCollider c) { 
			Debug.Log ("DIE!");
			GameObject.Destroy(me, 0.01f); });
		b.Add(InteractionResult.BOUNCE, delegate(GeneralCollider c) { 
			Debug.Log ("BOUNCE!");
			if(c.is3d) {
				Vector3 v = me.rigidbody.velocity;
				v.y = 0f;
				me.rigidbody.velocity = v;
				me.rigidbody.AddForce(Vector2.up * 10.0f, ForceMode.Impulse);
			} else 
			{
				Vector2 v = me.rigidbody2D.velocity;
				v.y = 0f;
				me.rigidbody2D.velocity = v;
				me.rigidbody2D.AddForce(Vector2.up * 10.0f, ForceMode2D.Impulse);
			}
		});
		b.Add (InteractionResult.ATTACH, delegate(GeneralCollider c) {
			Debug.Log ("ATTACH!");
			if(c.is3d) {
				if(c.collider3d.rigidbody != null && me.transform != c.collider3d.transform.parent) 
				{
					GameObject.Destroy(me.rigidbody);
					me.transform.parent = c.collider3d.transform;
				}
			} else {
				if(c.collider2d.rigidbody2D != null && me.transform != c.collider2d.transform.parent) 
				{
					GameObject.Destroy(me.rigidbody2D);
					me.transform.parent = c.collider2d.transform;
				}
			}
		});
	}


	void processCollision (Interactor x, GeneralCollider c)
	{
		Debug.Log (this + " CHECKING INTERACTION! x: " + x + ", c:" + c);
		if(! a.ContainsKey(x)) {return;}
		Debug.Log (this + " INTERACTION! x: " + x + ", c:" + c);
		b [a [x]] (c);
	}

	void OnCollisionEnter (Collision collision) {
		Interaction pc = collision.collider.GetComponent<Interaction>();
		if(pc == null) {return;}

		GeneralCollider gc = new GeneralCollider (collision.collider);
		pc.elements.ForEach(x => this.processCollision(x, gc));
	}

	void OnCollisionEnter2D (Collision2D collision) {
		Interaction pc = collision.collider.GetComponent<Interaction>();
		if(pc == null) {return;}

		GeneralCollider gc = new GeneralCollider (collision.collider);
		pc.elements.ForEach(x => this.processCollision(x, gc));
	}
	void OnTriggerEnter2D(Collider2D collider) {
		Interaction pc = collider.GetComponent<Interaction>();
		if(pc == null) {return;}
		
		GeneralCollider gc = new GeneralCollider (collider);
		pc.elements.ForEach(x => this.processCollision(x, gc));
	}
	void OnTriggerEnter(Collider collision) {
		Interaction pc = collider.GetComponent<Interaction>();
		if(pc == null) {return;}
		
		GeneralCollider gc = new GeneralCollider (collider);
		pc.elements.ForEach(x => this.processCollision(x, gc));
	}
}
