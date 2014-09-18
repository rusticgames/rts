using UnityEngine;
using RusticGames.Act;
using System.Collections;
using System.Collections.Generic;

public enum InteractorType
{
	PROJECTILE,
	TERRAIN,
	UNIT,
	GOOMBA,
	SIDE,
	MUSHROOM,
	BRICK,
	PRIZE,
	ONEUP,
	MARIO,
	MARIO_HEAD,
	MARIO_BOOTS,
	DEFAULT
}

public enum InteractionResult {
	DIE,
	ATTACH,
	BOUNCE,
	TURN,
	NONE,
	GROW,
	SPAWN_MUSHROOM
}

[System.Serializable]
public class InteractionResultMapping {
	public InteractorType e;
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
	private static List<InteractorType> defaultElement = new List<InteractorType>(){InteractorType.DEFAULT};
	public List<InteractorType> elements = new List<InteractorType>();
	public List<InteractionResultMapping> reactions = new List<InteractionResultMapping>();
	public Dictionary<InteractorType, InteractionResult> a = new Dictionary<InteractorType, InteractionResult>();
	public Dictionary<InteractionResult, InteractionLogic> b = new Dictionary<InteractionResult, InteractionLogic>();
	public GameObject me;
	
	public void logInteraction(InteractionResult r, GameObject g) {
		Debug.Log ("<" + me + "> performing <" + r + "> because of <" + g + ">");
	}
	
	public void logInteractionCheck(InteractorType t) {
		Debug.Log ("<" + me + "> checking interaction between my <" + this + "> and <" + t + ">");
	}

	public delegate void InteractionLogic(GeneralCollider c);
	void Start () {
		if (me == null) {
						me = gameObject;
				}
		reactions.ForEach(m => a.Add(m.e, m.r));

		b.Add(InteractionResult.DIE, delegate(GeneralCollider c) {
			logInteraction(InteractionResult.DIE, c.collider2d.gameObject);
			GameObject.Destroy(me, 0.001f); 
		});

		b.Add(InteractionResult.BOUNCE, delegate(GeneralCollider c) { 
			logInteraction(InteractionResult.BOUNCE, c.collider2d.gameObject);
			if(c.is3d) {
				Vector3 v = me.rigidbody.velocity;
				v.y = 0f;
				me.rigidbody.velocity = v;
				me.rigidbody.AddForce(Vector2.up * 2.5f, ForceMode.Impulse);
			} else 
			{
				Vector2 v = me.rigidbody2D.velocity;
				v.y = 0f;
				me.rigidbody2D.velocity = v;
				me.rigidbody2D.AddForce(Vector2.up * 2.5f, ForceMode2D.Impulse);
			}
		});

		b.Add (InteractionResult.ATTACH, delegate(GeneralCollider c) {
			logInteraction(InteractionResult.ATTACH, c.collider2d.gameObject);
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
		
		b.Add(InteractionResult.TURN, delegate(GeneralCollider c) {
			logInteraction(InteractionResult.TURN, c.collider2d.gameObject);
			me.GetComponent<Move>().moveDirection.x = -me.GetComponent<Move>().moveDirection.x;
		});
		
		b.Add(InteractionResult.GROW, delegate(GeneralCollider c) {
			logInteraction(InteractionResult.GROW, c.collider2d.gameObject);
			Vector3 scale = me.transform.localScale;
			scale.y *=2;
			me.transform.localScale = scale;
		});

		b.Add(InteractionResult.SPAWN_MUSHROOM, delegate(GeneralCollider c) {
			logInteraction(InteractionResult.SPAWN_MUSHROOM, c.collider2d.gameObject);
			spawn("Mushroom");
		});

		b.Add(InteractionResult.NONE, delegate(GeneralCollider c) { return;	});
	}

	void spawn (string resource) {
		GameObject spawned = Instantiate(Resources.Load<GameObject>(resource)) as GameObject;
		Vector3 pos = me.transform.position;
		pos.y = pos.y + 1.0f;
		spawned.transform.position = pos;
	}

	void processCollisions (Interaction i, GeneralCollider c)
	{
		List<InteractorType> iList;
		if(i == null) { iList = defaultElement; } else {iList = i.elements;}
		iList.ForEach(x => this.processCollision(x, c));
	}
	
	void processCollision (InteractorType x, GeneralCollider c)
	{
		logInteractionCheck(x);
		if(a.ContainsKey(x)) {
			b [a [x]] (c);
			return;
		}

		if(a.ContainsKey(InteractorType.DEFAULT)) {
			b[a[InteractorType.DEFAULT]](c);
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
