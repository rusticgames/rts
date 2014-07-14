using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Element
{
	PROJECTILE,
	TERRAIN,
	UNIT
}

public enum InteractionResult {
	DIE,
	ATTACH,
	BOUNCE
}

[System.Serializable]
public class ElementResultMapping {
	public Element e;
	public InteractionResult r;
}


public class PhysicalComposition : MonoBehaviour {
	public List<Element> elements = new List<Element>();
	public List<ElementResultMapping> reactions = new List<ElementResultMapping>();
	public Dictionary<Element, InteractionResult> a = new Dictionary<Element, InteractionResult>();
	public bool DEBUG_MODE = false;
	
	// Start is called just before any of the
	// Update methods is called the first time.
	void Start () {
		reactions.ForEach(m => a.Add(m.e, m.r));
	}

	/*IEnumerable processUpdates() {
		while(SelectUnits.GAME_IS_RUNNING) {
			pendingChanges.ForEach(x => 
			yield return WaitForFixedUpdate();
		}
	}*/


	void processCollision (Element x, Collision collision)
	{
		if(DEBUG_MODE) Debug.Log("Result of: [" + this.gameObject.name + "] hit by [" + collision.collider.gameObject.name + "]");
		if(! a.ContainsKey(x)) {return;}

		switch (a[x]) {
		case InteractionResult.DIE:
			GameObject.Destroy(this.gameObject);
			if(DEBUG_MODE) Debug.Log("Die");
			break;
		case InteractionResult.BOUNCE:
			if(DEBUG_MODE) Debug.Log("Bounce");
			break;
		case InteractionResult.ATTACH:
			if(collision.collider.rigidbody != null && this.transform != collision.collider.transform.parent) 
			{
				if(DEBUG_MODE) Debug.Log("Attach");
				GameObject.Destroy(this.rigidbody);
				this.transform.parent = collision.collider.transform;
			}
			break;
		default:
			if(DEBUG_MODE) Debug.Log("nah");
			break;
		}
	}
	
	// OnCollisionEnter is called when this
	// collider/rigidbody has begun touching
	// another rigidbody/collider.
	void OnCollisionEnter (Collision collision) {
		PhysicalComposition pc = collision.collider.GetComponent<PhysicalComposition>();
		//uncomment the following line to cease assuming that all colliders have a physical composition
		//if(pc == null) {return;}
		pc.elements.ForEach(x => this.processCollision(x, collision));
	}
	
	// Implement this OnDrawGizmos if you want
	// to draw gizmos that are also pickable
	// and always drawn.
	void OnDrawGizmos () {
		
	}
	
}
